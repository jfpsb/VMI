﻿using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria
{
    public class PesquisarEntradasPorFornecedorVM : APesquisarViewModel<Model.EntradaMercadoriaProdutoGrade>
    {
        private DAOFornecedor daoFornecedor;
        private DateTime _dataEscolhida;
        private Model.Fornecedor _fornecedor;
        private ObservableCollection<EntradaMercadoriaProdutoGrade> _entradas = new ObservableCollection<EntradaMercadoriaProdutoGrade>();
        private ObservableCollection<Model.Fornecedor> _fornecedores = new ObservableCollection<Model.Fornecedor>();
        public PesquisarEntradasPorFornecedorVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<EntradaMercadoriaProdutoGrade> abrePelaTelaPesquisaService) : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            daoEntidade = new DAOEntradaMercadoriaProdutoGrade(_session);
            daoFornecedor = new DAOFornecedor(_session);

            GetFornecedores();

            DataEscolhida = DateTime.Now;
        }

        private async void GetFornecedores()
        {
            Fornecedores = new ObservableCollection<Model.Fornecedor>(await daoFornecedor.Listar());
            Fornecedor = Fornecedores[0];
        }

        public override bool Editavel(object parameter)
        {
            return false;
        }

        public async override Task PesquisaItens(string termo)
        {
            if (DataEscolhida.Year == 1)
                return;

            var dao = daoEntidade as DAOEntradaMercadoriaProdutoGrade;
            Entradas = new ObservableCollection<EntradaMercadoriaProdutoGrade>(await dao.ListarPorPeriodoFornecedor(DataEscolhida, Fornecedor));
        }

        protected override WorksheetContainer<EntradaMercadoriaProdutoGrade>[] GetWorksheetContainers()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<EntradaMercadoriaProdutoGrade> Entradas
        {
            get => _entradas;
            set
            {
                _entradas = value;
                OnPropertyChanged("Entradas");
            }
        }

        public DateTime DataEscolhida
        {
            get => _dataEscolhida;
            set
            {
                _dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
                OnPropertyChanged("TermoPesquisa");
            }
        }
        public Model.Fornecedor Fornecedor
        {
            get => _fornecedor;
            set
            {
                _fornecedor = value;
                OnPropertyChanged("Fornecedor");
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public ObservableCollection<Model.Fornecedor> Fornecedores
        {
            get => _fornecedores;
            set
            {
                _fornecedores = value;
                OnPropertyChanged("Fornecedores");
            }
        }
    }
}