using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Representante
{
    public class PesquisarRepresentanteVM : APesquisarViewModel<Model.Representante>
    {
        public PesquisarRepresentanteVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<Model.Representante> abrePelaTelaPesquisaService) : base(messageBoxService, abrePelaTelaPesquisaService)
        {
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        public override void PesquisaItens(string termo)
        {
            throw new NotImplementedException();
        }
    }
}
