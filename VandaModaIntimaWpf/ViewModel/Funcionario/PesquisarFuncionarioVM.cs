using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class PesquisarFuncionarioVM : APesquisarViewModel<Model.Funcionario>
    {
        private int pesquisarPor;
        private bool _mostraDemitidos;

        private enum OpcoesPesquisa
        {
            Cpf,
            Nome
        }
        public PesquisarFuncionarioVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<Model.Funcionario> abrePelaTelaPesquisaService)
            : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            //TODO: Excel strategy
            pesquisarViewModelStrategy = new PesquisarFuncMsgVMStrategy();
            daoEntidade = new DAOFuncionario(_session);
            PesquisarPor = 1;
            PropertyChanged += PesquisarFuncVM_PropertyChanged;
        }

        private void PesquisarFuncVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "MostraDemitidos":
                    OnPropertyChanged("TermoPesquisa");
                    break;
            }
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        public override async Task PesquisaItens(string termo)
        {
            DAOFuncionario daoFuncionario = (DAOFuncionario)daoEntidade;
            switch (pesquisarPor)
            {
                case (int)OpcoesPesquisa.Cpf:
                    Entidades = new ObservableCollection<EntidadeComCampo<Model.Funcionario>>(EntidadeComCampo<Model.Funcionario>.CriarListaEntidadeComCampo(await daoFuncionario.ListarPorCpf(termo, MostraDemitidos)));
                    break;
                case (int)OpcoesPesquisa.Nome:
                    Entidades = new ObservableCollection<EntidadeComCampo<Model.Funcionario>>(EntidadeComCampo<Model.Funcionario>.CriarListaEntidadeComCampo(await daoFuncionario.ListarPorNome(termo, MostraDemitidos)));
                    break;
            }
        }

        protected override WorksheetContainer<Model.Funcionario>[] GetWorksheetContainers()
        {
            throw new NotImplementedException();
        }

        public int PesquisarPor
        {
            get { return pesquisarPor; }
            set
            {
                pesquisarPor = value;
                OnPropertyChanged("TermoPesquisa"); //Realiza pesquisa se mudar seleção de combobox
            }
        }

        public bool MostraDemitidos
        {
            get
            {
                return _mostraDemitidos;
            }

            set
            {
                _mostraDemitidos = value;
                OnPropertyChanged("MostraDemitidos");
            }
        }
    }
}
