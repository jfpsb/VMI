using System.Collections.ObjectModel;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class PesquisarMarcaViewModel : APesquisarViewModel<MarcaModel>
    {
        public PesquisarMarcaViewModel()
        {
            daoEntidade = new DAOMarca(_session);
            excelStrategy = new ExcelStrategy(new MarcaExcelStrategy(_session));
            pesquisarViewModelStrategy = new PesquisarMarcaViewModelStrategy();
            OnPropertyChanged("TermoPesquisa");
        }
        public override async void PesquisaItens(string termo)
        {
            DAOMarca daoMarca = (DAOMarca)daoEntidade;
            Entidades = new ObservableCollection<EntidadeComCampo<MarcaModel>>(EntidadeComCampo<MarcaModel>.ConverterIList(await daoMarca.ListarPorNome(termo)));
        }
        public override bool Editavel(object parameter)
        {
            return false;
        }
    }
}
