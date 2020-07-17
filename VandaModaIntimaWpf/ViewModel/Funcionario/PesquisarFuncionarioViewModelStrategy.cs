using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.View.Funcionario;
using FuncionarioModel = VandaModaIntimaWpf.Model.Funcionario;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    class PesquisarFuncionarioViewModelStrategy : IPesquisarViewModelStrategy<FuncionarioModel>
    {
        //TODO: Colocar strings no resources
        public void AbrirAjuda(object parameter)
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(object parameter, ISession session)
        {
            CadastrarFuncionarioViewModel cadastrarFuncionarioViewModel = new CadastrarFuncionarioViewModel(session);
            CadastrarFuncionario cadastrarFuncionario = new CadastrarFuncionario() { DataContext = cadastrarFuncionarioViewModel };
            return cadastrarFuncionario.ShowDialog();
        }

        public bool? AbrirEditar(FuncionarioModel entidade, ISession session)
        {
            EditarFuncionarioViewModel editarFuncionarioViewModel = new EditarFuncionarioViewModel(session)
            {
                Funcionario = entidade
            };

            EditarFuncionario editarFuncionario = new EditarFuncionario()
            {
                DataContext = editarFuncionarioViewModel
            };

            return editarFuncionario.ShowDialog();
        }

        public void AbrirExportarSQL(object parameter, IList<FuncionarioModel> entidades)
        {
            throw new NotImplementedException();
        }

        public string MensagemApagarEntidadeCerteza(FuncionarioModel e)
        {
            return "Tem Certeza que Deseja Deletar o Funcionário (a) " + e.Nome + "?";
        }

        public string MensagemApagarMarcados()
        {
            return "Tem Certeza Que Deseja Apagar os Funcionários Marcados?";
        }

        public string MensagemEntidadeDeletada(FuncionarioModel e)
        {
            return "Funcionário " + e.Nome + " Deletado (a) Com Sucesso";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "Funcionário Não Foi Deletado";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "Funcionários Marcados Foram Deletados Com Sucesso";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "Funcionários Marcados Não Foram Deletados Com Sucesso";
        }

        public void RestauraEntidade(FuncionarioModel original, FuncionarioModel backup)
        {
            throw new NotImplementedException();
        }

        public string TelaApagarCaption()
        {
            return "Apagar Funcionários";
        }
    }
}
