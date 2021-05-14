using System;
using System.Globalization;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public class SalvarAlmoco : ISalvarBonus
    {
        public string DescricaoBonus(int numDias, double valorDiario, DateTime primeiroDia, DateTime ultimoDia)
        {
            return $"ALIMENTAÇÃO DE {primeiroDia.ToString("dd/MM")} A {ultimoDia.ToString("dd/MM")} ({valorDiario.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"))} x {numDias} DIAS)";
        }

        public string MensagemCaption()
        {
            return "Adicionando Vale Alimentação Nas Folhas De Pagamento";
        }

        public string MensagemInseridoErro()
        {
            return "Erro Ao Inserir O Valor de Vale Alimentação!";
        }

        public string MensagemInseridoSucesso()
        {
            return "O Valor de Vale de Alimentação Foi Inserido Com Sucesso Como Bônus Nos Funcionários Marcados!";
        }

        public string RecebeRegularmenteHeader()
        {
            return "Recebe Almoço Regularmente";
        }
    }
}
