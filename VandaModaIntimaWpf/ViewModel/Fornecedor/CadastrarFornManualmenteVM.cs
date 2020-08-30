﻿using NHibernate;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornManualmenteVM : ACadastrarViewModel<FornecedorModel>
    {
        public CadastrarFornManualmenteVM(ISession session, IMessageBoxService messageBoxService) : base(session, messageBoxService)
        {
            cadastrarViewModelStrategy = new CadastrarFornMsgVMStrategy();
            daoEntidade = new DAOFornecedor(_session);
            Entidade = new FornecedorModel();
            Entidade.PropertyChanged += Entidade_PropertyChanged;
        }

        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public override async void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cnpj":
                    var result = await daoEntidade.ListarPorId(Entidade.Cnpj);

                    if (result != null)
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Visible;
                        IsEnabled = false;
                    }
                    else
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Collapsed;
                        IsEnabled = true;
                    }

                    break;
            }
        }
        public override void ResetaPropriedades()
        {
            Entidade = new FornecedorModel();
            Entidade.Cnpj = Entidade.Nome = Entidade.Fantasia = Entidade.Email = string.Empty;
        }
        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Entidade.Cnpj)
                || string.IsNullOrEmpty(Entidade.Nome)
                || Entidade.Cnpj.Length != 14)
            {
                return false;
            }

            return true;
        }

        protected override void ExecutarAntesCriarDocumento()
        {

        }
    }
}
