﻿using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.View.Loja;
using VandaModaIntimaWpf.ViewModel.Loja;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    public class AbrePelaTelaPesqLojaService : IAbrePelaTelaPesquisaService<Model.Loja>
    {
        public void AbrirAjuda()
        {
            AjudaLoja ajudaLoja = new AjudaLoja();
            ajudaLoja.ShowDialog();
        }

        public bool? AbrirCadastrar(ISession session)
        {
            CadastrarLojaVM cadastrarLojaViewModel = new CadastrarLojaVM(session, new MessageBoxService(), false);
            SalvarLoja cadastrar = new SalvarLoja()
            {
                DataContext = cadastrarLojaViewModel
            };
            return cadastrar.ShowDialog();
        }

        public bool? AbrirEditar(Model.Loja clone, ISession session)
        {
            EditarLojaVM editarLojaViewModel = new EditarLojaVM(session, new MessageBoxService());
            editarLojaViewModel.Entidade = clone;

            SalvarLoja editar = new SalvarLoja()
            {
                DataContext = editarLojaViewModel
            };

            return editar.ShowDialog();
        }

        public void AbrirExportarSQL(IList<Model.Loja> entidades, ISession session)
        {
            throw new NotImplementedException();
        }

        public void AbrirImprimir(IList<Model.Loja> lista)
        {
            throw new NotImplementedException();
        }
    }
}
