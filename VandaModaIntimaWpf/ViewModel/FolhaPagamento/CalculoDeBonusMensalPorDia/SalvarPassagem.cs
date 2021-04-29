﻿using System;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public class SalvarPassagem : ISalvarBonus
    {
        public string DescricaoBonus(int numDias, DateTime primeiroDia, DateTime ultimoDia)
        {
            return $"PASSAGEM DE {primeiroDia.ToString("dd/MM")} A {ultimoDia.ToString("dd/MM")} ({numDias} DIAS)";
        }

        public string MensagemCaption()
        {
            return "Adicionando Passagem Nas Folhas De Pagamento";
        }

        public string MensagemInseridoErro()
        {
            return "Erro Ao Inserir O Valor de Passagens!";
        }

        public string MensagemInseridoSucesso()
        {
            return "O Valor de Passagem Foi Inserido Com Sucesso Como Bônus Nos Funcionários Marcados!";
        }

        public string RecebeRegularmenteHeader()
        {
            return "Recebe Passagem Regularmente";
        }
    }
}
