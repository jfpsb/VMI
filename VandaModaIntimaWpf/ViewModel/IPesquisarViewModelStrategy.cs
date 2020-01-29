using System.Collections.Generic;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel
{
    public interface IPesquisarViewModelStrategy<E> where E : class, IModel
    {
        void AbrirCadastrar(object parameter);
        bool? AbrirEditar(E entidade);
        void AbrirAjuda(object parameter);
        void AbrirExportarSQL(object parameter, IList<E> entidades);
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
