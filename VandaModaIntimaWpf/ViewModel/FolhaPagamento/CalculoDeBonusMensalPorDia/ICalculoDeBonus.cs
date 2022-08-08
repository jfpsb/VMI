using NHibernate;
using System;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public interface ICalculoDeBonus
    {
        string WindowCaption();
        string MenuItemHeader1();
        double ValorDiario();
        void AtribuirNovoValorDiario(double valorDiario);
        void AbrirAdicionarBonus(ISession session, bool isUpdate, DateTime DataEscolhida, double Total, double valorDiario, int numDias, DateTime primeiroDia, DateTime ultimoDia);
    }
}
