using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.ViewModel.Fornecedor;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.CompraDeFornecedor
{
    public class CadastrarCompraDeFornecedorVM : ACadastrarViewModel<Model.CompraDeFornecedor>
    {
        protected DAOFornecedor daoFornecedor;
        protected DAOLoja daoLoja;
        protected DAORepresentante daoRepresentante;
        protected ObservableCollection<Model.ArquivosCompraFornecedor> _arquivos; //Guarda arquivos de compra adicionados
        protected IFileDialogService _fileDialogService;
        protected string caminhoDocVMI = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Vanda Moda Íntima", "Arquivos De Compras De Fornecedor");
        private ObservableCollection<Model.Fornecedor> _fornecedores;
        private ObservableCollection<Model.Representante> _representantes;
        private ObservableCollection<Model.Loja> _lojas;
        private Model.ArquivosCompraFornecedor _arquivoSelecionado;

        public ICommand ProcurarArquivoComando { get; set; }
        public ICommand ImportarXmlNFeComando { get; set; }
        public ICommand AbrirArquivoComando { get; set; }

        public CadastrarCompraDeFornecedorVM(ISession session, IMessageBoxService messageBoxService, IFileDialogService fileDialogService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            _session = session;
            _fileDialogService = fileDialogService;
            MessageBoxService = messageBoxService;
            IssoEUmUpdate = issoEUmUpdate;
            viewModelStrategy = new CadastrarCompraDeFornecedorVMStrategy();
            daoEntidade = new DAO<Model.CompraDeFornecedor>(session);
            daoFornecedor = new DAOFornecedor(session);
            daoRepresentante = new DAORepresentante(session);
            daoLoja = new DAOLoja(session);
            Entidade = new Model.CompraDeFornecedor();
            Entidade.DataPedido = DateTime.Now;
            Arquivos = new ObservableCollection<ArquivosCompraFornecedor>();

            GetFornecedores();
            GetLojas();
            GetRepresentantes();

            AntesDeInserirNoBancoDeDados += InsereArquivosEmEntidade;
            AposInserirNoBancoDeDados += CopiarArquivos;

            ProcurarArquivoComando = new RelayCommand(ProcurarArquivo);
            ImportarXmlNFeComando = new RelayCommand(ImportarXmlNFe);
            AbrirArquivoComando = new RelayCommand(AbrirArquivo);
        }

        private void AbrirArquivo(object obj)
        {
            if (ArquivoSelecionado != null)
            {
                string caminho;

                if (ArquivoSelecionado.CaminhoOriginal != null)
                {
                    caminho = ArquivoSelecionado.CaminhoOriginal;
                }
                else
                {
                    caminho = Path.Combine(caminhoDocVMI, Entidade.Id.ToString(), ArquivoSelecionado.Nome);
                }

                Process.Start(caminho);
            }
        }

        private async void ImportarXmlNFe(object obj)
        {
            var caminho = _fileDialogService.ShowFileBrowserDialog("Arquivo De Nota Fiscal (.xml)|*.xml");

            if (caminho.Length > 0)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TNfeProc));
                TNfeProc nfe;

                try
                {
                    using (Stream stream = new FileStream(caminho, FileMode.Open))
                    {
                        nfe = serializer.Deserialize(stream) as TNfeProc;

                        Entidade.Valor = double.Parse(nfe.NFe.infNFe.total.ICMSTot.vNF, CultureInfo.InvariantCulture);
                        Entidade.DataNotaFiscal = DateTime.Parse(nfe.NFe.infNFe.ide.dhEmi);
                        Entidade.NumeroNfe = int.Parse(nfe.NFe.infNFe.ide.nNF);
                        Entidade.ChaveAcessoNfe = Regex.Replace(nfe.NFe.infNFe.Id, "[^0-9]", string.Empty);

                        Model.Fornecedor fornecedor = await daoFornecedor.ListarPorId(nfe.NFe.infNFe.emit.Item);
                        Model.Loja loja = await daoLoja.ListarPorId(nfe.NFe.infNFe.dest.Item);

                        if (fornecedor == null)
                        {
                            var msgBox = MessageBoxService.Show("O Fornecedor Desta Nota Fiscal Não Está Cadastrado. Deseja Cadastrar Este Fornecedor?", "Fornecedor Não Encontrado",
                                System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question, System.Windows.MessageBoxResult.No);

                            if (msgBox == System.Windows.MessageBoxResult.Yes)
                            {
                                CadastrarFornecedorOnlineVM vm = new CadastrarFornecedorOnlineVM(_session, MessageBoxService, nfe.NFe.infNFe.emit.Item, false);
                                SalvarFornecedor view = new SalvarFornecedor { DataContext = vm };
                                var result = view.ShowDialog();

                                if (result == true)
                                {
                                    GetFornecedores();
                                    fornecedor = await daoFornecedor.ListarPorId(nfe.NFe.infNFe.emit.Item);
                                }
                            }
                        }

                        if (fornecedor != null)
                            Entidade.Fornecedor = fornecedor;

                        if (loja != null)
                            Entidade.Loja = loja;

                        AddArquivo(caminho);
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show($"Erro Ao Ler Arquivo De Nota Fiscal. Cheque Se O Arquivo Está Marcado Como \"Somente Leitura\", Se Estiver, Desmarque.\n\n{ex.Message}");
                }
            }
        }

        private void InsereArquivosEmEntidade()
        {
            Entidade.Arquivos.Clear();
            foreach (var arquivo in Arquivos)
                Entidade.Arquivos.Add(arquivo);
        }

        private void CopiarArquivos(AposInserirBDEventArgs e)
        {
            if (Arquivos.Count > 0 && e.Sucesso)
            {
                var dir = Directory.CreateDirectory(Path.Combine(caminhoDocVMI, e.IdentificadorEntidade.ToString()));

                try
                {
                    if (IssoEUmUpdate)
                    {
                        foreach (var arquivo in Arquivos)
                        {
                            if (File.Exists(Path.Combine(dir.FullName, arquivo.Nome)))
                                continue;

                            File.Copy(arquivo.CaminhoOriginal, Path.Combine(dir.FullName, arquivo.Nome), true);
                        }
                    }
                    else
                    {
                        foreach (var arquivo in Arquivos)
                        {
                            File.Copy(arquivo.CaminhoOriginal, Path.Combine(dir.FullName, arquivo.Nome), true);
                        }

                        Arquivos.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show($"Erro Ao Copiar Arquivos. Certifique-se Que Os Arquivos Escolhidos Ainda Estão No Local E Que Não Estão Abertos.\n\nMensagem de Erro: {ex.Message}");
                }
            }
        }

        private void ProcurarArquivo(object obj)
        {
            var caminho = _fileDialogService.ShowFileBrowserDialog();

            if (caminho.Length > 0)
            {
                AddArquivo(caminho);
            }
        }

        private void AddArquivo(string caminho)
        {
            ArquivosCompraFornecedor arquivo = new ArquivosCompraFornecedor
            {
                CaminhoOriginal = caminho,
                Extensao = Path.GetExtension(caminho),
                Nome = Path.GetFileName(caminho),
                CompraDeFornecedor = Entidade
            };

            Arquivos.Add(arquivo);
        }

        public ObservableCollection<ArquivosCompraFornecedor> Arquivos
        {
            get => _arquivos;
            set
            {
                _arquivos = value;
                OnPropertyChanged("Arquivos");
            }
        }
        public ObservableCollection<Model.Fornecedor> Fornecedores
        {
            get => _fornecedores;
            set
            {
                _fornecedores = value;
                OnPropertyChanged("Fornecedores");
            }
        }
        public ObservableCollection<Model.Loja> Lojas
        {
            get => _lojas;
            set
            {
                _lojas = value;
                OnPropertyChanged("Lojas");
            }
        }

        public ArquivosCompraFornecedor ArquivoSelecionado
        {
            get => _arquivoSelecionado;
            set
            {
                _arquivoSelecionado = value;
                OnPropertyChanged("ArquivoSelecionado");
            }
        }

        public ObservableCollection<Model.Representante> Representantes
        {
            get => _representantes;
            set
            {
                _representantes = value;
                OnPropertyChanged("Representantes");
            }
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Fornecedor":
                    if (Entidade.Fornecedor != null && Entidade.Fornecedor.Cnpj == null)
                    {
                        Entidade.Fornecedor = null;
                        break;
                    }

                    if (Entidade.Fornecedor != null)
                    {
                        Entidade.Representante = Entidade.Fornecedor.Representante;
                    }
                    break;
                case "Loja":
                    if (Entidade.Loja != null && Entidade.Loja.Cnpj == null)
                    {
                        Entidade.Loja = null;
                    }
                    break;
                case "Representante":
                    if (Entidade.Representante != null && Entidade.Representante.Id == 0)
                    {
                        Entidade.Representante = null;
                    }
                    break;
            }
        }

        public override void ResetaPropriedades()
        {
            Entidade = new Model.CompraDeFornecedor();
            Entidade.DataPedido = DateTime.Now;
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (Entidade.Valor < 0)
            {
                BtnSalvarToolTip += "Informe Um Valor Válido Para Esta Compra De Fornecedor!\n";
                valido = false;
            }

            if (Entidade.Representante == null && Entidade.Fornecedor == null)
            {
                BtnSalvarToolTip += "Selecione A Origem Da Compra (Representante Ou Fornecedor)!\n";
                valido = false;
            }

            return valido;
        }

        private async void GetFornecedores()
        {
            Fornecedores = new ObservableCollection<Model.Fornecedor>(await daoFornecedor.Listar());
            Fornecedores.Insert(0, new Model.Fornecedor(GetResource.GetString("fornecedor_nao_selecionado")));
        }

        private async void GetLojas()
        {
            Lojas = new ObservableCollection<Model.Loja>(await daoLoja.Listar());
            Lojas.Insert(0, new Model.Loja("LOJA NÃO SELECIONADA"));
        }

        private async void GetRepresentantes()
        {
            Representantes = new ObservableCollection<Model.Representante>(await daoRepresentante.Listar());
            Representantes.Insert(0, new Model.Representante { Nome = "REPRESENTANTE NÃO SELECIONADO" });
        }
    }
}
