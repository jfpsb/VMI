using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.View.Funcionario;
using VandaModaIntimaWpf.ViewModel.Funcionario;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    public class AbrePelaTelaPesqFuncService : IAbrePelaTelaPesquisaService<Model.Funcionario>
    {
        public void AbrirAjuda()
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(ISession session)
        {
            CadastrarFuncionarioVM cadastrarFuncionarioViewModel = new CadastrarFuncionarioVM(session, new MessageBoxService(), false);
            SalvarFuncionario cadastrarFuncionario = new SalvarFuncionario() { DataContext = cadastrarFuncionarioViewModel };
            return cadastrarFuncionario.ShowDialog();
        }

        public bool? AbrirEditar(Model.Funcionario clone, ISession session)
        {
            EditarFuncionarioVM editarFuncionarioViewModel = new EditarFuncionarioVM(session, new MessageBoxService());
            editarFuncionarioViewModel.Entidade = clone;

            SalvarFuncionario editarFuncionario = new SalvarFuncionario()
            {
                DataContext = editarFuncionarioViewModel
            };

            return editarFuncionario.ShowDialog();
        }

        public void AbrirExportarSQL(IList<Model.Funcionario> entidades, ISession session)
        {
            throw new NotImplementedException();
        }

        public void AbrirImprimir(IList<Model.Funcionario> lista)
        {
            throw new NotImplementedException();
        }
    }
}
