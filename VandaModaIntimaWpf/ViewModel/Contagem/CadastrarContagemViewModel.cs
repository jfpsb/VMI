﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using ContagemModel = VandaModaIntimaWpf.Model.Contagem;
using LojaModel = VandaModaIntimaWpf.Model.Loja;
using TipoContagemModel = VandaModaIntimaWpf.Model.TipoContagem;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    class CadastrarContagemViewModel : ACadastrarViewModel
    {
        protected DAOContagem _daoContagem;
        private DAOTipoContagem _daoTipoContagem;
        private DAOLoja _daoLoja;
        private ContagemModel _contagem;

        public ObservableCollection<LojaModel> Lojas { get; set; }
        public ObservableCollection<TipoContagemModel> TiposContagem { get; set; }

        public CadastrarContagemViewModel() : base("Contagem")
        {
            Contagem = new ContagemModel();

            _daoLoja = new DAOLoja(_session);
            _daoTipoContagem = new DAOTipoContagem(_session);
            _daoContagem = new DAOContagem(_session);

            GetLojas();
            GetTiposContagem();
        }
        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ResetaPropriedades()
        {
            Contagem = new ContagemModel();
            Contagem.TipoContagem = TiposContagem[0];
            Contagem.Loja = Lojas[0];
        }

        public override async void Salvar(object parameter)
        {
            Contagem.Data = DateTime.Now;
            Contagem.Finalizada = false;

            var result = await _daoContagem.Inserir(Contagem);

            if (result)
            {
                ResetaPropriedades();
                await SetStatusBarSucesso("Contagem Cadastrada Com Sucesso");
                return;
            }

            SetStatusBarErro("Erro ao Cadastrar Contagem");
        }

        public override bool ValidaModel(object parameter)
        {
            return true;
        }

        private async void GetLojas()
        {
            Lojas = new ObservableCollection<LojaModel>(await _daoLoja.Listar<LojaModel>());
        }

        private async void GetTiposContagem()
        {
            TiposContagem = new ObservableCollection<TipoContagemModel>(await _daoTipoContagem.Listar<TipoContagemModel>());
        }

        public ContagemModel Contagem
        {
            get
            {
                return _contagem;
            }

            set
            {
                _contagem = value;
                OnPropertyChanged("Contagem");
            }
        }
    }
}
