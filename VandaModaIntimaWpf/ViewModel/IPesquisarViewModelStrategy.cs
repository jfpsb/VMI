using NHibernate;
using System.Collections.Generic;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel
{
    public interface IPesquisarViewModelStrategy<E> where E : class, IModel
    {
        bool? AbrirCadastrar(object parameter, ISession session);
        bool? AbrirEditar(E entidade, ISession session);
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
        string MensagemDocumentoDeletado();
        string MensagemDocumentoNaoDeletado();
    }
}
