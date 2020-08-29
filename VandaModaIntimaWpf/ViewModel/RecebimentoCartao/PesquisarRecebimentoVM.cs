﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using LojaModel = VandaModaIntimaWpf.Model.Loja;
using RecebimentoCartaoModel = VandaModaIntimaWpf.Model.RecebimentoCartao;

namespace VandaModaIntimaWpf.ViewModel.RecebimentoCartao
{
    public class PesquisarRecebimentoVM : APesquisarViewModel<RecebimentoCartaoModel>
    {
        private DAOLoja daoLoja;
        private LojaModel matriz;
        private DateTime dataEscolhida = DateTime.Now;
        private int matrizComboBoxIndex;

        public ObservableCollection<LojaModel> Matrizes { get; set; }
        public ICommand AbrirCadastrarOperadoraComando { get; set; }
        public PesquisarRecebimentoVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<RecebimentoCartaoModel> abrePelaTelaPesquisaService)
            : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            daoEntidade = new DAORecebimentoCartao(_session);
            daoLoja = new DAOLoja(_session);
            pesquisarViewModelStrategy = new PesquisarRecebMsgVMStrategy();
            GetMatrizes();
            MatrizComboBoxIndex = 0;

            DataEscolhida = DateTime.Now;
        }
        public override async void PesquisaItens(string termo)
        {
            DAORecebimentoCartao daoRecebimento = (DAORecebimentoCartao)daoEntidade;
            if (MatrizComboBoxIndex != 0)
            {
                Entidades = new ObservableCollection<EntidadeComCampo<RecebimentoCartaoModel>>(EntidadeComCampo<RecebimentoCartaoModel>.ConverterIList(await daoRecebimento.ListarPorMesAnoLojaGroupByLoja(DataEscolhida.Month, DataEscolhida.Year, Matriz)));
            }
            else
            {
                Entidades = new ObservableCollection<EntidadeComCampo<RecebimentoCartaoModel>>(EntidadeComCampo<RecebimentoCartaoModel>.ConverterIList(await daoRecebimento.ListarPorMesAnoGroupByLoja(DataEscolhida.Month, DataEscolhida.Year)));
            }
        }
        public async void GetMatrizes()
        {
            Matrizes = new ObservableCollection<LojaModel>(await daoLoja.ListarMatrizes());
            Matrizes.Insert(0, new LojaModel(GetResource.GetString("matriz_nao_selecionada")));
        }
        public override bool Editavel(object parameter)
        {
            return false;
        }
        public LojaModel Matriz
        {
            get { return matriz; }
            set
            {
                matriz = value;
                OnPropertyChanged("Matriz");
                OnPropertyChanged("TermoPesquisa");
            }
        }
        public int MatrizComboBoxIndex
        {
            get { return matrizComboBoxIndex; }
            set
            {
                matrizComboBoxIndex = value;
                OnPropertyChanged("MatrizComboBoxIndex");
            }
        }
        public DateTime DataEscolhida
        {
            get { return dataEscolhida; }
            set
            {
                dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
                OnPropertyChanged("TermoPesquisa");
            }
        }
    }
}