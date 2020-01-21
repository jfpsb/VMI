using System.Collections.ObjectModel;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class PesquisarMarcaViewModel : APesquisarViewModel<MarcaModel>
    {
        public PesquisarMarcaViewModel() : base("Marca")
        {
            daoEntidade = new DAOMarca(_session);
            excelStrategy = new ExcelStrategy(new MarcaExcelStrategy());
            pesquisarViewModelStrategy = new PesquisarMarcaViewModelStrategy();
            OnPropertyChanged("TermoPesquisa");
        }
        public override async void GetItems(string termo)
        {
            DAOMarca daoMarca = (DAOMarca)daoEntidade;
            Entidades = new ObservableCollection<EntidadeComCampo<MarcaModel>>(EntidadeComCampo<MarcaModel>.ConverterIList(await daoMarca.ListarPorNome(termo)));
        }
    }
}
