using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Util.Sincronizacao;

namespace VandaModaIntimaWpf.ViewModel
{
    public class SincronizacaoViewModel : ObservableObject, IDisposable
    {
        public static Socket ClientSocket;

        private static readonly string VandaModaIntimaLocalAppDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Vanda Moda Íntima");
        private static readonly string StatementLogFile = Path.Combine(VandaModaIntimaLocalAppDir, "StatementLog.json");
        private static readonly string LastSyncFile = Path.Combine(VandaModaIntimaLocalAppDir, "LastSync.txt");
        private string _messageReceived = "";
        private readonly byte[] _bytesReceivedFromServer = new byte[1024];
        private string _textLog;
        private bool _disposed;
        private static readonly string LocalDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
        private static object _this; // Guarda referência à própria classe para ser usado em Reflection dentro de método estático
        private static List<StatementLog> _statementLogs; //Guarda os logs presentes no arquivo de logs
        private static List<StatementLog> _duplicateStatementLogs;
        public static List<StatementLog> TransientWriteStatementLogs; //Guarda os logs que ainda não foram escritos
        public static List<StatementLog> TransientSendStatementLogs; //Guarda os logs que ainda não foram enviados
        public static DateTime LastSync = new DateTime(2018, 1, 1, 0, 0, 0);
        private readonly string _connectionString = "SERVER=localhost;DATABASE=vandamodaintima;UID=root;PASSWORD=1124";

        public SincronizacaoViewModel()
        {
            if (!Directory.Exists(VandaModaIntimaLocalAppDir))
            {
                Directory.CreateDirectory(VandaModaIntimaLocalAppDir);
            }

            _statementLogs = new List<StatementLog>();
            TransientWriteStatementLogs = new List<StatementLog>();
            TransientSendStatementLogs = new List<StatementLog>();
            _duplicateStatementLogs = new List<StatementLog>();

            if (File.Exists(LastSyncFile))
            {
                string fromFile = ReadAllText(LastSyncFile);
                LastSync = DateTime.Parse(fromFile);
            }

            if (File.Exists(StatementLogFile))
            {
                string json = ReadAllText(StatementLogFile);
                if (json != string.Empty)
                    _statementLogs = JsonConvert.DeserializeObject<List<StatementLog>>(json);
            }

            _this = this;
            Conectar();
        }

        private void Conectar()
        {
            Timer timerConexaoServidor = null;

            timerConexaoServidor = new Timer((e) =>
            {
                try
                {
                    TextLog += $"{DateTime.Now.ToString(LocalDateFormat)}: Conectando ao Servidor";

                    ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //18.229.130.78
                    ClientSocket.Connect("localhost", 3999);

                    TextLog += $"{DateTime.Now.ToString(LocalDateFormat)}: Conectado ao Servidor Com Sucesso";

                    ApplicationOpening();
                }
                catch (SocketException se)
                {
                    ErroAoConectar(se, "Conectar", false);
                    timerConexaoServidor?.Change(30000, Timeout.Infinite);
                }
            }, null, 0, Timeout.Infinite);
        }

        private void ApplicationOpening()
        {
            try
            {
                TextLog += $"{DateTime.Now.ToString(LocalDateFormat)}: Sincronização Iniciada";
                TextLog += $"{DateTime.Now.ToString(LocalDateFormat)}: Solicitando Logs do Servidor";

                //Guarda logs locais que serão enviados ao servidor
                TransientSendStatementLogs.AddRange(_statementLogs.Where(w => w.WriteTime >= LastSync).ToList());

                string statementRequestJson = "StatementRequest|" + LastSync.ToString("o") + "\n";

                ClientSocket.Send(Encoding.UTF8.GetBytes(statementRequestJson));
                ClientSocket.BeginReceive(_bytesReceivedFromServer, 0, _bytesReceivedFromServer.Length, SocketFlags.None, ReceiveCallback, ClientSocket);
            }
            catch (SocketException se)
            {
                ErroAoConectar(se, "ApplicationOpening");
            }
        }

