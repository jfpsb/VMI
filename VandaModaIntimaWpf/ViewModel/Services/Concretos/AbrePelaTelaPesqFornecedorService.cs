using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.View.Representante;
using VandaModaIntimaWpf.View.SQL;
using VandaModaIntimaWpf.ViewModel.Fornecedor;
using VandaModaIntimaWpf.ViewModel.Representante;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using VandaModaIntimaWpf.ViewModel.SQL;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    public class AbrePelaTelaPesqFornecedorService : IAbrePelaTelaPesquisaService<Model.Fornecedor>
    {
        public void AbrirAjuda()
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(ISession session)
        {
            CadastrarFornecedorManualmenteVM cadastrarFornecedorManualmenteViewModel = new CadastrarFornecedorManualmenteVM(session, new MessageBoxService(), false);
            CadastrarFornecedorManualmente cadastrar = new CadastrarFornecedorManualmente() { DataContext = cadastrarFornecedorManualmenteViewModel };
            return cadastrar.ShowDialog();
        }

        public bool? AbrirEditar(Model.Fornecedor clone, ISession session)
        {
            EditarFornecedorVM viewModel = new EditarFornecedorVM(session, clone, new MessageBoxService());
            //viewModel.Entidade = clone;

            EditarFornecedor editarFornecedor = new EditarFornecedor() { DataContext = viewModel };

            return editarFornecedor.ShowDialog();
        }

        public void AbrirExportarSQL(IList<Model.Fornecedor> entidades, ISession session)
        {
            ExportarSQLFornecedor viewModel = new ExportarSQLFornecedor(entidades, session);
            ExportarSQL importarExportarSQL = new ExportarSQL();
            importarExportarSQL.DataContext = viewModel;
            importarExportarSQL.ShowDialog();
        }

        public void AbrirImprimir(IList<Model.Fornecedor> lista)
        {
            throw new NotImplementedException();
        }

        public void AbrirCadastrarOnline(ISession session)
        {
            CadastrarFornecedorOnlineVM viewModel = new CadastrarFornecedorOnlineVM(session, new MessageBoxService(), false);
            CadastrarFornecedorOnline cadastrarFornecedorOnline = new CadastrarFornecedorOnline
            {
                DataContext = viewModel
            };
            cadastrarFornecedorOnline.ShowDialog();
        }

        public void AbrirPesquisarRepresentante()
        {
            PesquisarRepresentanteVM viewModel = new PesquisarRepresentanteVM(new MessageBoxService(), new AbrePelaTelaPesquisaRepresentante());
            PesquisarRepresentante view = new PesquisarRepresentante
            {
                DataContext = viewModel
            };
            view.ShowDialog();
        }
    }
}
