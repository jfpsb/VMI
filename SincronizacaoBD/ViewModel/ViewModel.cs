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
        private Dictionary<String, DateTime> logsLastModify;

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
                    ClientSocket.Connect("18.222.177.179", 3999);

                    Texto += $"{DateTime.Now.ToString(dateFormat)}: Conectado ao Servidor Com Sucesso\n";

                    if (fileSystemWatcher == null)
                    {
                        logsLastModify = new Dictionary<string, DateTime>();
                        fileSystemWatcher = new FileSystemWatcher(Diretorio);
                        fileSystemWatcher.Filter = "*.json";
                        fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
                        fileSystemWatcher.Changed += FileSystemWatcher_OnChanged;
                        fileSystemWatcher.Created += FileSystemWatcher_OnChanged;
                    }

                    fileSystemWatcher.EnableRaisingEvents = true;

                    ApplicationOpening();
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
            if(logsLastModify.ContainsKey(e.Name))
            {
                double intervalo = (DateTime.Now - logsLastModify[e.Name]).TotalMilliseconds;

                if (intervalo < 500)
                    return;
            }

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

                    if (logsLastModify.ContainsKey(e.Name))
                    {
                        logsLastModify[e.Name] = DateTime.Now;
                    }
                    else
                    {
                        logsLastModify.Add(e.Name, DateTime.Now);
                    }
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

                string[] filePaths = Directory.GetFiles(Diretorio);
                List<DatabaseLogFileInfo> databaseLogFileInfos = new List<DatabaseLogFileInfo>();

                foreach (string filePath in filePaths)
                {
                    string fileName = Path.GetFileName(filePath);
                    string tipoLog = Path.GetFileName(fileName).Split(' ')[0];
                    string json = File.ReadAllText(filePath);

                    switch (tipoLog)
                    {
                        case "Contagem":
                            if (!PopulaDatabaseLogFileInfos<Contagem>(fileName, json, databaseLogFileInfos))
                            {
                                continue;
                            }
                            break;
                        case "ContagemProduto":
                            if (!PopulaDatabaseLogFileInfos<ContagemProduto>(fileName, json, databaseLogFileInfos))
                            {
                                continue;
                            }
                            break;
                        case "Fornecedor":
                            if (!PopulaDatabaseLogFileInfos<Fornecedor>(fileName, json, databaseLogFileInfos))
                            {
                                continue;
                            }
                            break;
                        case "Loja":
                            if (!PopulaDatabaseLogFileInfos<Loja>(fileName, json, databaseLogFileInfos))
                            {
                                continue;
                            }
                            break;
                        case "Marca":
                            if (!PopulaDatabaseLogFileInfos<Marca>(fileName, json, databaseLogFileInfos))
                            {
                                continue;
                            }
                            break;
                        case "OperadoraCartao":
                            if (!PopulaDatabaseLogFileInfos<OperadoraCartao>(fileName, json, databaseLogFileInfos))
                            {
                                continue;
                            }
                            break;
                        case "Produto":
                            if (!PopulaDatabaseLogFileInfos<Produto>(fileName, json, databaseLogFileInfos))
                            {
                                continue;
                            }
                            break;
                        case "RecebimentoCartao":
                            if (!PopulaDatabaseLogFileInfos<RecebimentoCartao>(fileName, json, databaseLogFileInfos))
                            {
                                continue;
                            }
                            break;
                        case "TipoContagem":
                            if (!PopulaDatabaseLogFileInfos<TipoContagem>(fileName, json, databaseLogFileInfos))
                            {
                                continue;
                            }
                            break;
                    }
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

        /// <summary>
        /// Deserializa o LOG, cria o DatabaseLogFileInfo e insere na lista fornecida.
        /// Se o LOG for do tipo DELETE e não tiver sido modificado há um ano ou mais, o LOG será deletado.
        /// </summary>
        /// <typeparam name="E">Tipo da entidade do LOG</typeparam>
        /// <param name="fileName">Caminho completo do arquivo de LOG</param>
        /// <param name="json">Json do arquivo de LGO</param>
        /// <param name="databaseLogFileInfos">Lista de DatabaseLogFileInfo</param>
        /// <returns></returns>
        private bool PopulaDatabaseLogFileInfos<E>(string fileName, string json, List<DatabaseLogFileInfo> databaseLogFileInfos) where E : class, IModel
        {
            DatabaseLogFile<E> databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<E>>(json);
            DateTime now = DateTime.Now;
            DateTime lastWriteTime = databaseLogFile.LastWriteTime;

            if ((now.Year - lastWriteTime.Year) >= 1 && databaseLogFile.OperacaoMySQL.Equals("DELETE"))
            {
                File.Delete(fileName);
                return false;
            }

            DatabaseLogFileInfo databaseLogFileInfo = new DatabaseLogFileInfo() { LastModified = lastWriteTime, FileName = fileName };
            databaseLogFileInfos.Add(databaseLogFileInfo);

            return true;
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

                                List<DatabaseLogFile<Contagem>> logsContagem = new List<DatabaseLogFile<Contagem>>();
                                List<DatabaseLogFile<ContagemProduto>> logsContagemProduto = new List<DatabaseLogFile<ContagemProduto>>();
                                List<DatabaseLogFile<Fornecedor>> logsFornecedor = new List<DatabaseLogFile<Fornecedor>>();
                                List<DatabaseLogFile<Loja>> logsLoja = new List<DatabaseLogFile<Loja>>();
                                List<DatabaseLogFile<Marca>> logsMarca = new List<DatabaseLogFile<Marca>>();
                                List<DatabaseLogFile<OperadoraCartao>> logsOperadoraCartao = new List<DatabaseLogFile<OperadoraCartao>>();
                                List<DatabaseLogFile<Produto>> logsProduto = new List<DatabaseLogFile<Produto>>();
                                List<DatabaseLogFile<RecebimentoCartao>> logsRecebimentoCartao = new List<DatabaseLogFile<RecebimentoCartao>>();
                                List<DatabaseLogFile<TipoContagem>> logsTipoContagem = new List<DatabaseLogFile<TipoContagem>>();

                                for (int i = 0; i < fileNamesDatabaseLogFile.Count; i++)
                                {
                                    string fileName = fileNamesDatabaseLogFile[i].Split(' ')[0];

                                    switch (fileName)
                                    {
                                        case "Contagem":
                                            DeserializaDatabaseLogFileRecebido(databaseLogFilesDatabaseLogFile[i], logsContagem);
                                            break;
                                        case "ContagemProduto":
                                            DeserializaDatabaseLogFileRecebido(databaseLogFilesDatabaseLogFile[i], logsContagemProduto);
                                            break;
                                        case "Fornecedor":
                                            DeserializaDatabaseLogFileRecebido(databaseLogFilesDatabaseLogFile[i], logsFornecedor);
                                            break;
                                        case "Loja":
                                            DeserializaDatabaseLogFileRecebido(databaseLogFilesDatabaseLogFile[i], logsLoja);
                                            break;
                                        case "Marca":
                                            DeserializaDatabaseLogFileRecebido(databaseLogFilesDatabaseLogFile[i], logsMarca);
                                            break;
                                        case "OperadoraCartao":
                                            DeserializaDatabaseLogFileRecebido(databaseLogFilesDatabaseLogFile[i], logsOperadoraCartao);
                                            break;
                                        case "Produto":
                                            DeserializaDatabaseLogFileRecebido(databaseLogFilesDatabaseLogFile[i], logsProduto);
                                            break;
                                        case "RecebimentoCartao":
                                            DeserializaDatabaseLogFileRecebido(databaseLogFilesDatabaseLogFile[i], logsRecebimentoCartao);
                                            break;
                                        case "TipoContagem":
                                            DeserializaDatabaseLogFileRecebido(databaseLogFilesDatabaseLogFile[i], logsTipoContagem);
                                            break;
                                    }
                                }

                                DAOSync daoSync = new DAOSync();

                                // Persiste os que não tem chaves estrangeiras antes
                                PersistLogs(daoSync, logsLoja);
                                PersistLogs(daoSync, logsFornecedor);
                                PersistLogs(daoSync, logsOperadoraCartao);
                                PersistLogs(daoSync, logsTipoContagem);

                                // The rest is free real state
                                PersistLogs(daoSync, logsContagem);
                                PersistLogs(daoSync, logsContagemProduto);
                                PersistLogs(daoSync, logsMarca);
                                PersistLogs(daoSync, logsProduto);
                                PersistLogs(daoSync, logsRecebimentoCartao);

                                Texto += $"{DateTime.Now.ToString(dateFormat)}: Logs Atualizados Com Sucesso\n";

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

        private void DeserializaDatabaseLogFileRecebido<E>(string databaseLogFileJson, List<DatabaseLogFile<E>> logs) where E : class, IModel
        {
            var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<E>>(databaseLogFileJson);
            FileInfoLogsRecebidos.Add(new DatabaseLogFileInfo() { FileName = databaseLogFile.GetFileName() });
            logs.Add(databaseLogFile);
        }

        private void PersistLogs<E>(DAOSync dao, IList<DatabaseLogFile<E>> logs) where E : class, IModel
        {
            if (logs.Count > 0)
            {
                List<DatabaseLogFile<E>> saveLogs = logs.Where(w => w.OperacaoMySQL.Equals("INSERT")).ToList();
                List<DatabaseLogFile<E>> updateLogs = logs.Where(w => w.OperacaoMySQL.Equals("UPDATE")).ToList();
                List<DatabaseLogFile<E>> deleteLogs = logs.Where(w => w.OperacaoMySQL.Equals("DELETE")).ToList();

                if (saveLogs.Count > 0)
                {
                    List<E> lista = saveLogs.Select(s => s.Entidade).ToList();
                    if (dao.InserirOuAtualizar(lista))
                    {
                        EscreverDatabaseLogFile(saveLogs);
                    }
                }

                if (updateLogs.Count > 0)
                {
                    List<E> lista = updateLogs.Select(s => s.Entidade).ToList();
                    if (dao.InserirOuAtualizar(lista))
                    {
                        EscreverDatabaseLogFile(updateLogs);
                    }
                }

                if (deleteLogs.Count > 0)
                {
                    List<E> lista = deleteLogs.Select(s => s.Entidade).ToList();
                    if (dao.Deletar(lista))
                    {
                        EscreverDatabaseLogFile(deleteLogs);
                    }
                }
            }
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
