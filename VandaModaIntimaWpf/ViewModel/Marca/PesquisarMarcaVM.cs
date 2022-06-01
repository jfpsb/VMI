using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using VandaModaIntimaWpf.ViewModel.SQL;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    public class PesquisarMarcaVM : APesquisarViewModel<MarcaModel>
    {
        public PesquisarMarcaVM()
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

        public override object GetCadastrarViewModel()
        {
            return new CadastrarMarcaVM(_session);
        }

        public override object GetEditarViewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}
