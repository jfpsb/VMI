﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class VisualizarHoraExtraVM : APesquisarViewModel<Model.HoraExtra>
    {
        private DAOFuncionario daoFuncionario;
        private ObservableCollection<Tuple<Model.Funcionario, TimeSpan, TimeSpan, DateTime>> _listaHoraExtra;
        private IList<Model.Funcionario> funcionarios;
        private DateTime _dataEscolhida;

        public ICommand AbrirImprimirHEComando { get; set; }

        public VisualizarHoraExtraVM(DateTime dataEscolhida, IMessageBoxService messageBoxService) : base(messageBoxService, null)
        {
            daoEntidade = new DAOHoraExtra(_session);
            daoFuncionario = new DAOFuncionario(_session);

            ListaHoraExtra = new ObservableCollection<Tuple<Model.Funcionario, TimeSpan, TimeSpan, DateTime>>();

            GetFuncionarios();

            DataEscolhida = dataEscolhida;

            AbrirImprimirHEComando = new RelayCommand(AbrirImprimirHE);
        }

        private void AbrirImprimirHE(object obj)
        {
            TelaRelatorioHoraExtra telaRelatorioHoraExtra = new TelaRelatorioHoraExtra(ListaHoraExtra);
            telaRelatorioHoraExtra.ShowDialog();
        }

        public override bool Editavel(object parameter)
        {
            return false;
        }

        public async override void PesquisaItens(string termo)
        {
            var daoHoraExtra = daoEntidade as DAOHoraExtra;

            ListaHoraExtra.Clear();

            foreach (var f in funcionarios)
            {
                var horasExtras = await daoHoraExtra.ListarPorAnoMesFuncionario(DataEscolhida.Year, DataEscolhida.Month, f);

                var he100 = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Contains("100%")).SingleOrDefault();
                var he55 = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Contains("055%")).SingleOrDefault();

                TimeSpan he100TimeSpan, he55TimeSpan;

                if (he100 != null)
                {
                    he100TimeSpan = he100.HorasTimeSpan;
                }
                else
                {
                    he100TimeSpan = new TimeSpan();
                }

                if (he55 != null)
                {
                    he55TimeSpan = he55.HorasTimeSpan;
                }
                else
                {
                    he55TimeSpan = new TimeSpan();
                }

                if (he100 != null || he55 != null)
                {
                    var tupla = Tuple.Create(f, he100TimeSpan, he55TimeSpan, DataEscolhida);
                    ListaHoraExtra.Add(tupla);
                }
            }
        }

        private async void GetFuncionarios()
        {
            funcionarios = await daoFuncionario.Listar();
        }

        public ObservableCollection<Tuple<Model.Funcionario, TimeSpan, TimeSpan, DateTime>> ListaHoraExtra
        {
            get => _listaHoraExtra;
            set
            {
                _listaHoraExtra = value;
                OnPropertyChanged("ListaHoraExtra");
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
    }
}
