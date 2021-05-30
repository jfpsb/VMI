using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.View.Representante;
using VandaModaIntimaWpf.ViewModel.Representante;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    public class AbrePelaTelaPesquisaRepresentante : IAbrePelaTelaPesquisaService<Model.Representante>
    {
        public void AbrirAjuda()
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(ISession session)
        {
            CadastrarRepresentanteVM viewModel = new CadastrarRepresentanteVM(session, new MessageBoxService(), false);
            SalvarRepresentante view = new SalvarRepresentante
            {
                DataContext = viewModel
            };
            return view.ShowDialog();
        }

        public bool? AbrirEditar(Model.Representante clone, ISession session)
        {
            EditarRepresentanteVM viewModel = new EditarRepresentanteVM(session, clone, new MessageBoxService());
            SalvarRepresentante view = new SalvarRepresentante
            {
                DataContext = viewModel
            };
            return view.ShowDialog();
        }

        public void AbrirExportarSQL(IList<Model.Representante> entidades, ISession session)
        {
            throw new NotImplementedException();
        }

        public void AbrirImprimir(IList<Model.Representante> lista)
        {
            throw new NotImplementedException();
        }
    }
}
