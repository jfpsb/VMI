using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.View.Loja;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    class PesquisarLojaViewModelStrategy : IPesquisarViewModelStrategy<LojaModel>
    {
        public void AbrirAjuda(object parameter)
        {
            AjudaLoja ajudaLoja = new AjudaLoja();
            ajudaLoja.ShowDialog();
        }

        public void AbrirCadastrar(object parameter)
        {
            CadastrarLoja cadastrar = new CadastrarLoja();
            cadastrar.ShowDialog();
        }

        public bool? AbrirEditar(LojaModel entidade)
        {
            EditarLoja editar = new EditarLoja(entidade.Cnpj);
            return editar.ShowDialog();
        }

        public void AbrirExportarSQL(object parameter, IList<LojaModel> entidades)
        {
            throw new NotImplementedException();
        }

        public string MensagemApagarEntidadeCerteza(LojaModel e)
        {
            return $"Tem Certeza Que Deseja Apagar a Loja {e.Nome}?";
        }

        public string MensagemApagarMarcados()
        {
            return "Desejar Apagar as Lojas Marcadas?";
        }

        public string MensagemEntidadeDeletada(LojaModel e)
        {
            return $"Loja {e.Nome} Foi Deletada Com Sucesso";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "Loja Não Foi Deletada";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "Lojas Apagadas Com Sucesso";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "Erro ao Apagar Lojas";
        }

        public void RestauraEntidade(LojaModel original, LojaModel backup)
        {
            original.Cnpj = backup.Cnpj;
            original.Nome = backup.Nome;
            original.Telefone = backup.Telefone;
            original.Endereco = backup.Endereco;
            original.InscricaoEstadual = backup.InscricaoEstadual;
            original.Matriz = backup.Matriz;
        }

        public string TelaApagarCaption()
        {
            return "Apagar Loja(s)";
        }
    }
}
