using NHibernate;
using VandaModaIntimaWpf.View.Grade;
using VandaModaIntimaWpf.View.TipoGrade;
using VandaModaIntimaWpf.ViewModel.Grade;
using VandaModaIntimaWpf.ViewModel.TipoGrade;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    public class AbrePelaTelaCadastroDeProduto
    {
        public void AbrirTelaCadastrarTipoGrade(ISession session)
        {
            CadastrarTipoGradeVM viewModel = new CadastrarTipoGradeVM(session, new MessageBoxService(), false);
            CadastrarTipoGrade cadastrarTipoGrade = new CadastrarTipoGrade()
            {
                DataContext = viewModel
            };
            cadastrarTipoGrade.ShowDialog();
        }

        public void AbrirTelaCadastrarGrade(ISession session)
        {
            CadastrarGradeVM viewModel = new CadastrarGradeVM(session, new MessageBoxService(), false);
            CadastrarGrade cadastrarGrade = new CadastrarGrade()
            {
                DataContext = viewModel
            };
            cadastrarGrade.ShowDialog();
        }
    }
}
