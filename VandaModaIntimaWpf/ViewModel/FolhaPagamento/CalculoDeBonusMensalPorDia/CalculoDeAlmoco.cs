using NHibernate;
using System;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public class CalculoDeAlmoco : ICalculoDeBonus
    {
        public void AbrirAdicionarBonus(ISession session, IMessageBoxService messageBox, bool isUpdate, DateTime DataEscolhida, double Total, double valorDiario, int numDias, DateTime primeiroDia, DateTime ultimoDia)
        {
            SalvarBonusPorMesVM adicionarBonusVM = new SalvarBonusPorMesVM(session, messageBox, isUpdate, DataEscolhida, Total, valorDiario, numDias, primeiroDia, ultimoDia, new SalvarAlmoco());
            SalvarBonusDeFuncionario adicionarBonus = new SalvarBonusDeFuncionario()
            {
                DataContext = adicionarBonusVM
            };
            adicionarBonus.ShowDialog();
        }

        public string MenuItemHeader1()
        {
            return "Adicionar Vale Alimentação À Folha";
        }

        public double ValorDiario()
        {
            return Config.Instancia.ValorDiarioValeAlimentacao;
        }

        public string WindowCaption()
        {
            return "Cálculo de Vale Alimentação";
        }
    }
}
