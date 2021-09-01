using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarFaltasVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro Ao Adicionar Faltas!";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Faltas Foram Adicionadas Com Sucesso!";
        }

        public string MessageBoxCaption()
        {
            return "Adicionar Faltas";
        }
    }
}
