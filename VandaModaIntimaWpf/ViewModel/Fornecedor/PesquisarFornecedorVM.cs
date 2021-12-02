using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    public class PesquisarFornecedorVM : APesquisarViewModel<FornecedorModel>
    {
        private int pesquisarPor;
        public ICommand AbrirCadastrarOnlineComando { get; set; }
        public ICommand AbrirTelaPesquisarRepresentanteComando { get; set; }
        private enum OpcoesPesquisa
        {
            Cnpj,
            Nome,
            Email
        }
        public PesquisarFornecedorVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<FornecedorModel> abrePelaTelaPesquisaService)
            : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            AbrirCadastrarOnlineComando = new RelayCommand(AbrirCadastrarOnline);
            AbrirTelaPesquisarRepresentanteComando = new RelayCommand(AbrirTelaPesquisarRepresentante);
            excelStrategy = new FornecedorExcelStrategy(_session);
            pesquisarViewModelStrategy = new PesquisarFornecedorVMStrategy();
            daoEntidade = new DAOFornecedor(_session);

            //Seleciona o index da combobox e por padrão realiza a pesquisa ao atualizar a propriedade
            //Lista todos os produtos ao abrir tela porque texto está vazio
            PesquisarPor = 0;
        }

        private void AbrirTelaPesquisarRepresentante(object obj)
        {
            ((AbrePelaTelaPesqFornecedorService)AbrePelaTelaPesquisaService).AbrirPesquisarRepresentante();
            OnPropertyChanged("TermoPesquisa");
        }

        private void AbrirCadastrarOnline(object p)
        {
            ((AbrePelaTelaPesqFornecedorService)AbrePelaTelaPesquisaService).AbrirCadastrarOnline(_session);
            OnPropertyChanged("TermoPesquisa");
        }
        public override async Task PesquisaItens(string termo)
        {
            DAOFornecedor daoFornecedor = (DAOFornecedor)daoEntidade;
            switch (pesquisarPor)
            {
                case (int)OpcoesPesquisa.Cnpj:
                    Entidades = new ObservableCollection<EntidadeComCampo<FornecedorModel>>(EntidadeComCampo<FornecedorModel>.CriarListaEntidadeComCampo(await daoFornecedor.ListarPorCnpj(termo)));
                    break;
                case (int)OpcoesPesquisa.Nome:
                    Entidades = new ObservableCollection<EntidadeComCampo<FornecedorModel>>(EntidadeComCampo<FornecedorModel>.CriarListaEntidadeComCampo(await daoFornecedor.ListarPorNome(termo)));
                    break;
                case (int)OpcoesPesquisa.Email:
                    Entidades = new ObservableCollection<EntidadeComCampo<FornecedorModel>>(EntidadeComCampo<FornecedorModel>.CriarListaEntidadeComCampo(await daoFornecedor.ListarPorEmail(termo)));
                    break;
            }
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        protected override WorksheetContainer<FornecedorModel>[] GetWorksheetContainers()
        {
            var worksheets = new WorksheetContainer<FornecedorModel>[1];
            worksheets[0] = new WorksheetContainer<FornecedorModel>()
            {
                Nome = "Marcas",
                Lista = Entidades.Select(s => s.Entidade).ToList()
            };

            return worksheets;
        }

        public int PesquisarPor
        {
            get { return pesquisarPor; }
            set
            {
                pesquisarPor = value;
                OnPropertyChanged("TermoPesquisa"); //Realiza pesquisa se mudar seleção de combobox
            }
        }
    }
}
