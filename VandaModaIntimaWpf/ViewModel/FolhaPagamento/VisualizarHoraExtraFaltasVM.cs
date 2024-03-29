﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento.Util;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class VisualizarHoraExtraFaltasVM : APesquisarViewModel<Model.HoraExtra>
    {
        private DAOFuncionario daoFuncionario;
        private DAOFaltas daoFaltas;
        private ObservableCollection<Tuple<Model.FolhaPagamento, Model.HoraExtra, Model.HoraExtra, Model.HoraExtra, Model.Faltas, DateTime>> _listaHoraExtra;
        private IList<Model.Funcionario> funcionarios;
        private IList<Model.FolhaPagamento> folhas;
        private DateTime _dataEscolhida;

        public ICommand AbrirImprimirHEComando { get; set; }

        public VisualizarHoraExtraFaltasVM(DateTime dataEscolhida)
        {
            daoEntidade = new DAOHoraExtra(_session);
            daoFuncionario = new DAOFuncionario(_session);
            daoFaltas = new DAOFaltas(_session);
            ListaHoraExtra = new ObservableCollection<Tuple<Model.FolhaPagamento, Model.HoraExtra, Model.HoraExtra, Model.HoraExtra, Model.Faltas, DateTime>>();
            folhas = new ObservableCollection<Model.FolhaPagamento>();
            var task1 = GetFuncionarios();
            task1.Wait();
            var task2 = GetFolhas();
            task2.Wait();
            DataEscolhida = dataEscolhida;
            AbrirImprimirHEComando = new RelayCommand(AbrirImprimirHE);
        }

        private void AbrirImprimirHE(object obj)
        {
            //TODO: implementar viewmodel
            TelaRelatorioHoraExtraFaltas telaRelatorioHoraExtra = new TelaRelatorioHoraExtraFaltas(_session, ListaHoraExtra.Select(s => s.Item1).ToList(), DataEscolhida);
            telaRelatorioHoraExtra.ShowDialog();
        }

        public override bool Editavel(object parameter)
        {
            return false;
        }

        public async override Task PesquisaItens(string termo)
        {
            //TODO melhorar UI e consequentemente melhorar essa consulta
            var daoHoraExtra = daoEntidade as DAOHoraExtra;

            ListaHoraExtra.Clear();
            folhas.Clear();
            await GetFolhas();

            foreach (var f in folhas)
            {
                var horasExtras = await daoHoraExtra.ListarPorMesFuncionario(DataEscolhida.Year, DataEscolhida.Month, f.Funcionario);
                var falta = await daoFaltas.ListarFaltasPorMesFuncionarioSoma(DataEscolhida.Year, DataEscolhida.Month, f.Funcionario);

                if (falta == null)
                    falta = new Model.Faltas();

                var he100 = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Equals("HORA EXTRA C/100%")).FirstOrDefault();
                var he60 = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Equals("HORA EXTRA C/060%")).FirstOrDefault();
                var he50 = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Equals("HORA EXTRA C/050%")).FirstOrDefault();

                if (he100 == null)
                    he100 = new Model.HoraExtra();

                if (he60 == null)
                    he60 = new Model.HoraExtra();

                if (he50 == null)
                    he50 = new Model.HoraExtra();

                var possuiBonusPagoEmFolha = f.Bonus.Any(w => w.PagoEmFolha);

                if (he100.EmTimeSpan.TotalSeconds == 0
                    && he60.EmTimeSpan.TotalSeconds == 0
                    && falta.EmTimeSpan.TotalSeconds == 0
                    && he50.EmTimeSpan.TotalSeconds == 0
                    && !possuiBonusPagoEmFolha)
                    continue;

                var tupla = Tuple.Create(f, he100, he60, he50, falta, DataEscolhida);
                ListaHoraExtra.Add(tupla);
            }
        }

        private async Task GetFuncionarios()
        {
            funcionarios = await daoFuncionario.Listar();
        }

        private async Task GetFolhas()
        {
            folhas = await PesquisarFolhaPagamentoUtil.GeraListaDeFolhas(_session, funcionarios, DataEscolhida, Config.RetornaMetodoCalculoDeBonusDeMeta());
        }

        protected override WorksheetContainer<Model.HoraExtra>[] GetWorksheetContainers()
        {
            throw new NotImplementedException();
        }

        public override object GetCadastrarViewModel()
        {
            throw new NotImplementedException();
        }

        public override object GetEditarViewModel()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Tuple<Model.FolhaPagamento, Model.HoraExtra, Model.HoraExtra, Model.HoraExtra, Model.Faltas, DateTime>> ListaHoraExtra
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
