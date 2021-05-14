using System;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public interface ISalvarBonus
    {
        string DescricaoBonus(int numDias, double valorDiario, DateTime primeiroDia, DateTime ultimoDia);
        string MensagemCaption();
        string MensagemInseridoSucesso();
        string MensagemInseridoErro();
        string RecebeRegularmenteHeader();
    }
}
