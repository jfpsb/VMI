using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.View.Despesa;
using VandaModaIntimaWpf.ViewModel.Despesa;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    public class AbrePelaTelaPesquisaDespesa : IAbrePelaTelaPesquisaService<Model.Despesa>
    {
        public void AbrirAjuda()
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(ISession session)
        {
            CadastrarDespesaVM viewModel = new CadastrarDespesaVM(session, new MessageBoxService(), false);
            SalvarDespesa view = new SalvarDespesa
            {
                DataContext = viewModel
            };
            return view.ShowDialog();
        }

        public bool? AbrirEditar(Model.Despesa clone, ISession session)
        {
            throw new NotImplementedException();
        }

        public void AbrirExportarSQL(IList<Model.Despesa> entidades, ISession session)
        {
            throw new NotImplementedException();
        }

        public void AbrirImprimir(IList<Model.Despesa> lista)
        {
            throw new NotImplementedException();
        }
    }
}
