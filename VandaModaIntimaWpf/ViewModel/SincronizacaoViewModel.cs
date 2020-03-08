using Newtonsoft.Json;
using NHibernate;
using SincronizacaoBD.Util.Sincronizacao;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Util.Sincronizacao;

namespace VandaModaIntimaWpf.ViewModel
{
    public class SincronizacaoViewModel : ObservableObject, IDisposable
    {
        public static Socket ClientSocket;

        private static readonly string DatabaseLogDir = "DatabaseLog";
        private string MessageReceived = "";
        private byte[] BytesReceivedFromServer = new byte[1024];
        private string _textLog;
        private bool Disposed;
        private static readonly string _localDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;

        public SincronizacaoViewModel()
        {
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
                    ClientSocket.Connect(IPAddress.Loopback, 3999);

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
                if (!Directory.Exists(DatabaseLogDir))
                    Directory.CreateDirectory(DatabaseLogDir);

                string[] filePaths = Directory.GetFiles(DatabaseLogDir);
                List<DatabaseLogFileInfo> databaseLogFileInfos = new List<DatabaseLogFileInfo>();

                string[] contagemFilePaths = filePaths.Where(w => Path.GetFileName(w).Split(' ')[0].Equals("Contagem")).ToArray();
                string[] contagemProdutoFilePaths = filePaths.Where(w => Path.GetFileName(w).Split(' ')[0].Equals("ContagemProduto")).ToArray();
                string[] fornecedorFilePaths = filePaths.Where(w => Path.GetFileName(w).Split(' ')[0].Equals("Fornecedor")).ToArray();
                string[] lojaFilePaths = filePaths.Where(w => Path.GetFileName(w).Split(' ')[0].Equals("Loja")).ToArray();
                string[] marcaFilePaths = filePaths.Where(w => Path.GetFileName(w).Split(' ')[0].Equals("Marca")).ToArray();
                string[] operadoraCartaoFilePaths = filePaths.Where(w => Path.GetFileName(w).Split(' ')[0].Equals("OperadoraCartao")).ToArray();
                string[] produtoFilePaths = filePaths.Where(w => Path.GetFileName(w).Split(' ')[0].Equals("Produto")).ToArray();
                string[] recebimentoCartaoFilePaths = filePaths.Where(w => Path.GetFileName(w).Split(' ')[0].Equals("RecebimentoCartao")).ToArray();
                string[] tipoContagemFilePaths = filePaths.Where(w => Path.GetFileName(w).Split(' ')[0].Equals("TipoContagem")).ToArray();

                foreach (string fileName in contagemFilePaths)
                {
                    AddToDatabaseLogFileInfoList<Model.Contagem>(fileName, databaseLogFileInfos);
                }

                foreach (string fileName in contagemProdutoFilePaths)
                {
                    AddToDatabaseLogFileInfoList<Model.ContagemProduto>(fileName, databaseLogFileInfos);
                }

                foreach (string fileName in fornecedorFilePaths)
                {
                    AddToDatabaseLogFileInfoList<Model.Fornecedor>(fileName, databaseLogFileInfos);
                }

                foreach (string fileName in lojaFilePaths)
                {
                    AddToDatabaseLogFileInfoList<Model.Loja>(fileName, databaseLogFileInfos);
                }

                foreach (string fileName in marcaFilePaths)
                {
                    AddToDatabaseLogFileInfoList<Model.Marca>(fileName, databaseLogFileInfos);
                }

                foreach (string fileName in operadoraCartaoFilePaths)
                {
                    AddToDatabaseLogFileInfoList<Model.OperadoraCartao>(fileName, databaseLogFileInfos);
                }

                foreach (string fileName in produtoFilePaths)
                {
                    AddToDatabaseLogFileInfoList<Model.Produto>(fileName, databaseLogFileInfos);
                }

                foreach (string fileName in recebimentoCartaoFilePaths)
                {
                    AddToDatabaseLogFileInfoList<Model.RecebimentoCartao>(fileName, databaseLogFileInfos);
                }

                foreach (string fileName in tipoContagemFilePaths)
                {
                    AddToDatabaseLogFileInfoList<Model.TipoContagem>(fileName, databaseLogFileInfos);
                }

                string databaseLogFileInfosJson = "DatabaseLogFileInfo|" + JsonConvert.SerializeObject(databaseLogFileInfos) + "\n";

                TextLog += $"{DateTime.Now.ToString(_localDateFormat)}: Sincronização Executando\n";
                ClientSocket.Send(Encoding.UTF8.GetBytes(databaseLogFileInfosJson));
                ClientSocket.BeginReceive(BytesReceivedFromServer, 0, BytesReceivedFromServer.Length, SocketFlags.None, ReceiveCallback, ClientSocket);
            }
            catch (SocketException se)
            {
                ErroAoConectar(se, "ApplicationOpening");
            }
        }

