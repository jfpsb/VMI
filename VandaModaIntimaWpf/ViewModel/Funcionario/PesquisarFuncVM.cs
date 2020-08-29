using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FuncionarioModel = VandaModaIntimaWpf.Model.Funcionario;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class PesquisarFuncVM : APesquisarViewModel<FuncionarioModel>
    {
        private int pesquisarPor;

        private enum OpcoesPesquisa
        {
            Cpf,
            Nome
        }
        public PesquisarFuncVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<FuncionarioModel> abrePelaTelaPesquisaService)
            : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            //TODO: Excel strategy
            pesquisarViewModelStrategy = new PesquisarFuncMsgVMStrategy();
            daoEntidade = new DAOFuncionario(_session);
            PesquisarPor = 1;
        }
        public override bool Editavel(object parameter)
        {
            return true;
        }

        public override async void PesquisaItens(string termo)
        {
            DAOFuncionario daoFuncionario = (DAOFuncionario)daoEntidade;
            switch (pesquisarPor)
            {
                case (int)OpcoesPesquisa.Cpf:
                    Entidades = new ObservableCollection<EntidadeComCampo<FuncionarioModel>>(EntidadeComCampo<FuncionarioModel>.ConverterIList(await daoFuncionario.ListarPorCpf(termo)));
                    break;
                case (int)OpcoesPesquisa.Nome:
                    Entidades = new ObservableCollection<EntidadeComCampo<FuncionarioModel>>(EntidadeComCampo<FuncionarioModel>.ConverterIList(await daoFuncionario.ListarPorNome(termo)));
                    break;
            }
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
    }
}
