﻿using NHibernate;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FuncionarioModel = VandaModaIntimaWpf.Model.Funcionario;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class CadastrarFuncVM : ACadastrarViewModel<FuncionarioModel>
    {
        private DAOLoja daoLoja;
        public ObservableCollection<LojaModel> Lojas { get; set; }

        public CadastrarFuncVM(ISession session, IMessageBoxService messageBoxService) : base(session, messageBoxService)
        {
            cadastrarViewModelStrategy = new CadastrarFuncMsgVMStrategy();
            daoEntidade = new DAOFuncionario(_session);
            daoLoja = new DAOLoja(_session);
            Entidade = new FuncionarioModel();
            Entidade.PropertyChanged += Entidade_PropertyChanged;
            GetLojas();
            Entidade.Loja = Lojas[0];
        }
        public override async void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cpf":
                    var result = await daoEntidade.ListarPorId(Entidade.Cpf);

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
        private async void GetLojas()
        {
            Lojas = new ObservableCollection<LojaModel>(await daoLoja.ListarExcetoDeposito());
        }
        public override void ResetaPropriedades()
        {
            Entidade = new FuncionarioModel();
            Entidade.Loja = Lojas[0];
        }
        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Entidade.Cpf) || string.IsNullOrEmpty(Entidade.Nome) || Entidade.Salario <= 0.0)
                return false;

            return true;
        }

        protected override void ExecutarAntesCriarDocumento()
        {
            if (Entidade.Loja.Cnpj == null)
                Entidade.Loja = null;
        }

        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}