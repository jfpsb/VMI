namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public interface ISalvarBonus
    {
        string DescricaoBonus(int numDias);
        string MensagemCaption();
        string MensagemInseridoSucesso();
        string MensagemInseridoErro();
        string RecebeRegularmenteHeader();
    }
}
