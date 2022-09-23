using System;
using System.Globalization;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public class SalvarPassagem : ISalvarBonus
    {
        public string DescricaoBonus(int numDias, double valorDiario, DateTime primeiroDia, DateTime ultimoDia)
        {
            return $"PASSAGEM DE ÔNIBUS DE {primeiroDia:dd/MM} A {ultimoDia:dd/MM} ({valorDiario.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"))} x {numDias} DIAS)";
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

        public string RegularmentePropriedade()
        {
            return "RecebePassagem";
        }

        public string RecebeRegularmenteHeader()
        {
            return "Recebe Passagem Regularmente";
        }
    }
}
