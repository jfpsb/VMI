﻿using NHibernate;
using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class CadastrarMarcaViewModel : ACadastrarViewModel<MarcaModel>
    {
        public CadastrarMarcaViewModel(ISession session) : base(session)
        {
            cadastrarViewModelStrategy = new CadastrarMarcaViewModelStrategy();
            daoEntidade = new DAOMarca(_session);
            Entidade = new MarcaModel();
        }

        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public override async void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Nome":
                    MarcaModel marca = (MarcaModel)await daoEntidade.ListarPorId(Entidade.Nome);

                    if (marca != null)
                    {
                        VisibilidadeAvisoItemJaExiste = System.Windows.Visibility.Visible;
                        IsEnabled = false;
                    }
                    else
                    {
                        VisibilidadeAvisoItemJaExiste = System.Windows.Visibility.Collapsed;
                        IsEnabled = true;
                    }

                    break;
            }
        }

        public override void ResetaPropriedades()
        {
            Entidade = new MarcaModel();
            Entidade.Nome = string.Empty;
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Entidade.Nome?.Trim()))
                return false;

            return true;
        }

        protected override void ExecutarAntesCriarDocumento()
        {

        }
    }
}
