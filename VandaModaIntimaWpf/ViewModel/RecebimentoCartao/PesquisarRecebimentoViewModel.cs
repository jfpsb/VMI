using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecebimentoCartaoModel = VandaModaIntimaWpf.Model.RecebimentoCartao;

namespace VandaModaIntimaWpf.ViewModel.RecebimentoCartao
{
    public class PesquisarRecebimentoViewModel : APesquisarViewModel<RecebimentoCartaoModel>
    {
        public PesquisarRecebimentoViewModel() : base("RecebimentoCartao")
        {

        }
        public override void GetItems(string termo)
        {
            throw new NotImplementedException();
        }
    }
}
