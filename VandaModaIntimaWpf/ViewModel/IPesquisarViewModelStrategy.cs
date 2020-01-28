using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.ViewModel
{
    public interface IPesquisarViewModelStrategy<E> where E : class, IModel
    {
        void AbrirCadastrar(object parameter);
        bool? AbrirEditar(E entidade);
        void AbrirAjuda(object parameter);
        void ExportarSQLUpdate(object parameter, IDAO<E> dao);
        void ExportarSQLInsert(object parameter, IDAO<E> dao);
        void RestauraEntidade(E original, E backup);
        string MensagemApagarEntidadeCerteza(E e);
        string MensagemEntidadeDeletada(E e);
        string MensagemEntidadesDeletadas();
        string MensagemEntidadeNaoDeletada();
        string MensagemEntidadesNaoDeletadas();
        string MensagemApagarMarcados();
        string TelaApagarCaption();
    }
}
