using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.View.Marca;
using VandaModaIntimaWpf.ViewModel.Marca;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    class AbrePelaTelaPesqMarcaService : IAbrePelaTelaPesquisaService<Model.Marca>
    {
        public void AbrirAjuda()
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(ISession session)
        {
            CadastrarMarcaVM cadastrarMarcaViewModel = new CadastrarMarcaVM(session, new MessageBoxService());
            CadastrarMarca cadastrarMarca = new CadastrarMarca()
            {
                DataContext = cadastrarMarcaViewModel
            };
            return cadastrarMarca.ShowDialog();
        }

        public bool? AbrirEditar(Model.Marca clone, ISession session)
        {
            return false;
        }

        public void AbrirExportarSQL(IList<Model.Marca> entidades)
        {
            throw new NotImplementedException();
        }

        public void AbrirImprimir(IList<Model.Marca> lista)
        {
            throw new NotImplementedException();
        }
    }
}
