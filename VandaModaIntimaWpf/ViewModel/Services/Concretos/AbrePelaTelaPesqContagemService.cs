using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.View.Contagem;
using VandaModaIntimaWpf.ViewModel.Contagem;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    public class AbrePelaTelaPesqContagemService : IAbrePelaTelaPesquisaService<Model.Contagem>
    {
        public void AbrirAjuda()
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(ISession session)
        {
            CadastrarContagemVM cadastrarContagemViewModel = new CadastrarContagemVM(session, new MessageBoxService());
            CadastrarContagem cadastrarContagem = new CadastrarContagem()
            {
                DataContext = cadastrarContagemViewModel
            };

            return cadastrarContagem.ShowDialog();
        }

        public bool? AbrirEditar(Model.Contagem clone, ISession session)
        {
            EditarContagemVM editarContagemViewModel = new EditarContagemVM(session, new MessageBoxService());
            editarContagemViewModel.Entidade = clone;

            EditarContagem editarContagem = new EditarContagem()
            {
                DataContext = editarContagemViewModel
            };

            return editarContagem.ShowDialog();
        }

        public void AbrirExportarSQL(IList<Model.Contagem> entidades)
        {
            throw new NotImplementedException();
        }

        public void AbrirImprimir(IList<Model.Contagem> lista)
        {
            throw new NotImplementedException();
        }
    }
}
