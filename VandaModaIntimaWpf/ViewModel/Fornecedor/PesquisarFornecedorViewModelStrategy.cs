using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.View.SQL;
using VandaModaIntimaWpf.ViewModel.SQL;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class PesquisarFornecedorViewModelStrategy : IPesquisarViewModelStrategy<FornecedorModel>
    {
        public void AbrirAjuda(object parameter)
        {
            throw new NotImplementedException();
        }
        public bool? AbrirCadastrar(object parameter, ISession session)
        {
            CadastrarFornecedorManualmenteViewModel cadastrarFornecedorManualmenteViewModel = new CadastrarFornecedorManualmenteViewModel(session);
            CadastrarFornecedorManualmente cadastrar = new CadastrarFornecedorManualmente() { DataContext = cadastrarFornecedorManualmenteViewModel };
            return cadastrar.ShowDialog();
        }
        public bool? AbrirEditar(FornecedorModel clone, ISession session)
        {
            EditarFornecedorViewModel viewModel = new EditarFornecedorViewModel(session)
            {
                Fornecedor = clone
            };

            EditarFornecedor editarFornecedor = new EditarFornecedor()
            {
                DataContext = viewModel
            };

            return editarFornecedor.ShowDialog();
        }

        public void AbrirExportarSQL(object parameter, IList<FornecedorModel> entidades)
        {
            ExportarSQL importarExportarSQL = new ExportarSQL(new ExportarSQLFornecedor());
            importarExportarSQL.ShowDialog();
        }

        public string MensagemApagarEntidadeCerteza(FornecedorModel e)
        {
            return string.Format(GetResource.GetString("certeza_deletar_fornecedor"), e.Nome);
        }

        public string MensagemApagarMarcados()
        {
            return GetResource.GetString("deseja_apagar_fornecedor_marcados");
        }

        public string MensagemEntidadeDeletada(FornecedorModel e)
        {
            return string.Format(GetResource.GetString("fornecedor_deletado_com_sucesso"), e.Nome);
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return GetResource.GetString("fornecedor_nao_deletado");
        }

        public string MensagemEntidadesDeletadas()
        {
            return GetResource.GetString("fornecedores_deletados_com_sucesso");
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return GetResource.GetString("fornecedores_nao_deletados");
        }

        public void RestauraEntidade(FornecedorModel original, FornecedorModel backup)
        {
            original.Cnpj = backup.Cnpj;
            original.Nome = backup.Nome;
            original.Fantasia = backup.Fantasia;
            original.Email = backup.Email;
            original.Telefone = backup.Telefone;
            original.Produtos = backup.Produtos;
        }

        public string TelaApagarCaption()
        {
            return GetResource.GetString("apagar_fornecedores");
        }

        public void AbrirCadastrarOnline(ISession session)
        {
            CadastrarFornecedorOnlineViewModel viewModel = new CadastrarFornecedorOnlineViewModel(session);
            CadastrarFornecedorOnline cadastrarFornecedorOnline = new CadastrarFornecedorOnline
            {
                DataContext = viewModel
            };
            cadastrarFornecedorOnline.ShowDialog();
        }
    }
}
