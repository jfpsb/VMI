using System;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public interface ICalculoDeBonus
    {
        string WindowCaption();
        string MenuItemHeader1();
        double ValorDiario();
        void AbrirAdicionarBonus(DateTime DataEscolhida, double Total, int numDias, DateTime primeiroDia, DateTime ultimoDia, IMessageBoxService messageBoxService);
    }
}
