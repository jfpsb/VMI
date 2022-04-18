using NHibernate;
using System;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public interface ICalculoDeBonus
    {
        string WindowCaption();
        string MenuItemHeader1();
        double ValorDiario();
        void AbrirAdicionarBonus(ISession session, IMessageBoxService messageBox, bool isUpdate, DateTime DataEscolhida, double Total, double valorDiario, int numDias, DateTime primeiroDia, DateTime ultimoDia);
    }
}
