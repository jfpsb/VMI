using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;

namespace VandaModaIntimaWpf.ViewModel.Provisionamento
{
    class PesquisarProvisionamentoVM : APesquisarViewModel<Model.Provisionamento>
    {
        private DateTime _mesReferencia;
        private bool _listarProvisionamentosTotais;

        public PesquisarProvisionamentoVM()
        {
            daoEntidade = new DAOProvisionamento(_session);

            MesReferencia = DateTime.Now;
        }

        public override bool Editavel(object parameter)
        {
            return false;
        }

        public override object GetCadastrarViewModel()
        {
            throw new NotImplementedException();
        }

        public override object GetEditarViewModel()
        {
            throw new NotImplementedException();
        }

        public override async Task PesquisaItens(string termo)
        {
            DAOProvisionamento daoProvisionamento = (DAOProvisionamento) daoEntidade;

            if(ListarProvisionamentosTotais)
            {
                Entidades = new ObservableCollection<EntidadeComCampo<Model.Provisionamento>>(EntidadeComCampo<Model.Provisionamento>.CriarListaEntidadeComCampo(await daoProvisionamento.ListarProvisionamentosTotaisGroupByFuncionario()));
            }
            else
            {
                Entidades = new ObservableCollection<EntidadeComCampo<Model.Provisionamento>>(EntidadeComCampo<Model.Provisionamento>.CriarListaEntidadeComCampo(await daoProvisionamento.ListarProvisionamentosPorMesAno(MesReferencia.Month, MesReferencia.Year)));
            }
        }

        protected override WorksheetContainer<Model.Provisionamento>[] GetWorksheetContainers()
        {
            throw new NotImplementedException();
        }

        public DateTime MesReferencia
        {
            get
            {
                return _mesReferencia;
            }

            set
            {
                _mesReferencia = value;
                OnPropertyChanged("MesReferencia");
                OnPropertyChanged("TermoPesquisa"); //Realiza a pesquisa ao mudar mês de referência
            }
        }

        public bool ListarProvisionamentosTotais
        {
            get
            {
                return _listarProvisionamentosTotais;
            }

            set
            {
                _listarProvisionamentosTotais = value;
                OnPropertyChanged("ListarProvisionamentosTotais");
                OnPropertyChanged("TermoPesquisa"); //Realiza a pesquisa ao mudar esta opção
            }
        }
    }
}
