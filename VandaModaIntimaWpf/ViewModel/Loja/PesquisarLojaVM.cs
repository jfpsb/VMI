using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using VandaModaIntimaWpf.ViewModel.SQL;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    class PesquisarLojaVM : APesquisarViewModel<LojaModel>
    {
        private int pesquisarPor;
        private enum OpcoesPesquisa
        {
            Cnpj,
            Nome
        }
        public PesquisarLojaVM(IMessageBoxService messageBoxService) : base(messageBoxService)
        {
            daoEntidade = new DAOLoja(_session);
            excelStrategy = new LojaExcelStrategy(_session);
            pesquisarViewModelStrategy = new PesquisarLojaMsgVMStrategy();
            //Seleciona o index da combobox e por padrão realiza a pesquisa ao atualizar a propriedade
            //Lista todos os produtos ao abrir tela porque texto está vazio
            PesquisarPor = 0;
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
        public override async Task PesquisaItens(string termo)
        {
            DAOLoja daoLoja = (DAOLoja)daoEntidade;
            switch (pesquisarPor)
            {
                case (int)OpcoesPesquisa.Cnpj:
                    Entidades = new ObservableCollection<EntidadeComCampo<LojaModel>>(EntidadeComCampo<LojaModel>.CriarListaEntidadeComCampo(await daoLoja.ListarPorCnpj(termo)));
                    break;
                case (int)OpcoesPesquisa.Nome:
                    Entidades = new ObservableCollection<EntidadeComCampo<LojaModel>>(EntidadeComCampo<LojaModel>.CriarListaEntidadeComCampo(await daoLoja.ListarPorNome(termo)));
                    break;
            }
        }
        public override bool Editavel(object parameter)
        {
            return true;
        }

        protected override WorksheetContainer<LojaModel>[] GetWorksheetContainers()
        {
            var worksheets = new WorksheetContainer<LojaModel>[1];
            worksheets[0] = new WorksheetContainer<LojaModel>()
            {
                Nome = "Lojas",
                Lista = Entidades.Select(s => s.Entidade).ToList()
            };

            return worksheets;
        }

        public override ACadastrarViewModel<LojaModel> GetCadastrarViewModel()
        {
            return new CadastrarLojaVM(_session, MessageBoxService, false);
        }

        public override ACadastrarViewModel<LojaModel> GetEditarViewModel()
        {
            return new EditarLojaVM(EntidadeSelecionada.Entidade, _session, MessageBoxService);
        }

        public override AAjudarVM GetAjudaVM()
        {
            //TODO: implementar viewmodel
            throw new System.NotImplementedException();
        }

        public override ExportarSQLViewModel<LojaModel> GetExportaSQLVM()
        {
            throw new System.NotImplementedException();
        }

        public override ATelaRelatorio GetTelaRelatorioVM()
        {
            throw new System.NotImplementedException();
        }
    }
}
