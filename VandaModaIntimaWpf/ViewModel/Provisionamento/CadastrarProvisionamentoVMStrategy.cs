using System;
using System.Collections.Generic;
using System.Text;

namespace VandaModaIntimaWpf.ViewModel.Provisionamento
{
    public class CadastrarProvisionamentoVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Inserir Provisionamentos";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Provisionamentos Foram Inseridos Com Sucesso";
        }

        public string MessageBoxCaption()
        {
            return "Cadastro de Provisionamentos";
        }
    }
}
