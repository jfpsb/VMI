using Newtonsoft.Json;
using SincronizacaoBD.Model;
using SincronizacaoBD.Sincronizacao;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SincronizacaoBD.ViewModel
{
    public class ViewModel : ObservableObject, IDisposable
    {
        private static readonly string Diretorio = "DatabaseLog";
        private Socket ClientSocket;
        private string MessageReceived = "";
        private byte[] BytesReceivedFromServer = new byte[1024];
        private string texto;
        string dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
        private FileSystemWatcher fileSystemWatcher = null;
        private List<DatabaseLogFileInfo> FileInfoLogsRecebidos = new List<DatabaseLogFileInfo>();
        private bool disposed;

        public ViewModel()
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
                    Texto += $"{DateTime.Now.ToString(dateFormat)}: Conectando ao Servidor\n";

                    ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    ClientSocket.Connect(IPAddress.Loopback, 3999);

                    Texto += $"{DateTime.Now.ToString(dateFormat)}: Conectado ao Servidor Com Sucesso\n";

                    ApplicationOpening();

                    if (fileSystemWatcher == null)
                    {
                        fileSystemWatcher = new FileSystemWatcher(Diretorio);
                        fileSystemWatcher.Filter = "*.json";
                        fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
                        fileSystemWatcher.Changed += FileSystemWatcher_OnChanged;
                        fileSystemWatcher.Created += FileSystemWatcher_OnChanged;
                    }

                    fileSystemWatcher.EnableRaisingEvents = true;
                }
                catch (SocketException se)
                {
                    ErroAoConectar(se, "Conectar", false);
                    timerConexaoServidor.Change(30000, Timeout.Infinite);
                }
            }, null, 0, Timeout.Infinite);
        }

        private void FileSystemWatcher_OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                Texto += $"{DateTime.Now.ToString(dateFormat)}: Arquivo {e.Name} Criado\n";
            }
            else
            {
                Texto += $"{DateTime.Now.ToString(dateFormat)}: Arquivo {e.Name} Atualizado\n";
            }

            DatabaseLogFileInfo databaseLogFileInfo = new DatabaseLogFileInfo() { FileName = e.Name };

            // Impede que o arquivo recebido do servidor seja enviado de volta ao servidor quando é criado na pasta DatabaseLog
            if (!FileInfoLogsRecebidos.Contains(databaseLogFileInfo))
            {
                try
                {
                    List<DatabaseLogFileInfo> databaseLogFileInfos = new List<DatabaseLogFileInfo>();
                    databaseLogFileInfos.Add(databaseLogFileInfo);
                    GeraEEnviaMensagemDatabaseFileLog(JsonConvert.SerializeObject(databaseLogFileInfos));
                    Texto += $"{DateTime.Now.ToString(dateFormat)}: Arquivo {e.Name} Enviado Ao Servidor\n";
                }
                catch (SocketException se)
                {
                    ErroAoConectar(se, "FileSystemWatcher_OnChanged");
                }
                catch (Exception ex)
                {
                    string erro = $"FileSystemWatcher_OnChanged | Erro ao Enviar DatabaseLogFile | {ex.Message}";
                    Console.WriteLine(erro);
                    Texto += erro + "\n";
                }
            }

            FileInfoLogsRecebidos.Remove(databaseLogFileInfo);
        }
        private void ApplicationOpening()
        {
            try
            {
                if (!Directory.Exists(Diretorio))
                    Directory.CreateDirectory(Diretorio);

                string[] fileNames = Directory.GetFiles(Diretorio);
                List<DatabaseLogFileInfo> databaseLogFileInfos = new List<DatabaseLogFileInfo>();

                foreach (string fileName in fileNames)
                {
                    string name = Path.GetFileName(fileName);
                    string json = File.ReadAllText(fileName);
                    DateTime lastModified = new DateTime();

                    //TODO: Pensar em algum jeito de melhorar isso aqui
                    if (name.StartsWith("Contagem"))
                    {
                        //DatabaseLogFile<Contagem> databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<Contagem>>(json);
                        //lastModified = databaseLogFile.LastWriteTime;
                    }
                    else if (name.StartsWith("ContagemProduto"))
                    {
                        //DatabaseLogFile<ContagemProduto> databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<ContagemProduto>>(json);
                        //lastModified = databaseLogFile.LastWriteTime;
                    }
                    else if (name.StartsWith("Fornecedor"))
                    {
                        DatabaseLogFile<Fornecedor> databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<Fornecedor>>(json);
                        lastModified = databaseLogFile.LastWriteTime;
                    }
                    else if (name.StartsWith("Loja"))
                    {
                        DatabaseLogFile<Loja> databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<Loja>>(json);
                        lastModified = databaseLogFile.LastWriteTime;
                    }
                    else if (name.StartsWith("Marca"))
                    {
                        DatabaseLogFile<Marca> databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<Marca>>(json);
                        lastModified = databaseLogFile.LastWriteTime;
                    }
                    else if (name.StartsWith("OperadoraCartao"))
                    {
                        DatabaseLogFile<OperadoraCartao> databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<OperadoraCartao>>(json);
                        lastModified = databaseLogFile.LastWriteTime;
                    }
                    else if (name.StartsWith("Produto"))
                    {
                        DatabaseLogFile<Produto> databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<Produto>>(json);
                        lastModified = databaseLogFile.LastWriteTime;
                    }
                    else if (name.StartsWith("RecebimentoCartao"))
                    {
                        DatabaseLogFile<RecebimentoCartao> databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<RecebimentoCartao>>(json);
                        lastModified = databaseLogFile.LastWriteTime;
                    }
                    else if (name.StartsWith("TipoContagem"))
                    {
                        //DatabaseLogFile<TipoContagem> databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<TipoContagem>>(json);
                        //lastModified = databaseLogFile.LastWriteTime;
                    }
                    else
                    {
                        throw new Exception("Classe Não Existe");
                    }

                    DatabaseLogFileInfo databaseLogFileInfo = new DatabaseLogFileInfo() { LastModified = lastModified, FileName = name };
                    databaseLogFileInfos.Add(databaseLogFileInfo);
                }

                string databaseLogFileInfosJson = "DatabaseLogFileInfo|" + JsonConvert.SerializeObject(databaseLogFileInfos) + "\n";

                Texto += $"{DateTime.Now.ToString(dateFormat)}: Sincronização Executando\n";
                ClientSocket.Send(Encoding.UTF8.GetBytes(databaseLogFileInfosJson));
                ClientSocket.BeginReceive(BytesReceivedFromServer, 0, BytesReceivedFromServer.Length, SocketFlags.None, ReceiveCallback, ClientSocket);
            }
            catch (SocketException se)
            {
                ErroAoConectar(se, "ApplicationOpening");
            }
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

                    if (MessageReceived.EndsWith("\n"))
                    {
                        string[] SplittedMessage = MessageReceived.Split('|');

                        string MessageId = SplittedMessage[0];

                        switch (MessageId)
                        {
                            case "DatabaseLogFileRequest":
                                // Recebendo requests de LOGS que devem ser enviados ao servidor
                                Texto += $"{DateTime.Now.ToString(dateFormat)}: Recebido Pedido Do Servidor Para Envio De Logs\n";
                                string dataLogFileRequest = SplittedMessage[1];
                                GeraEEnviaMensagemDatabaseFileLog(dataLogFileRequest);
                                Texto += $"{DateTime.Now.ToString(dateFormat)}: Logs Enviados Ao Servidor\n";
                                break;
                            case "DatabaseLogFile":
                                // Recebendo LOG do servidor
                                string fileNamesDatabaseLogFileJson = SplittedMessage[1];
                                string databaseLogFilesDatabaseLogFileJson = SplittedMessage[2];

                                Texto += $"{DateTime.Now.ToString(dateFormat)}: Recebido Log Do Servidor. Inserindo No Banco de Dados\n";

                                List<string> fileNamesDatabaseLogFile = JsonConvert.DeserializeObject<List<string>>(fileNamesDatabaseLogFileJson);
                                List<string> databaseLogFilesDatabaseLogFile = JsonConvert.DeserializeObject<List<string>>(databaseLogFilesDatabaseLogFileJson);
                                List<Object> saveUpdateList = new List<Object>();
                                List<Object> deleteList = new List<Object>();

                                List<DatabaseLogFile<Fornecedor>> logsFornecedor = new List<DatabaseLogFile<Fornecedor>>();
                                List<DatabaseLogFile<Loja>> logsLoja = new List<DatabaseLogFile<Loja>>();
                                List<DatabaseLogFile<Marca>> logsMarca = new List<DatabaseLogFile<Marca>>();
                                List<DatabaseLogFile<OperadoraCartao>> logsOperadoraCartao = new List<DatabaseLogFile<OperadoraCartao>>();
                                List<DatabaseLogFile<Produto>> logsProduto = new List<DatabaseLogFile<Produto>>();
                                List<DatabaseLogFile<RecebimentoCartao>> logsRecebimentoCartao = new List<DatabaseLogFile<RecebimentoCartao>>();

                                for (int i = 0; i < fileNamesDatabaseLogFile.Count; i++)
                                {
                                    //TODO: Pensar em algum jeito de melhorar isso aqui
                                    if (fileNamesDatabaseLogFile[i].StartsWith("Contagem"))
                                    {
                                        //databaseLogFile = new DatabaseLogFile<Contagem>();
                                    }
                                    else if (fileNamesDatabaseLogFile[i].StartsWith("ContagemProduto"))
                                    {

                                    }
                                    else if (fileNamesDatabaseLogFile[i].StartsWith("Fornecedor"))
                                    {
                                        DeserializaDatabaseLogFileJson(databaseLogFilesDatabaseLogFile[i], logsFornecedor, saveUpdateList, deleteList);
                                    }
                                    else if (fileNamesDatabaseLogFile[i].StartsWith("Loja"))
                                    {
                                        DeserializaDatabaseLogFileJson(databaseLogFilesDatabaseLogFile[i], logsLoja, saveUpdateList, deleteList);
                                    }
                                    else if (fileNamesDatabaseLogFile[i].StartsWith("Marca"))
                                    {
                                        DeserializaDatabaseLogFileJson(databaseLogFilesDatabaseLogFile[i], logsMarca, saveUpdateList, deleteList);
                                    }
                                    else if (fileNamesDatabaseLogFile[i].StartsWith("OperadoraCartao"))
                                    {
                                        DeserializaDatabaseLogFileJson(databaseLogFilesDatabaseLogFile[i], logsOperadoraCartao, saveUpdateList, deleteList);
                                    }
                                    else if (fileNamesDatabaseLogFile[i].StartsWith("Produto"))
                                    {
                                        DeserializaDatabaseLogFileJson(databaseLogFilesDatabaseLogFile[i], logsProduto, saveUpdateList, deleteList);
                                    }
                                    else if (fileNamesDatabaseLogFile[i].StartsWith("RecebimentoCartao"))
                                    {
                                        DeserializaDatabaseLogFileJson(databaseLogFilesDatabaseLogFile[i], logsRecebimentoCartao, saveUpdateList, deleteList);
                                    }
                                    else if (fileNamesDatabaseLogFile[i].StartsWith("TipoContagem"))
                                    {
                                        //databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<TipoContagem>>(databaseLogFilesDatabaseLogFile[i]);
                                    }
                                }

                                DAOSync daoSync = new DAOSync();

                                if (daoSync.InserirOuAtualizar(saveUpdateList))
                                {
                                    EscreverDatabaseLogFile(logsFornecedor.Where(w => w.OperacaoMySQL.Equals("INSERT") || w.OperacaoMySQL.Equals("UPDATE")).ToList());
                                    EscreverDatabaseLogFile(logsLoja.Where(w => w.OperacaoMySQL.Equals("INSERT") || w.OperacaoMySQL.Equals("UPDATE")).ToList());
                                    EscreverDatabaseLogFile(logsMarca.Where(w => w.OperacaoMySQL.Equals("INSERT") || w.OperacaoMySQL.Equals("UPDATE")).ToList());
                                    EscreverDatabaseLogFile(logsOperadoraCartao.Where(w => w.OperacaoMySQL.Equals("INSERT") || w.OperacaoMySQL.Equals("UPDATE")).ToList());
                                    EscreverDatabaseLogFile(logsProduto.Where(w => w.OperacaoMySQL.Equals("INSERT") || w.OperacaoMySQL.Equals("UPDATE")).ToList());
                                    EscreverDatabaseLogFile(logsRecebimentoCartao.Where(w => w.OperacaoMySQL.Equals("INSERT") || w.OperacaoMySQL.Equals("UPDATE")).ToList());

                                    Texto += $"{DateTime.Now.ToString(dateFormat)}: Logs Inseridos Ou Atualizados Com Sucesso\n";
                                }

                                if (daoSync.Deletar(deleteList))
                                {
                                    EscreverDatabaseLogFile(logsFornecedor.Where(w => w.OperacaoMySQL.Equals("DELETE")).ToList());
                                    EscreverDatabaseLogFile(logsLoja.Where(w => w.OperacaoMySQL.Equals("DELETE")).ToList());
                                    EscreverDatabaseLogFile(logsMarca.Where(w => w.OperacaoMySQL.Equals("DELETE")).ToList());
                                    EscreverDatabaseLogFile(logsOperadoraCartao.Where(w => w.OperacaoMySQL.Equals("DELETE")).ToList());
                                    EscreverDatabaseLogFile(logsProduto.Where(w => w.OperacaoMySQL.Equals("DELETE")).ToList());
                                    EscreverDatabaseLogFile(logsRecebimentoCartao.Where(w => w.OperacaoMySQL.Equals("DELETE")).ToList());

                                    Texto += $"{DateTime.Now.ToString(dateFormat)}: Logs Deletados Com Sucesso\n";
                                }

                                break;
                        }

                        MessageReceived = "";
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

        public string Texto
        {
            get { return texto; }
            set
            {
                texto = value;
                OnPropertyChanged("Texto");
            }
        }

        private void InsereLista(string operacao, Object entidade, List<Object> saveUpdateList, List<Object> deleteList)
        {
            switch (operacao)
            {
                case "INSERT":
                    saveUpdateList.Add(entidade);
                    break;
                case "UPDATE":
                    saveUpdateList.Add(entidade);
                    break;
                case "DELETE":
                    deleteList.Add(entidade);
                    break;
            }
        }

        private void DeserializaDatabaseLogFileJson<E>(string databaseLogFileJson, List<DatabaseLogFile<E>> logs, List<Object> saveUpdateList, List<Object> deleteList) where E : class, IModel
        {
            var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<E>>(databaseLogFileJson);
            FileInfoLogsRecebidos.Add(new DatabaseLogFileInfo() { FileName = databaseLogFile.GetFileName() });
            logs.Add(databaseLogFile);
            InsereLista(databaseLogFile.OperacaoMySQL, databaseLogFile.Entidade, saveUpdateList, deleteList);
        }

        private void EscreverDatabaseLogFile<E>(List<DatabaseLogFile<E>> logs) where E : class, IModel
        {
            foreach (DatabaseLogFile<E> databaseLogFile in logs)
            {
                OperacoesDatabaseLogFile<E>.EscreverJson(databaseLogFile);
            }
        }

        private void GeraEEnviaMensagemDatabaseFileLog(string dataLogFileRequest)
        {
            List<DatabaseLogFileInfo> databaseLogFileInfos = JsonConvert.DeserializeObject<List<DatabaseLogFileInfo>>(dataLogFileRequest);

            var fileNamesRequested = new List<string>();
            var databaseLogFilesRequested = new List<string>();

            foreach (DatabaseLogFileInfo databaseLogFileInfo in databaseLogFileInfos)
            {
                fileNamesRequested.Add(databaseLogFileInfo.FileName);
                databaseLogFilesRequested.Add(File.ReadAllText(Path.Combine(Diretorio, databaseLogFileInfo.FileName)).Replace("\r", string.Empty).Replace("\n", string.Empty));
            }

            string fileNamesRequestedJson = JsonConvert.SerializeObject(fileNamesRequested);
            string databaseLogFilesRequestedJson = JsonConvert.SerializeObject(databaseLogFilesRequested);

            string messageToServer = "DatabaseLogFile";
            messageToServer += "|" + fileNamesRequestedJson;
            messageToServer += "|" + databaseLogFilesRequestedJson;
            messageToServer += "\n"; // Para que o servidor encontre o fim do texto

            ClientSocket.BeginSend(Encoding.UTF8.GetBytes(messageToServer), 0, Encoding.UTF8.GetBytes(messageToServer).Length, SocketFlags.None, SendCallback, ClientSocket);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.Close();
            }

            disposed = true;
        }

        private void ErroAoConectar(SocketException se, string metodo, bool callConectar = true)
        {
            Texto += $"{metodo} | Não É Possível Conectar Ao Servidor.\nTentando Novamente Em 30 Segundos.\nMensagem de Erro: {se.Message}\n";
            Console.WriteLine(se.Message);
            Console.WriteLine("Socket Error Code: " + se.SocketErrorCode);
            Console.WriteLine(se.StackTrace);

            if (fileSystemWatcher != null)
                fileSystemWatcher.EnableRaisingEvents = false;

            if (callConectar)
                Conectar();
        }
    }
}
