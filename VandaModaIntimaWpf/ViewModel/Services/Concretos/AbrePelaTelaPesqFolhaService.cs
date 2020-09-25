using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    public class AbrePelaTelaPesqFolhaService : IAbrePelaTelaPesquisaService<Model.FolhaPagamento>
    {
        public void AbrirAjuda()
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(ISession session)
        {
            throw new NotImplementedException();
        }

        public bool? AbrirEditar(Model.FolhaPagamento clone, ISession session)
        {
            throw new NotImplementedException();
        }

        public void AbrirExportarSQL(IList<Model.FolhaPagamento> entidades)
        {
            throw new NotImplementedException();
        }

        public void AbrirImprimir(IList<Model.FolhaPagamento> lista)
        {
            throw new NotImplementedException();
        }
    }
}