        private void AddToDatabaseLogFileInfoList<E>(string fileName, List<DatabaseLogFileInfo> databaseLogFileInfos) where E : class, IModel
        {
            string JsonText = File.ReadAllText(fileName).Replace("\r", string.Empty).Replace("\n", string.Empty);
            DatabaseLogFile<E> DatabaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<E>>(JsonText);

            if ((DateTime.Now - DatabaseLogFile.LastWriteTime).TotalDays >= 180 && DatabaseLogFile.OperacaoMySQL.Equals("DELETE"))
            {
                File.Delete(fileName);
                return;
            }

            DatabaseLogFileInfo databaseLogFileInfo = new DatabaseLogFileInfo() { LastModified = DatabaseLogFile.LastWriteTime, FileName = Path.GetFileName(fileName) };
            databaseLogFileInfos.Add(databaseLogFileInfo);
        }

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
                            case "DatabaseLogFileRequest":
                                // Recebendo requests de LOGS que devem ser enviados ao servidor
                                TextLog += $"{DateTime.Now.ToString(_localDateFormat)}: Recebido Pedido Do Servidor Para Envio De Logs";
                                string dataLogFileRequest = FirstLineSplitted[1];
                                SendDatabaseLogFileToServer(dataLogFileRequest);
                                TextLog += $"{DateTime.Now.ToString(_localDateFormat)}: Logs Enviados Ao Servidor";
                                break;
                            case "DatabaseLogFile":
                                // Recebendo LOG do servidor
                                string fileNamesJson = FirstLineSplitted[1];
                                string databaseLogFilesJson = FirstLineSplitted[2];

                                TextLog += $"{DateTime.Now.ToString(_localDateFormat)}: Recebido Log Do Servidor. Inserindo No Banco de Dados";

                                List<string> fileNames = JsonConvert.DeserializeObject<List<string>>(fileNamesJson);
                                List<string> databaseLogFiles = JsonConvert.DeserializeObject<List<string>>(databaseLogFilesJson);

                                List<DatabaseLogFile<Model.Contagem>> logsContagem = new List<DatabaseLogFile<Model.Contagem>>();
                                List<DatabaseLogFile<Model.ContagemProduto>> logsContagemProduto = new List<DatabaseLogFile<Model.ContagemProduto>>();
                                List<DatabaseLogFile<Model.Fornecedor>> logsFornecedor = new List<DatabaseLogFile<Model.Fornecedor>>();
                                List<DatabaseLogFile<Model.Loja>> logsLoja = new List<DatabaseLogFile<Model.Loja>>();
                                List<DatabaseLogFile<Model.Marca>> logsMarca = new List<DatabaseLogFile<Model.Marca>>();
                                List<DatabaseLogFile<Model.OperadoraCartao>> logsOperadoraCartao = new List<DatabaseLogFile<Model.OperadoraCartao>>();
                                List<DatabaseLogFile<Model.Produto>> logsProduto = new List<DatabaseLogFile<Model.Produto>>();
                                List<DatabaseLogFile<Model.RecebimentoCartao>> logsRecebimentoCartao = new List<DatabaseLogFile<Model.RecebimentoCartao>>();
                                List<DatabaseLogFile<Model.TipoContagem>> logsTipoContagem = new List<DatabaseLogFile<Model.TipoContagem>>();

