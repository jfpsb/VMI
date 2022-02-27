using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.View.EntradaDeMercadoria;
using VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    class AbrePelaTelaPesquisaEntradaMercadoriaService : IAbrePelaTelaPesquisaService<Model.EntradaDeMercadoria>
    {
        public void AbrirAjuda()
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(ISession session)
        {
            CadastrarEntradaDeMercadoriaVM viewModel = new CadastrarEntradaDeMercadoriaVM(session, new MessageBoxService(), false);
            SalvarEntradaDeMercadoria view = new SalvarEntradaDeMercadoria() { DataContext = viewModel };
            return view.ShowDialog();
        }

        public bool? AbrirEditar(Model.EntradaDeMercadoria clone, ISession session)
        {
            throw new NotImplementedException();
        }

        public void AbrirExportarSQL(IList<Model.EntradaDeMercadoria> entidades, ISession session)
        {
            throw new NotImplementedException();
        }

        public void AbrirImprimir(IList<Model.EntradaDeMercadoria> lista)
        {
            throw new NotImplementedException();
        }
    }
}
