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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public string MensagemEntidadeDeletada(FuncionarioModel e)
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadeNaoDeletada()
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadesDeletadas()
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            throw new NotImplementedException();
        }

        public void RestauraEntidade(FuncionarioModel original, FuncionarioModel backup)
        {
            throw new NotImplementedException();
        }

        public string TelaApagarCaption()
        {
            throw new NotImplementedException();
        }
    }
}