                                int[] contagemIndexes = fileNames.Select((str, index) => new { Value = str, Index = index }).Where(w => w.Value.Split(' ')[0].Equals("Contagem")).Select(s => s.Index).ToArray();
                                int[] contagemProdutoIndexes = fileNames.Select((str, index) => new { Value = str, Index = index }).Where(w => w.Value.Split(' ')[0].Equals("ContagemProduto")).Select(s => s.Index).ToArray();
                                int[] fornecedorIndexes = fileNames.Select((str, index) => new { Value = str, Index = index }).Where(w => w.Value.Split(' ')[0].Equals("Fornecedor")).Select(s => s.Index).ToArray();
                                int[] lojaIndexes = fileNames.Select((str, index) => new { Value = str, Index = index }).Where(w => w.Value.Split(' ')[0].Equals("Loja")).Select(s => s.Index).ToArray();
                                int[] marcaIndexes = fileNames.Select((str, index) => new { Value = str, Index = index }).Where(w => w.Value.Split(' ')[0].Equals("Marca")).Select(s => s.Index).ToArray();
                                int[] operadoraCartaoIndexes = fileNames.Select((str, index) => new { Value = str, Index = index }).Where(w => w.Value.Split(' ')[0].Equals("OperadoraCartao")).Select(s => s.Index).ToArray();
                                int[] produtoIndexes = fileNames.Select((str, index) => new { Value = str, Index = index }).Where(w => w.Value.Split(' ')[0].Equals("Produto")).Select(s => s.Index).ToArray();
                                int[] recebimentoCartaoIndexes = fileNames.Select((str, index) => new { Value = str, Index = index }).Where(w => w.Value.Split(' ')[0].Equals("RecebimentoCartao")).Select(s => s.Index).ToArray();
                                int[] tipoContagemIndexes = fileNames.Select((str, index) => new { Value = str, Index = index }).Where(w => w.Value.Split(' ')[0].Equals("TipoContagem")).Select(s => s.Index).ToArray();

                                foreach (int contagemIndex in contagemIndexes)
                                {
                                    DeserializeLogAndAddToList<Contagem>(databaseLogFiles[contagemIndex], logsContagem);
                                }

                                foreach (int contagemProdutoIndex in contagemProdutoIndexes)
                                {
                                    DeserializeLogAndAddToList(databaseLogFiles[contagemProdutoIndex], logsContagemProduto);
                                }

                                foreach (int fornecedorIndex in fornecedorIndexes)
                                {
                                    DeserializeLogAndAddToList(databaseLogFiles[fornecedorIndex], logsFornecedor);
                                }

                                foreach (int lojaIndex in lojaIndexes)
                                {
                                    DeserializeLogAndAddToList(databaseLogFiles[lojaIndex], logsLoja);
                                }

                                foreach (int marcaIndex in marcaIndexes)
                                {
                                    DeserializeLogAndAddToList(databaseLogFiles[marcaIndex], logsMarca);
                                }

                                foreach (int operadoraIndex in operadoraCartaoIndexes)
                                {
                                    DeserializeLogAndAddToList(databaseLogFiles[operadoraIndex], logsOperadoraCartao);
                                }

                                foreach (int produtoIndex in produtoIndexes)
                                {
                                    DeserializeLogAndAddToList(databaseLogFiles[produtoIndex], logsProduto);
                                }

                                foreach (int recebimentoIndex in recebimentoCartaoIndexes)
                                {
                                    DeserializeLogAndAddToList(databaseLogFiles[recebimentoIndex], logsRecebimentoCartao);
                                }

                                foreach (int tipoContagemIndex in tipoContagemIndexes)
                                {
                                    DeserializeLogAndAddToList(databaseLogFiles[tipoContagemIndex], logsTipoContagem);
                                }

                                ISession session = SessionProvider.GetSession("Sincronizacao");

                                // Persiste os que não tem chaves estrangeiras antes
                                PersistLogs(new DAOLoja(session), logsLoja);
                                PersistLogs(new DAOFornecedor(session), logsFornecedor);
                                PersistLogs(new DAOOperadoraCartao(session), logsOperadoraCartao);
                                PersistLogs(new DAOTipoContagem(session), logsTipoContagem);

                                // The rest is free real state
                                PersistLogs(new DAOContagem(session), logsContagem);
                                PersistLogs(new DAOContagemProduto(session), logsContagemProduto);
                                PersistLogs(new DAOMarca(session), logsMarca);
                                PersistLogs(new DAOProduto(session), logsProduto);
                                PersistLogs(new DAORecebimentoCartao(session), logsRecebimentoCartao);

                                SessionProvider.FechaSession("Sincronizacao");

                                TextLog += $"{DateTime.Now.ToString(_localDateFormat)}: Logs Atualizados Com Sucesso";

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