        [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Comando não precisa adicionar parâmetros")]
        private async void ReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                int messageLength = ClientSocket.EndReceive(asyncResult);
                Socket socket = (Socket)asyncResult.AsyncState;

                if (messageLength > 0)
                {
                    _messageReceived += Encoding.UTF8.GetString(_bytesReceivedFromServer, 0, messageLength);

                    if (_messageReceived.Contains("\n"))
                    {
                        string[] splittedMessage = _messageReceived.Split('\n');
                        string firstLine = splittedMessage[0];

                        string[] firstLineSplitted = firstLine.Split('|');
                        string messageId = firstLineSplitted[0];

                        switch (messageId)
                        {
                            case "StatementLogs":
                                var statementsRecebidos = JsonConvert.DeserializeObject<List<StatementLog>>(firstLineSplitted[1]);
                                var insertsRecebidos = statementsRecebidos.Where(w => w.Statement.StartsWith("INSERT"))
                                    .ToList();

                                //Para cada statement do tipo INSERT
                                foreach (var statementRecebido in insertsRecebidos)
                                {
                                    if (_statementLogs.Contains(statementRecebido))
                                    {
                                        var sameIdStatement = _statementLogs.FirstOrDefault(f =>
                                            f.Statement.Equals(statementRecebido.Statement));

                                        //Se sameIdStatement retorna null significa que em _statementLogs existe um log com a(s)
                                        //mesma(s) id(s) de statementRecebido, mas o Statement em si difere entre os dois, ou seja,
                                        //contêm a mesma id, mas os valores dos outros campos são diferentes.
                                        //Então eu deleto item do Bd local e insiro com uma nova Id
                                        if (sameIdStatement == null)
                                        {
                                            ISession mainSession = SessionProvider.GetMainSession("Duplicate");
                                            ISession secondarySession = SessionProvider.GetSecondarySession("Duplicate");

                                            switch (statementRecebido.Tabela)
                                            {
                                                case "contagem":
                                                    var lojaId = (Model.Loja)await new DAOLoja(secondarySession).ListarPorId(statementRecebido.Identificadores[0]);
                                                    var dataId = DateTime.ParseExact(statementRecebido.Identificadores[1], "yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture);

                                                    var daoContagem = new DAOContagem(secondarySession);

                                                    var contagem = new Model.Contagem() { Loja = lojaId, Data = dataId };
                                                    contagem = (Model.Contagem)await daoContagem.ListarPorId(contagem);

                                                    var contagem1 = (Model.Contagem)contagem.Clone();

                                                    contagem1.Data = contagem.Data.AddMinutes(1);

                                                    await daoContagem.Deletar(contagem);

                                                    await new DAOContagem(mainSession).Inserir(contagem1);
                                                    break;
                                                case "contagemproduto":
                                                    var daoContagemProduto = new DAOContagemProduto(secondarySession);
                                                    string idContagemProduto = statementRecebido.Identificadores[0];
                                                    long newIdContagemProduto = 0;

                                                    //Recuperando local
                                                    Model.ContagemProduto contagemProduto = await daoContagemProduto.ListarPorId(idContagemProduto) as Model.ContagemProduto;
                                                    Model.ContagemProduto copia = contagemProduto.Clone() as Model.ContagemProduto;

                                                    while (contagemProduto != null)
                                                    {
                                                        contagemProduto = await daoContagemProduto.ListarPorId(newIdContagemProduto) as Model.ContagemProduto;
                                                        newIdContagemProduto = DateTime.Now.Ticks;
                                                    }

                                                    copia.Id = newIdContagemProduto;

                                                    await daoContagemProduto.Deletar(contagemProduto);
                                                    await new DAOContagemProduto(mainSession).Inserir(copia);
                                                    break;
                                                case "fornecedor":
                                                    var daoFornecedor = new DAOFornecedor(secondarySession);
                                                    var idFornecedor = statementRecebido.Identificadores[0];
                                                    var fornecedor = await daoFornecedor.ListarPorId(idFornecedor);
                                                    await daoFornecedor.Deletar(fornecedor);
                                                    break;
                                                case "loja":
                                                    var daoLoja = new DAOLoja(secondarySession);
                                                    var idLoja = statementRecebido.Identificadores[0];
                                                    var loja = await daoLoja.ListarPorId(idLoja);
                                                    await daoLoja.Deletar(loja);
                                                    break;
                                                case "marca":
                                                    statementsRecebidos.Remove(statementRecebido);
                                                    break;
                                                case "operadoracartao":
                                                    break;
                                                case "produto":
                                                    var daoProduto = new DAOProduto(secondarySession);
                                                    string idProduto = statementRecebido.Identificadores[0];
                                                    var newIdProduto = daoProduto.GetMaxId() + 1;

                                                    //Recuperando produto no BD local com a mesma id do produto recebido do servidor
                                                    Model.Produto produto = await daoProduto.ListarPorId(idProduto) as Model.Produto;
                                                    //Copio o produto local para um novo objeto
                                                    Model.Produto produtoCopia = produto.Clone() as Model.Produto;

                                                    //Checando qual Id está livre para usar para inserir a cópia do produto
                                                    while (produto != null)
                                                    {
                                                        produto = await daoProduto.ListarPorId((newIdProduto).ToString()) as Model.Produto;
                                                        newIdProduto++;
                                                    }

                                                    //Mudando Id da cópia
                                                    produtoCopia.CodBarra = newIdProduto.ToString();

                                                    //Deleto produto com id repetida do BD local
                                                    await daoProduto.Deletar(produto);

                                                    //Insiro a cópia com nova id
                                                    await new DAOProduto(mainSession).Inserir(produtoCopia);
                                                    break;
                                                case "recebimentocartao":
                                                    break;
                                                case "tipocontagem":
                                                    break;
                                            }

                                            SessionProvider.FechaMainSession("Duplicate");
                                            SessionProvider.FechaSecondarySession("Duplicate");
                                        }

                                        _statementLogs.Remove(statementRecebido);
                                        TransientSendStatementLogs.Remove(statementRecebido);
                                    }
                                }

                                using (var conexao = new MySqlConnection(_connectionString))
                                {
                                    conexao.Open();

                                    using (var transacao = conexao.BeginTransaction())
                                    {
                                        try
                                        {
                                            using (var comando = new MySqlCommand())
                                            {
                                                comando.Connection = conexao;
                                                comando.Transaction = transacao;

                                                foreach (var statement in statementsRecebidos)
                                                {
                                                    comando.Parameters.Clear();
                                                    comando.CommandText = statement.Statement;
                                                    comando.ExecuteNonQuery();
                                                    TextLog += $"{DateTime.Now.ToString(LocalDateFormat)}: Statement Recebido: {statement.Statement}";
                                                }

                                                transacao.Commit();

                                                LastSync = DateTime.Now;
                                                File.WriteAllText(LastSyncFile, LastSync.ToString("o"));
                                            }
                                        }
                                        catch (MySqlException ex)
                                        {
                                            transacao.Rollback();
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }

                                break;
                        }

                        _messageReceived = _messageReceived.Replace(firstLine + "\n", string.Empty);
                    }

                    socket.BeginReceive(_bytesReceivedFromServer, 0, _bytesReceivedFromServer.Length, SocketFlags.None, ReceiveCallback, socket);
                }
            }
            catch (SocketException se)
            {
                ErroAoConectar(se, "ReceiveCallback");
            }
            catch (ObjectDisposedException ode)
            {
                Console.WriteLine("Socket foi Disposed. " + ode.Message);
            }
        }

        private void SendCallback(IAsyncResult asyncResult)
        {
            try
            {
                Socket socket = (Socket)asyncResult.AsyncState;
                socket.EndSend(asyncResult);
            }
            catch (SocketException se)
            {
                ErroAoConectar(se, "SendCallback");
            }
            catch (ObjectDisposedException ode)
            {
                Console.WriteLine("Socket foi Disposed. " + ode.Message);
            }
        }

        public static void SendStatementLog()
        {
            try
            {
                if (TransientSendStatementLogs.Count > 0)
                {
                    string transientStatementLogsJson = JsonConvert.SerializeObject(TransientSendStatementLogs);

                    string messageToServer = "StatementLogs";
                    messageToServer += "|" + transientStatementLogsJson;
                    messageToServer += "\n"; // Para que o servidor encontre o fim da mensagem

                    ClientSocket.Send(Encoding.UTF8.GetBytes(messageToServer));

                    WriteAllText(LastSyncFile, DateTime.Now.ToString("o"));

                    // Usando Reflection Para Setar Valor de TextLog Porque Este Método é Estático, Mas a Propriedade Não É
                    PropertyInfo propertyInfo = typeof(SincronizacaoViewModel).GetProperty("TextLog");
                    string textLogAtual = (string)propertyInfo.GetValue(_this, null);
                    propertyInfo.SetValue(_this, textLogAtual + $"{DateTime.Now.ToString(LocalDateFormat)}: Statements Enviados ao Servidor: {TransientSendStatementLogs.Count}");

                    TransientSendStatementLogs.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static void SendStatementLog(List<StatementLog> statements)
        {
            try
            {
                if (statements.Count > 0)
                {
                    string transientSendStatementLogsJson = JsonConvert.SerializeObject(statements);

                    string messageToServer = "StatementLogs";
                    messageToServer += "|" + transientSendStatementLogsJson;
                    messageToServer += "\n"; // Para que o servidor encontre o fim da mensagem

                    ClientSocket.Send(Encoding.UTF8.GetBytes(messageToServer));

                    WriteAllText(LastSyncFile, DateTime.Now.ToString("o"));

                    // Usando Reflection Para Setar Valor de TextLog Porque Este Método é Estático, Mas a Propriedade Não É
                    PropertyInfo propertyInfo = typeof(SincronizacaoViewModel).GetProperty("TextLog");
                    string textLogAtual = (string)propertyInfo.GetValue(_this, null);
                    propertyInfo.SetValue(_this, textLogAtual + $"{DateTime.Now.ToString(LocalDateFormat)}: Statements Enviados ao Servidor: {statements.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static void WriteStatementLog()
        {
            _statementLogs.AddRange(TransientWriteStatementLogs);
            _statementLogs = _statementLogs.OrderBy(o => o.WriteTime).ToList();
            string json = JsonConvert.SerializeObject(_statementLogs, Formatting.Indented);
            WriteAllText(StatementLogFile, json);

            TransientSendStatementLogs.AddRange(TransientWriteStatementLogs);

            try
            {
                //Usando Reflection Para Setar Valor de TextLog Porque Este Método é Estático, Mas a Propriedade Não É
                PropertyInfo propertyInfo = typeof(SincronizacaoViewModel).GetProperty("TextLog");

                foreach (StatementLog statement in TransientWriteStatementLogs)
                {
                    string textLogAtual = (string)propertyInfo.GetValue(_this, null);
                    propertyInfo.SetValue(_this, textLogAtual + $"{statement.WriteTime.ToString(LocalDateFormat)}: Statement Escrito - {statement.Statement}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void ErroAoConectar(SocketException se, string metodo, bool callConectar = true)
        {
            TextLog += $"{metodo} | Não É Possível Conectar Ao Servidor.\nTentando Novamente Em 30 Segundos.\nMensagem de Erro: {se.Message}\n";
            Console.WriteLine(se.Message);
            Console.WriteLine("Socket Error Code: " + se.SocketErrorCode);
            Console.WriteLine(se.StackTrace);

            if (callConectar)
                Conectar();
        }

        private string ReadAllText(string path)
        {
            string text = null;

            if (!File.Exists(path))
                File.Create(path);

            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var textReader = new StreamReader(fileStream))
                {
                    text = textReader.ReadToEnd();
                }
            }

            return text;
        }

        private static void WriteAllText(string path, string text)
        {
            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var textReader = new StreamWriter(fileStream))
                {
                    textReader.Write(text);
                }
            }
        }

        /// <summary>
        /// Apaga todos os LOGS e recria usando os dados presentes atualmente no banco de dados
        /// </summary>
        private async static void ResetaLogs()
        {
            if (File.Exists(StatementLogFile))
                File.Delete(StatementLogFile);

            ISession session = SessionProvider.GetMainSession("SincronizacaoReset");

            DAOContagem dAOContagem = new DAOContagem(session);
            DAOContagemProduto dAOContagemProduto = new DAOContagemProduto(session);
            DAOFornecedor dAOFornecedor = new DAOFornecedor(session);
            DAOLoja dAOLoja = new DAOLoja(session);
            DAOMarca dAOMarca = new DAOMarca(session);
            DAOOperadoraCartao dAOOperadoraCartao = new DAOOperadoraCartao(session);
            DAOProduto dAOProduto = new DAOProduto(session);
            DAORecebimentoCartao dAORecebimentoCartao = new DAORecebimentoCartao(session);
            DAOTipoContagem dAOTipoContagem = new DAOTipoContagem(session);

            ////var lojas = await dAOLoja.Listar<Model.Loja>();
            ////var fornecedores = await dAOFornecedor.Listar<Model.Fornecedor>();
            ////var operadoras = await dAOOperadoraCartao.Listar<Model.OperadoraCartao>();
            ////var tipocontagens = await dAOTipoContagem.Listar<Model.TipoContagem>();
            ////var marcas = await dAOMarca.Listar<Model.Marca>();
            ////var produtos = await dAOProduto.Listar<Model.Produto>();
            ////var contagens = await dAOContagem.Listar<Model.Contagem>();
            ////var contagemprodutos = await dAOContagemProduto.Listar<ContagemProduto>();
            ////var recebimentos = await dAORecebimentoCartao.Listar<Model.RecebimentoCartao>();

            //string l = JsonConvert.SerializeObject(lojas);
            //string f = JsonConvert.SerializeObject(fornecedores);
            //string o = JsonConvert.SerializeObject(operadoras);
            //string tc = JsonConvert.SerializeObject(tipocontagens);
            //string m = JsonConvert.SerializeObject(marcas);
            //string p = JsonConvert.SerializeObject(produtos);
            //string c = JsonConvert.SerializeObject(contagens);
            //string cp = JsonConvert.SerializeObject(contagemprodutos);
            //string r = JsonConvert.SerializeObject(recebimentos);

            //File.WriteAllText("Loja.txt", l);
            //File.WriteAllText("Fornecedor.txt", f);
            //File.WriteAllText("OperadoraCartao.txt", o);
            //File.WriteAllText("TipoContagem.txt", tc);
            //File.WriteAllText("Marca.txt", m);
            //File.WriteAllText("Produto.txt", p);
            //File.WriteAllText("Contagem.txt", c);
            //File.WriteAllText("ContagemProduto.txt", cp);
            //File.WriteAllText("RecebimentoCartao.txt", r);

            IList<Model.Loja> Loja = JsonConvert.DeserializeObject<List<Model.Loja>>(File.ReadAllText("Loja.txt"));
            IList<Model.Fornecedor> Fornecedor = JsonConvert.DeserializeObject<List<Model.Fornecedor>>(File.ReadAllText("Fornecedor.txt"));
            IList<Model.OperadoraCartao> OperadoraCartao = JsonConvert.DeserializeObject<List<Model.OperadoraCartao>>(File.ReadAllText("OperadoraCartao.txt"));
            IList<Model.TipoContagem> TipoContagem = JsonConvert.DeserializeObject<List<Model.TipoContagem>>(File.ReadAllText("TipoContagem.txt"));
            IList<Model.Marca> Marca = JsonConvert.DeserializeObject<List<Model.Marca>>(File.ReadAllText("Marca.txt"));
            IList<Model.Produto> Produto = JsonConvert.DeserializeObject<List<Model.Produto>>(File.ReadAllText("Produto.txt"));
            IList<Model.Contagem> Contagem = JsonConvert.DeserializeObject<List<Model.Contagem>>(File.ReadAllText("Contagem.txt"));
            IList<Model.ContagemProduto> ContagemProduto = JsonConvert.DeserializeObject<List<Model.ContagemProduto>>(File.ReadAllText("ContagemProduto.txt"));
            IList<Model.RecebimentoCartao> RecebimentoCartao = JsonConvert.DeserializeObject<List<Model.RecebimentoCartao>>(File.ReadAllText("RecebimentoCartao.txt"));

            await dAOLoja.Inserir(Loja);
            await dAOFornecedor.Inserir(Fornecedor);
            await dAOOperadoraCartao.Inserir(OperadoraCartao);
            await dAOMarca.Inserir(Marca);
            await dAOProduto.Inserir(Produto);
            await dAOTipoContagem.Inserir(TipoContagem);
            await dAOContagem.Inserir(Contagem);
            await dAOContagemProduto.Inserir(ContagemProduto);
            await dAORecebimentoCartao.Inserir(RecebimentoCartao);

            WriteStatementLog();

            SessionProvider.FechaMainSession("SincronizacaoReset");
        }

        public string TextLog
        {
            get
            {
                return _textLog;
            }

            set
            {
                _textLog = value + "\n";
                OnPropertyChanged("TextLog");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing && ClientSocket.Connected)
            {
                ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.Close();
            }

            _disposed = true;
        }
    }
}
