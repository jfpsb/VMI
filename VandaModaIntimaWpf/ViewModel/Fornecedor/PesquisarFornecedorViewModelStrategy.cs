using System;
using VandaModaIntimaWpf.View.Fornecedor;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class PesquisarFornecedorViewModelStrategy : IPesquisarViewModelStrategy<FornecedorModel>
    {
        public void AbrirAjuda(object parameter)
        {
            throw new NotImplementedException();
        }

        public void AbrirCadastrar(object parameter)
        {
            CadastrarFornecedorManualmente cadastrar = new CadastrarFornecedorManualmente();
            cadastrar.ShowDialog();
        }

        public bool? AbrirEditar(FornecedorModel entidade)
        {
            EditarFornecedor editar = new EditarFornecedor(entidade.Cnpj);
            return editar.ShowDialog();
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
            return "Apagar Fornecedor";
        }
    }
}