        /// <summary>
        /// Envia os Logs Ao Servidor
        /// </summary>
        /// <param name="databaseLogFileInfoRequestJson">Lista de DatabaseLogFileInfo em Json</param>
        private void SendDatabaseLogFileToServer(string databaseLogFileInfoRequestJson)
        {
            List<DatabaseLogFileInfo> databaseLogFileInfos = JsonConvert.DeserializeObject<List<DatabaseLogFileInfo>>(databaseLogFileInfoRequestJson);

            List<string> fileNamesRequested = databaseLogFileInfos.Select(s => Path.GetFileName(s.FileName)).ToList();
            List<string> databaseLogFilesRequested = new List<string>();

            Directory.CreateDirectory(DatabaseLogDir);

            foreach (string fileNameRequested in fileNamesRequested)
            {
                databaseLogFilesRequested.Add(File.ReadAllText(Path.Combine(DatabaseLogDir, fileNameRequested)).Replace("\r", string.Empty).Replace("\n", string.Empty));
            }

            string fileNamesRequestedJson = JsonConvert.SerializeObject(fileNamesRequested);
            string databaseLogFilesRequestedJson = JsonConvert.SerializeObject(databaseLogFilesRequested);

            string messageToServer = "DatabaseLogFile";
            messageToServer += "|" + fileNamesRequestedJson;
            messageToServer += "|" + databaseLogFilesRequestedJson;
            messageToServer += "\n"; // Para que o servidor encontre o fim do texto

            ClientSocket.BeginSend(Encoding.UTF8.GetBytes(messageToServer), 0, Encoding.UTF8.GetBytes(messageToServer).Length, SocketFlags.None, SendCallback, ClientSocket);
        }

        public static void SendDatabaseLogFileToServer<E>(DatabaseLogFile<E> databaseLogFile) where E : class, IModel
        {
            try
            {
                List<string> fileNamesRequested = new List<string>();
                List<string> databaseLogFilesRequested = new List<string>();

                fileNamesRequested.Add(databaseLogFile.GetFileName());
                databaseLogFilesRequested.Add(JsonConvert.SerializeObject(databaseLogFile, Formatting.Indented));

                string fileNamesRequestedJson = JsonConvert.SerializeObject(fileNamesRequested);
                string databaseLogFilesRequestedJson = JsonConvert.SerializeObject(databaseLogFilesRequested);

                string messageToServer = "DatabaseLogFile";
                messageToServer += "|" + fileNamesRequestedJson;
                messageToServer += "|" + databaseLogFilesRequestedJson;
                messageToServer += "\n"; // Para que o servidor encontre o fim do texto

                ClientSocket.Send(Encoding.UTF8.GetBytes(messageToServer));
                //ClientSocket.BeginSend(Encoding.UTF8.GetBytes(messageToServer), 0, Encoding.UTF8.GetBytes(messageToServer).Length, SocketFlags.None, SendCallback, ClientSocket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void DeserializeLogAndAddToList<E>(string jsonLog, List<DatabaseLogFile<E>> logs) where E : class, IModel
        {
            DatabaseLogFile<E> databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<E>>(jsonLog);
            logs.Add(databaseLogFile);
        }

        private async void PersistLogs<E>(DAO dao, IList<DatabaseLogFile<E>> logs) where E : class, IModel
        {
            if (logs.Count > 0)
            {
                List<DatabaseLogFile<E>> saveLogs = logs.Where(w => w.OperacaoMySQL.Equals("INSERT")).ToList();
                List<DatabaseLogFile<E>> updateLogs = logs.Where(w => w.OperacaoMySQL.Equals("UPDATE")).ToList();
                List<DatabaseLogFile<E>> deleteLogs = logs.Where(w => w.OperacaoMySQL.Equals("DELETE")).ToList();

                if (saveLogs.Count > 0)
                {
                    List<E> lista = saveLogs.Select(s => s.Entidade).ToList();
                    await dao.InserirOuAtualizar(lista, true, false);
                }

                if (updateLogs.Count > 0)
                {
                    List<E> lista = updateLogs.Select(s => s.Entidade).ToList();
                    await dao.InserirOuAtualizar(lista, true, false);
                }

                if (deleteLogs.Count > 0)
                {
                    List<E> lista = deleteLogs.Select(s => s.Entidade).ToList();
                    await dao.Deletar(lista, true, false);
                }
            }
        }

        public static DatabaseLogFile<E> WriteDatabaseLogFile<E>(string operacao, E entidade) where E : class, IModel
        {
            if (!Directory.Exists($"{DatabaseLogDir}"))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory($"{DatabaseLogDir}");
                directoryInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            DateTime LastWriteTime = DateTime.Now;
            DatabaseLogFile<E> databaseLogFile = new DatabaseLogFile<E>() { OperacaoMySQL = operacao, Entidade = entidade, LastWriteTime = LastWriteTime };

            string json = JsonConvert.SerializeObject(databaseLogFile, Formatting.Indented);

            File.WriteAllText(Path.Combine(DatabaseLogDir, databaseLogFile.GetFileName()), json);

            return databaseLogFile;
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
