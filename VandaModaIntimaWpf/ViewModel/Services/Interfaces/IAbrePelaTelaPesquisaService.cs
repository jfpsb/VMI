using NHibernate;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.ViewModel.Services.Interfaces
{
    public interface IAbrePelaTelaPesquisaService<E>
    {
        void AbrirAjuda();
        bool? AbrirCadastrar(ISession session);
        bool? AbrirEditar(E clone, ISession session);
        void AbrirExportarSQL(IList<E> entidades, ISession session);
        void AbrirImprimir(IList<E> lista);
    }
}
