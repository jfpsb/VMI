using Newtonsoft.Json;
using SincronizacaoBD.Model;
using SincronizacaoBD.Sincronizacao;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SincronizacaoBD.ViewModel
{
    public class ViewModel : ObservableObject
    {
        private static readonly string Diretorio = "DatabaseLog";
        private Socket ClientSocket;
        private string MessageReceived = "";
        private byte[] BytesReceivedFromServer = new byte[1024];
        private string texto;
        string dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;

        public ViewModel()
        {
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ClientSocket.Connect(IPAddress.Loopback, 3999);

            Texto += $"{DateTime.Now.ToString(dateFormat)}: Conectado ao Servidor\n";

            ApplicationOpening();

            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(Diretorio);
            fileSystemWatcher.Filter = "*.json";
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            fileSystemWatcher.EnableRaisingEvents = true;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.Created += FileSystemWatcher_Created;
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Texto += $"{DateTime.Now.ToString(dateFormat)}: Arquivo {e.Name} Criado\n";
            string fileText = $"DatabaseLogFile|{e.Name}|{File.ReadAllText(e.FullPath)}";
            ClientSocket.BeginSend(Encoding.UTF8.GetBytes(fileText), 0, Encoding.UTF8.GetBytes(fileText).Length, SocketFlags.None, SendCallback, ClientSocket);
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Texto += $"{DateTime.Now.ToString(dateFormat)}: Arquivo {e.Name} Atualizado\n";
            string fileText = $"DatabaseLogFile|{e.Name}|{File.ReadAllText(e.FullPath)}";
            ClientSocket.BeginSend(Encoding.UTF8.GetBytes(fileText), 0, Encoding.UTF8.GetBytes(fileText).Length, SocketFlags.None, SendCallback, ClientSocket);
        }

        private void ApplicationOpening()
        {
            if (!Directory.Exists(Diretorio))
                Directory.CreateDirectory(Diretorio);

            string[] fileNames = Directory.GetFiles(Diretorio);
            List<DatabaseLogFileInfo> databaseLogFileInfos = new List<DatabaseLogFileInfo>();

            foreach (string fileName in fileNames)
            {
                string name = Path.GetFileName(fileName);
                DateTime lastModified = File.GetLastWriteTime(fileName);
                DatabaseLogFileInfo databaseLogFileInfo = new DatabaseLogFileInfo() { LastModified = lastModified, FileName = name };
                databaseLogFileInfos.Add(databaseLogFileInfo);
            }

            string databaseLogFileInfosJson = "DatabaseLogFileInfo|" + JsonConvert.SerializeObject(databaseLogFileInfos) + "\n";

            ClientSocket.Send(Encoding.UTF8.GetBytes(databaseLogFileInfosJson));
            ClientSocket.BeginReceive(BytesReceivedFromServer, 0, BytesReceivedFromServer.Length, SocketFlags.None, ReceiveCallback, ClientSocket);
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
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
                    string JsonText = "";

                    switch (MessageId)
                    {
                        case "DatabaseLogFileRequest":
                            // Recebendo requests de LOGS que devem ser enviados ao servidor
                            Texto += $"{DateTime.Now.ToString(dateFormat)}: Recebido Pedido Do Servidor Para Envio De Log\n";
                            JsonText = SplittedMessage[1];

                            List<DatabaseLogFileInfo> databaseLogFileInfos = JsonConvert.DeserializeObject<List<DatabaseLogFileInfo>>(JsonText);

                            var fileNames = new List<string>();
                            var databaseLogFiles = new List<string>();

                            foreach (DatabaseLogFileInfo databaseLogFileInfo in databaseLogFileInfos)
                            {
                                fileNames.Add(databaseLogFileInfo.FileName);
                                databaseLogFiles.Add(File.ReadAllText(Path.Combine(Diretorio, databaseLogFileInfo.FileName)).Replace("\r", string.Empty).Replace("\n", string.Empty));
                            }

                            string fileNamesJson = JsonConvert.SerializeObject(fileNames);
                            string databaseLogFilesJson = JsonConvert.SerializeObject(databaseLogFiles);

                            string messageToServer = "DatabaseLogFile";
                            messageToServer += "|" + fileNamesJson;
                            messageToServer += "|" + databaseLogFilesJson;
                            messageToServer += "\n"; // Para que o servidor encontre o fim do texto

                            ClientSocket.BeginSend(Encoding.UTF8.GetBytes(messageToServer), 0, Encoding.UTF8.GetBytes(messageToServer).Length, SocketFlags.None, SendCallback, ClientSocket);

                            break;
                        case "DatabaseLogFile":
                            // Recebendo LOG do servidor
                            string fileName = SplittedMessage[1];
                            JsonText = SplittedMessage[2];

                            Texto += $"{DateTime.Now.ToString(dateFormat)}: Recebido Log Do Servidor. Inserindo No Banco de Dados. LOG {fileName}\n";

                            if (fileName.StartsWith("Contagem"))
                            {
                                //databaseLogFile = new DatabaseLogFile<Contagem>();
                            }
                            else if (fileName.StartsWith("ContagemProduto"))
                            {

                            }
                            else if (fileName.StartsWith("Fornecedor"))
                            {
                                var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<Fornecedor>>(JsonText);
                                new DAOSync<Fornecedor>().InserirOuAtualizar(databaseLogFile.Entidade);
                            }
                            else if (fileName.StartsWith("Loja"))
                            {
                                var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<Loja>>(JsonText);
                                new DAOSync<Loja>().InserirOuAtualizar(databaseLogFile.Entidade);
                            }
                            else if (fileName.StartsWith("Marca"))
                            {
                                var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<Marca>>(JsonText);
                                new DAOSync<Marca>().InserirOuAtualizar(databaseLogFile.Entidade);
                            }
                            else if (fileName.StartsWith("OperadoraCartao"))
                            {
                                var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<OperadoraCartao>>(JsonText);
                                new DAOSync<OperadoraCartao>().InserirOuAtualizar(databaseLogFile.Entidade);
                            }
                            else if (fileName.StartsWith("Produto"))
                            {
                                var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<Produto>>(JsonText);
                                new DAOSync<Produto>().InserirOuAtualizar(databaseLogFile.Entidade);
                            }
                            else if (fileName.StartsWith("RecebimentoCartao"))
                            {
                                var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<RecebimentoCartao>>(JsonText);
                                new DAOSync<RecebimentoCartao>().InserirOuAtualizar(databaseLogFile.Entidade);
                            }
                            else if (fileName.StartsWith("TipoContagem"))
                            {
                                //databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<TipoContagem>>(JsonText);
                            }
                            break;
                    }

                    MessageReceived = "";
                }

                socket.BeginReceive(BytesReceivedFromServer, 0, BytesReceivedFromServer.Length, SocketFlags.None, ReceiveCallback, socket);
            }
        }

        private void SendCallback(IAsyncResult asyncResult)
        {
            Socket socket = (Socket)asyncResult.AsyncState;
            socket.EndSend(asyncResult);
        }

        public void FechaSessionFactories()
        {
            SessionSyncProvider.FechaConexoesLocal();
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

        private void AdicionaTexto(string texto)
        {
            Texto += texto;
        }
    }
}
