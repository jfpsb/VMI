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
            CadastrarFuncVM cadastrarFuncionarioViewModel = new CadastrarFuncVM(session, new MessageBoxService());
            CadastrarFuncionario cadastrarFuncionario = new CadastrarFuncionario() { DataContext = cadastrarFuncionarioViewModel };
            return cadastrarFuncionario.ShowDialog();
        }

        public bool? AbrirEditar(Model.Funcionario clone, ISession session)
        {
            EditarFuncVM editarFuncionarioViewModel = new EditarFuncVM(session, new MessageBoxService());
            editarFuncionarioViewModel.Entidade = clone;

            EditarFuncionario editarFuncionario = new EditarFuncionario()
            {
                DataContext = editarFuncionarioViewModel
            };

            return editarFuncionario.ShowDialog();
        }

        public void AbrirExportarSQL(IList<Model.Funcionario> entidades)
        {
            throw new NotImplementedException();
        }

        public void AbrirImprimir(IList<Model.Funcionario> lista)
        {
            throw new NotImplementedException();
        }
    }
}
