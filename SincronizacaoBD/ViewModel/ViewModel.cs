using Newtonsoft.Json;
using SincronizacaoBD.Model;
using SincronizacaoBD.Sincronizacao;
using System;
using System.Collections.Generic;
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

        public ViewModel()
        {
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ClientSocket.Connect(IPAddress.Loopback, 3999);

            ApplicationOpening();

            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(Diretorio);
            fileSystemWatcher.Filter = "*.json";
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            fileSystemWatcher.EnableRaisingEvents = true;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.Created += FileSystemWatcher_Created;

            if (!Directory.Exists(Diretorio))
                Directory.CreateDirectory(Diretorio);
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            string fileText = $"DatabaseLogFile|{e.Name}|{File.ReadAllText(e.FullPath)}";
            ClientSocket.BeginSend(Encoding.UTF8.GetBytes(fileText), 0, Encoding.UTF8.GetBytes(fileText).Length, SocketFlags.None, SendCallback, ClientSocket);
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileText = $"DatabaseLogFile|{e.Name}|{File.ReadAllText(e.FullPath)}";
            ClientSocket.BeginSend(Encoding.UTF8.GetBytes(fileText), 0, Encoding.UTF8.GetBytes(fileText).Length, SocketFlags.None, SendCallback, ClientSocket);
        }

        private void ApplicationOpening()
        {
            string[] fileNames = Directory.GetFiles(Diretorio);
            List<DatabaseLogFileInfo> databaseLogFileInfos = new List<DatabaseLogFileInfo>();

            foreach (string fileName in fileNames)
            {
                String name = Path.GetFileName(fileName);
                DateTime lastModified = File.GetLastWriteTime(fileName);

                DatabaseLogFileInfo databaseLogFileInfo = new DatabaseLogFileInfo() { LastModified = lastModified, FileName = name };
                databaseLogFileInfos.Add(databaseLogFileInfo);
            }

            string databaseLogFileInfosJson = "DatabaseLogFileInfo|" + JsonConvert.SerializeObject(databaseLogFileInfos);

            ClientSocket.BeginSend(Encoding.UTF8.GetBytes(databaseLogFileInfosJson), 0, Encoding.UTF8.GetBytes(databaseLogFileInfosJson).Length, SocketFlags.None, SendCallback, ClientSocket);
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
                            JsonText = SplittedMessage[1];

                            List<DatabaseLogFileInfo> databaseLogFileInfos = JsonConvert.DeserializeObject<List<DatabaseLogFileInfo>>(JsonText);

                            foreach (DatabaseLogFileInfo databaseLogFileInfo in databaseLogFileInfos)
                            {
                                string messageToServer = $"DatabaseLogFile|{databaseLogFileInfo.FileName}|";
                                messageToServer += File.ReadAllText(Path.Combine(Diretorio, databaseLogFileInfo.FileName));
                                ClientSocket.BeginSend(Encoding.UTF8.GetBytes(messageToServer), 0, Encoding.UTF8.GetBytes(messageToServer).Length, SocketFlags.None, SendCallback, ClientSocket);
                            }
                            break;
                        case "DatabaseLogFile":
                            string fileName = SplittedMessage[1];
                            JsonText = SplittedMessage[2];

                            if (fileName.StartsWith("Contagem"))
                            {
                                //databaseLogFile = new DatabaseLogFile<Contagem>();
                            }
                            else if (fileName.StartsWith("ContagemProduto"))
                            {
                                
                            }
                            else if (fileName.StartsWith("Fornecedor"))
                            {
                                var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFileFornecedor>(JsonText);
                                new DAOSync<Fornecedor>().InserirOuAtualizar(databaseLogFile.Entidade);
                            }
                            else if (fileName.StartsWith("Loja"))
                            {
                                var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFileLoja>(JsonText);
                                new DAOSync<Loja>().InserirOuAtualizar(databaseLogFile.Entidade);
                            }
                            else if (fileName.StartsWith("Marca"))
                            {
                                var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFileMarca>(JsonText);
                                new DAOSync<Marca>().InserirOuAtualizar(databaseLogFile.Entidade);
                            }
                            else if (fileName.StartsWith("OperadoraCartao"))
                            {
                                var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFileOperadoraCartao>(JsonText);
                                new DAOSync<OperadoraCartao>().InserirOuAtualizar(databaseLogFile.Entidade);
                            }
                            else if (fileName.StartsWith("Produto"))
                            {
                                var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFileProduto>(JsonText);
                                new DAOSync<Produto>().InserirOuAtualizar(databaseLogFile.Entidade);
                            }
                            else if (fileName.StartsWith("RecebimentoCartao"))
                            {
                                var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFileRecebimentoCartao>(JsonText);
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
