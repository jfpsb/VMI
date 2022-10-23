using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.ViewModel.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.CompraDeFornecedor
{
    public class CadastrarCompraDeFornecedorVM : ACadastrarViewModel<Model.CompraDeFornecedor>
    {
        protected DAOFornecedor daoFornecedor;
        protected DAOLoja daoLoja;
        protected DAORepresentante daoRepresentante;
        protected ObservableCollection<Model.ArquivosCompraFornecedor> _arquivos; //Guarda arquivos de compra adicionados
        protected string caminhoDocVMI = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Vanda Moda Intima", "Arquivos De Compras De Fornecedor");
        private ObservableCollection<Model.Fornecedor> _fornecedores;
        private ObservableCollection<Model.Representante> _representantes;
        private ObservableCollection<Model.Loja> _lojas;
        private Model.ArquivosCompraFornecedor _arquivoSelecionado;

        public ICommand ProcurarArquivoComando { get; set; }
        public ICommand ImportarXmlNFeComando { get; set; }
        public ICommand AbrirArquivoComando { get; set; }
        public ICommand AbrirLocalArquivoComando { get; set; }
        public ICommand DeletarArquivoComando { get; set; }

        public CadastrarCompraDeFornecedorVM(ISession session, bool isUpdate) : base(session, isUpdate)
        {
            viewModelStrategy = new CadastrarCompraDeFornecedorVMStrategy();
            daoEntidade = new DAO<Model.CompraDeFornecedor>(_session);
            daoFornecedor = new DAOFornecedor(_session);
            daoRepresentante = new DAORepresentante(_session);
            daoLoja = new DAOLoja(_session);
            Entidade = new Model.CompraDeFornecedor();
            Entidade.DataPedido = DateTime.Now;
            Arquivos = new ObservableCollection<ArquivosCompraFornecedor>();

            GetFornecedores();
            GetLojas();
            GetRepresentantes();

            AposInserirNoBancoDeDados += CopiarArquivos;

            ProcurarArquivoComando = new RelayCommand(ProcurarArquivo);
            ImportarXmlNFeComando = new RelayCommand(ImportarXmlNFe);
            AbrirArquivoComando = new RelayCommand(AbrirArquivo);
            AbrirLocalArquivoComando = new RelayCommand(AbrirLocalArquivo);
            DeletarArquivoComando = new RelayCommand(DeletarArquivo);
        }

        private void DeletarArquivo(object obj)
        {
            if (ArquivoSelecionado != null)
            {
                var dialogResult = _messageBoxService.Show($"Deseja remover o arquivo {ArquivoSelecionado.Nome}?", "Compra de Fornecedor",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    ArquivoSelecionado.Deletado = true;
                    Entidade.Arquivos.Remove(ArquivoSelecionado);
                    Arquivos.Remove(ArquivoSelecionado);
                }
            }
        }

        private void AbrirLocalArquivo(object obj)
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
                    caminho = Path.Combine(caminhoDocVMI, Entidade.Uuid.ToString(), ArquivoSelecionado.Nome);
                }

                try
                {
                    Process.Start("explorer.exe", $"/select, {caminho}");
                }
                catch (Exception ex)
                {
                    _messageBoxService.Show($"Erro Ao Tentar Abrir Arquivo!\n{ex.Message}");
                }
            }
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
                    caminho = Path.Combine(caminhoDocVMI, Entidade.Uuid.ToString(), ArquivoSelecionado.Nome);
                }

                try
                {
                    Process.Start(caminho);
                }
                catch (Exception ex)
                {
                    _messageBoxService.Show($"Erro Ao Tentar Abrir Arquivo!\n{ex.Message}");
                }
            }
        }
        private async void ImportarXmlNFe(object parameter)
        {
            try
            {
                if (parameter == null)
                    throw new Exception("Parâmetro de comando não configurado para ImportarXmlNFe em Salvar Compra De Fornecedor.");

                var openFileDialog = parameter as IOpenFileDialog;
                var caminhoArquivo = openFileDialog.OpenFileDialog();

                if (caminhoArquivo != null)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(TNfeProc));
                    TNfeProc nfe;

                    try
                    {
                        using (Stream stream = new FileStream(caminhoArquivo, FileMode.Open))
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
                                var msgBox = _messageBoxService.Show("O Fornecedor Desta Nota Fiscal Não Está Cadastrado. Deseja Cadastrar Este Fornecedor?", "Fornecedor Não Encontrado",
                                    System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question, System.Windows.MessageBoxResult.No);

                                if (msgBox == System.Windows.MessageBoxResult.Yes)
                                {
                                    CadastrarFornecedorOnlineVM vm = new CadastrarFornecedorOnlineVM(_session, nfe.NFe.infNFe.emit.Item);
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

                            AddArquivo(caminhoArquivo);
                        }
                    }
                    catch (Exception ex)
                    {
                        _messageBoxService.Show($"Erro Ao Ler Arquivo De Nota Fiscal. Cheque Se O Arquivo Está Marcado Como \"Somente Leitura\", Se Estiver, Desmarque.\n\n{ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _messageBoxService.Show(ex.Message);
            }
        }
        private void CopiarArquivos(AposInserirBDEventArgs e)
        {
            if (Arquivos.Count > 0 && e.Sucesso)
            {
                var dir = Directory.CreateDirectory(Path.Combine(caminhoDocVMI, e.UuidEntidade.ToString()));

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
                    _messageBoxService.Show($"Erro Ao Copiar Arquivos. Certifique-se Que Os Arquivos Escolhidos Ainda Estão No Local E Que Não Estão Abertos.\n\nMensagem de Erro: {ex.Message}");
                }
            }
        }
        private void ProcurarArquivo(object parameter)
        {
            try
            {
                if (parameter == null)
                    throw new Exception("Parâmetro de comando não configurado para ImportarXmlNFe em Salvar Compra De Fornecedor.");

                var openFileDialog = parameter as IOpenFileDialog;
                var caminhoArquivo = openFileDialog.OpenFileDialog();

                if (caminhoArquivo != null)
                {
                    AddArquivo(caminhoArquivo);
                }
            }
            catch (Exception ex)
            {
                _messageBoxService.Show(ex.Message);
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

            Entidade.Arquivos.Add(arquivo);
            Arquivos = new ObservableCollection<ArquivosCompraFornecedor>(Entidade.Arquivos);
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

        public override void ResetaPropriedades(AposInserirBDEventArgs e)
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
