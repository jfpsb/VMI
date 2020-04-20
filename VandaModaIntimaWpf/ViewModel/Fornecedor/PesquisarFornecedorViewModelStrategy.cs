using NHibernate;
using System;
using System.Collections.Generic;
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
            return $"Tem Certeza Que Deseja Apagar o Fornecedor {e.Nome}?";
        }

        public string MensagemApagarMarcados()
        {
            return "Desejar Apagar os Fornecedores Marcados?";
        }

        public string MensagemEntidadeDeletada(FornecedorModel e)
        {
            return $"Fornecedor {e.Nome} Foi Deletado Com Sucesso";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "Fornecedor Não Foi Deletado";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "Fornecedores Apagados Com Sucesso";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "Erro ao Apagar Fornecedores";
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
            return "Apagar Fornecedor(es)";
        }

        public void AbrirCadastrarOnline(ISession session)
        {
            CadastrarFornecedorOnlineViewModel viewModel = new CadastrarFornecedorOnlineViewModel(session);
            CadastrarFornecedorOnline cadastrarFornecedorOnline = new CadastrarFornecedorOnline();
            cadastrarFornecedorOnline.DataContext = viewModel;
            cadastrarFornecedorOnline.ShowDialog();
        }
    }
}
