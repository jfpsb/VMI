using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class PesquisarMarcaVM : APesquisarViewModel<MarcaModel>
    {
        public PesquisarMarcaVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<MarcaModel> abrePelaTelaPesquisaService)
            : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            daoEntidade = new DAOMarca(_session);
            excelStrategy = new ExcelStrategy(new MarcaExcelStrategy(_session));
            pesquisarViewModelStrategy = new PesquisarMarcaMsgVMStrategy();
            OnPropertyChanged("TermoPesquisa");
        }
        public override async Task PesquisaItens(string termo)
        {
            DAOMarca daoMarca = (DAOMarca)daoEntidade;
            Entidades = new ObservableCollection<EntidadeComCampo<MarcaModel>>(EntidadeComCampo<MarcaModel>.CriarListaEntidadeComCampo(await daoMarca.ListarPorNome(termo)));
        }
        public override bool Editavel(object parameter)
        {
            return false;
        }
    }
}
