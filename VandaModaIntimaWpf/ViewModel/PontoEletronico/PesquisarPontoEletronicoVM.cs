using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;

namespace VandaModaIntimaWpf.ViewModel.PontoEletronico
{
    public class PesquisarPontoEletronicoVM : APesquisarViewModel<Model.PontoEletronico>
    {
        public override bool Editavel(object parameter)
        {
            return false;
        }

        public override object GetCadastrarViewModel()
        {
            throw new NotImplementedException();
        }

        public override object GetEditarViewModel()
        {
            return null;
        }

        public override Task PesquisaItens(string termo)
        {
            throw new NotImplementedException();
        }

        protected override WorksheetContainer<Model.PontoEletronico>[] GetWorksheetContainers()
        {
            throw new NotImplementedException();
        }
    }
}
