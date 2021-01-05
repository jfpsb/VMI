﻿using NHibernate;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class EditarFuncionarioVM : CadastrarFuncionarioVM
    {
        public EditarFuncionarioVM(ISession session, IMessageBoxService messageBoxService) : base(session, messageBoxService)
        {
            viewModelStrategy = new EditarFuncionarioVMStrategy();
            issoEUmUpdate = true;
        }
    }
}
