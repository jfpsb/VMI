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

        private static readonly string StatementLogFile = "StatementLog.txt";
        private static readonly string LastSyncFile = "LastSync.txt";
        private string MessageReceived = "";
        private byte[] BytesReceivedFromServer = new byte[1024];
        private string _textLog;
        private bool Disposed;
        private static readonly string _localDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
        private static object This; // Guarda referência à própria classe para ser usado em Reflection dentro de método estático
        private static List<StatementLog> StatementLogs;
        public static List<StatementLog> TransientWriteStatementLogs;
        public static List<StatementLog> TransientSendStatementLogs;
        private static DateTime LastSync = new DateTime(2018, 1, 1, 0, 0, 0);
        private string connectionString = "SERVER=localhost;DATABASE=vandamodaintima;UID=root;PASSWORD=1124";

        public SincronizacaoViewModel()
        {


            StatementLogs = new List<StatementLog>();
            TransientWriteStatementLogs = new List<StatementLog>();
            TransientSendStatementLogs = new List<StatementLog>();

            ResetaLogs();

            if (File.Exists(LastSyncFile))
            {
                string fromFile = File.ReadAllText(LastSyncFile);
                LastSync = DateTime.Parse(fromFile);
            }

            if (File.Exists(StatementLogFile))
            {
                string json = File.ReadAllText(StatementLogFile);
                if (json != string.Empty)
                    StatementLogs = JsonConvert.DeserializeObject<List<StatementLog>>(json);
            }

            This = this;
            Conectar();
        }

        private void Conectar()
        {
            Timer timerConexaoServidor = null;

            timerConexaoServidor = new Timer((e) =>
            {
                try
                {
                    TextLog += $"{DateTime.Now.ToString(_localDateFormat)}: Conectando ao Servidor";

                    ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //18.229.130.78
                    ClientSocket.Connect("18.229.130.78", 3999);

                    TextLog += $"{DateTime.Now.ToString(_localDateFormat)}: Conectado ao Servidor Com Sucesso";

                    ApplicationOpening();
                }
                catch (SocketException se)
                {
                    ErroAoConectar(se, "Conectar", false);
                    timerConexaoServidor.Change(30000, Timeout.Infinite);
                }
            }, null, 0, Timeout.Infinite);
        }

        private void ApplicationOpening()
        {
            try
            {
                TextLog += $"{DateTime.Now.ToString(_localDateFormat)}: Sincronização Iniciada";

                if (StatementLogs.Count == 0)
                {
                    TextLog += $"{DateTime.Now.ToString(_localDateFormat)}: Não Há Arquivo de Logs Local. Solicitando Ao Servidor";
                    DateTime dateTime = new DateTime(2018, 1, 1, 0, 0, 0);
                    string requestJson = "StatementRequest|" + dateTime.ToString("o") + "\n";
                    ClientSocket.Send(Encoding.UTF8.GetBytes(requestJson));
                    return;
                }

                List<StatementLog> statementsAEnviar = StatementLogs.Where(w => w.WriteTime >= LastSync).ToList();

                if (statementsAEnviar.Count == 0)
                {
                    TextLog += $"{DateTime.Now.ToString(_localDateFormat)}: Não Há Statements Para Mandar Para O Servidor";
                }
                else
                {
                    SendStatementLog(statementsAEnviar);
                    TextLog += $"{DateTime.Now.ToString(_localDateFormat)}: Enviando Statements Locais Para Servidor";
                }

                string statementRequestJson = "StatementRequest|" + LastSync.ToString("o") + "\n";

                ClientSocket.Send(Encoding.UTF8.GetBytes(statementRequestJson));
                ClientSocket.BeginReceive(BytesReceivedFromServer, 0, BytesReceivedFromServer.Length, SocketFlags.None, ReceiveCallback, ClientSocket);
            }
            catch (SocketException se)
            {
                ErroAoConectar(se, "ApplicationOpening");
            }
        }

        [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Comando não precisa adicionar parâmetros")]
        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                int messageLength = ClientSocket.EndReceive(asyncResult);
                Socket socket = (Socket)asyncResult.AsyncState;

                if (messageLength > 0)
                {
                    MessageReceived += Encoding.UTF8.GetString(BytesReceivedFromServer, 0, messageLength);

                    if (MessageReceived.Contains("\n"))
                    {
                        string[] SplittedMessage = MessageReceived.Split('\n');
                        string FirstLine = SplittedMessage[0];

                        string[] FirstLineSplitted = FirstLine.Split('|');
                        string MessageId = FirstLineSplitted[0];

                        switch (MessageId)
                        {
                            case "StatementLogs":
                                List<StatementLog> statementsRecebidos = JsonConvert.DeserializeObject<List<StatementLog>>(FirstLineSplitted[1]);

                                using (MySqlConnection conexao = new MySqlConnection(connectionString))
                                {
                                    using (MySqlTransaction transacao = conexao.BeginTransaction())
                                    {
                                        try
                                        {
                                            using (MySqlCommand comando = new MySqlCommand())
                                            {
                                                comando.Connection = conexao;
                                                comando.Transaction = transacao;

                                                foreach (StatementLog statement in statementsRecebidos)
                                                {
                                                    comando.Parameters.Clear();
                                                    comando.CommandText = statement.Statement;
                                                    comando.ExecuteNonQuery();
                                                    TextLog += $"{DateTime.Now.ToString(_localDateFormat)}: Statement Recebido: {statement.Statement}";
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

                        MessageReceived = MessageReceived.Replace(FirstLine + "\n", string.Empty);
                    }

                    socket.BeginReceive(BytesReceivedFromServer, 0, BytesReceivedFromServer.Length, SocketFlags.None, ReceiveCallback, socket);
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
                if (TransientWriteStatementLogs.Count > 0)
                {
                    string transientStatementLogsJson = JsonConvert.SerializeObject(TransientWriteStatementLogs);

                    string messageToServer = "StatementLogs";
                    messageToServer += "|" + transientStatementLogsJson;
                    messageToServer += "\n"; // Para que o servidor encontre o fim da mensagem

                    ClientSocket.Send(Encoding.UTF8.GetBytes(messageToServer));

                    TransientWriteStatementLogs.Clear();

                    // Usando Reflection Para Setar Valor de TextLog Porque Este Método é Estático, Mas a Propriedade Não É
                    PropertyInfo propertyInfo = typeof(SincronizacaoViewModel).GetProperty("TextLog");
                    string textLogAtual = (string)propertyInfo.GetValue(This, null);
                    propertyInfo.SetValue(This, textLogAtual + $"{DateTime.Now.ToString(_localDateFormat)}: Statements Enviados ao Servidor: {TransientWriteStatementLogs.Count}");
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
                if (TransientSendStatementLogs.Count > 0)
                {
                    string transientSendStatementLogsJson = JsonConvert.SerializeObject(statements);

                    string messageToServer = "StatementLogs";
                    messageToServer += "|" + transientSendStatementLogsJson;
                    messageToServer += "\n"; // Para que o servidor encontre o fim da mensagem

                    ClientSocket.Send(Encoding.UTF8.GetBytes(messageToServer));

                    TransientSendStatementLogs.Clear();

                    // Usando Reflection Para Setar Valor de TextLog Porque Este Método é Estático, Mas a Propriedade Não É
                    PropertyInfo propertyInfo = typeof(SincronizacaoViewModel).GetProperty("TextLog");
                    string textLogAtual = (string)propertyInfo.GetValue(This, null);
                    propertyInfo.SetValue(This, textLogAtual + $"{DateTime.Now.ToString(_localDateFormat)}: Statements Enviados ao Servidor: {TransientSendStatementLogs.Count}");
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
            StatementLogs.AddRange(TransientWriteStatementLogs);
            string json = JsonConvert.SerializeObject(StatementLogs, Formatting.Indented);
            File.WriteAllText(StatementLogFile, json);
            File.SetAttributes(StatementLogFile, File.GetAttributes(StatementLogFile) | FileAttributes.Hidden);

            try
            {
                //Usando Reflection Para Setar Valor de TextLog Porque Este Método é Estático, Mas a Propriedade Não É
                PropertyInfo propertyInfo = typeof(SincronizacaoViewModel).GetProperty("TextLog");

                foreach (StatementLog statement in TransientWriteStatementLogs)
                {
                    string textLogAtual = (string)propertyInfo.GetValue(This, null);
                    propertyInfo.SetValue(This, textLogAtual + $"{statement.WriteTime.ToString(_localDateFormat)}: Statement Escrito - {statement.Statement}");
                }

                TransientWriteStatementLogs.Clear();
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

        /// <summary>
        /// Apaga todos os LOGS e recria usando os dados presentes atualmente no banco de dados
        /// </summary>
        private async static void ResetaLogs()
        {
            if (File.Exists(StatementLogFile))
                File.Delete(StatementLogFile);

            List<StatementLog> statements = new List<StatementLog>();

            ISession session = SessionProvider.GetSession("SincronizacaoReset");

            DAOContagem dAOContagem = new DAOContagem(session);
            DAOContagemProduto dAOContagemProduto = new DAOContagemProduto(session);
            DAOFornecedor dAOFornecedor = new DAOFornecedor(session);
            DAOLoja dAOLoja = new DAOLoja(session);
            DAOMarca dAOMarca = new DAOMarca(session);
            DAOOperadoraCartao dAOOperadoraCartao = new DAOOperadoraCartao(session);
            DAOProduto dAOProduto = new DAOProduto(session);
            DAORecebimentoCartao dAORecebimentoCartao = new DAORecebimentoCartao(session);
            DAOTipoContagem dAOTipoContagem = new DAOTipoContagem(session);

            //var lojas = await dAOLoja.Listar<Model.Loja>();
            //var fornecedores = await dAOFornecedor.Listar<Model.Fornecedor>();
            //var operadoras = await dAOOperadoraCartao.Listar<Model.OperadoraCartao>();
            //var tipocontagens = await dAOTipoContagem.Listar<Model.TipoContagem>();
            //var marcas = await dAOMarca.Listar<Model.Marca>();
            //var produtos = await dAOProduto.Listar<Model.Produto>();
            //var contagens = await dAOContagem.Listar<Model.Contagem>();
            //var contagemprodutos = await dAOContagemProduto.Listar<ContagemProduto>();
            //var recebimentos = await dAORecebimentoCartao.Listar<Model.RecebimentoCartao>();

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

            SessionProvider.FechaSession("SincronizacaoReset");

            WriteStatementLog();
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
            if (Disposed)
                return;

            if (disposing && ClientSocket.Connected)
            {
                ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.Close();
            }

            Disposed = true;
        }
    }
}
