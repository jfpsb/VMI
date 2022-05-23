using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;
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
            excelStrategy = new MarcaExcelStrategy(_session);
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

        protected override WorksheetContainer<MarcaModel>[] GetWorksheetContainers()
        {
            var worksheets = new WorksheetContainer<MarcaModel>[1];
            worksheets[0] = new WorksheetContainer<MarcaModel>()
            {
                Nome = "Marcas",
                Lista = Entidades.Select(s => s.Entidade).ToList()
            };

            return worksheets;
        }
    }
}
