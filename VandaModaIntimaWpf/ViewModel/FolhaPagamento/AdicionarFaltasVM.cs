using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarFaltasVM : ACadastrarViewModel<Model.Faltas>
    {
        private Model.FolhaPagamento _folha;

        public AdicionarFaltasVM(ISession session, Model.FolhaPagamento folha, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            daoEntidade = new DAOFaltas(session);
            viewModelStrategy = new AdicionarFaltasVMStrategy();
            Folha = folha;

            ResetaPropriedades();
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }

        public override void ResetaPropriedades()
        {
            Entidade = new Model.Faltas
            {
                Data = DateTime.Now,
                Funcionario = Folha.Funcionario,
            };
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (Entidade.Horas < 0 && Entidade.Minutos < 0)
            {
                BtnSalvarToolTip += "Horas e Minutos Não Podem Ser Menores Que Zero!\n";
                valido = false;
            }

            if (Entidade.Horas < 0)
            {
                BtnSalvarToolTip += "Valor de Horas Não Pode Ser Menor Que Zero!\n";
                valido = false;
            }

            if (Entidade.Horas > 99)
            {
                BtnSalvarToolTip += "Valor de Horas É Alto Demais!\n";
                valido = false;
            }

            if (Entidade.Minutos >= 60)
            {
                BtnSalvarToolTip += "Valor de Minutos Não Pode Ser Maior Ou Igual a 60!\n";
                valido = false;
            }

            if (Entidade.Minutos < 0)
            {
                BtnSalvarToolTip += "Valor de Minutos Não Pode Ser Menor Que Zero!\n";
                valido = false;
            }

            return valido;
        }

        public Model.FolhaPagamento Folha
        {
            get => _folha;
            set
            {
                _folha = value;
                OnPropertyChanged("Folha");
            }
        }
    }
}
