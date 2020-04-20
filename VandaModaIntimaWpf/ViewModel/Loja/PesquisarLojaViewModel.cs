using System.Collections.ObjectModel;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    class PesquisarLojaViewModel : APesquisarViewModel<LojaModel>
    {
        private int pesquisarPor;
        private enum OpcoesPesquisa
        {
            Cnpj,
            Nome
        }
        public PesquisarLojaViewModel()
        {
            daoEntidade = new DAOLoja(_session);
            excelStrategy = new ExcelStrategy(new LojaExcelStrategy());
            pesquisarViewModelStrategy = new PesquisarLojaViewModelStrategy();
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
        public override async void GetItems(string termo)
        {
            DAOLoja daoLoja = (DAOLoja)daoEntidade;
            switch (pesquisarPor)
            {
                case (int)OpcoesPesquisa.Cnpj:
                    Entidades = new ObservableCollection<EntidadeComCampo<LojaModel>>(EntidadeComCampo<LojaModel>.ConverterIList(await daoLoja.ListarPorCnpj(termo)));
                    break;
                case (int)OpcoesPesquisa.Nome:
                    Entidades = new ObservableCollection<EntidadeComCampo<LojaModel>>(EntidadeComCampo<LojaModel>.ConverterIList(await daoLoja.ListarPorNome(termo)));
                    break;
            }
        }
        public override bool IsEditable(object parameter)
        {
            return true;
        }
    }
}
