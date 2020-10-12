using NHibernate;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class CadastrarLojaVM : ACadastrarViewModel<LojaModel>
    {
        public ObservableCollection<LojaModel> Matrizes { get; set; }
        public CadastrarLojaVM(ISession session, IMessageBoxService messageBoxService) : base(session, messageBoxService)
        {
            viewModelStrategy = new CadastrarLojaVMStrategy();
            daoEntidade = new DAOLoja(_session);
            Entidade = new LojaModel();
            Entidade.PropertyChanged += ChecaPropriedadesLoja;
            GetMatrizes();
            AntesDeInserirNoBancoDeDados += ConfiguraLojaAntesDeInserir;
        }

        private void ConfiguraLojaAntesDeInserir()
        {
            if (Entidade.Matriz?.Cnpj == null)
                Entidade.Matriz = null;
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Entidade.Cnpj) || string.IsNullOrEmpty(Entidade.Nome) || Entidade.Aluguel <= 0.0)
            {
                return false;
            }

            return true;
        }
        public override void ResetaPropriedades()
        {
            Entidade = new LojaModel();
            Entidade.Cnpj = Entidade.Nome = Entidade.Telefone = Entidade.Endereco = Entidade.InscricaoEstadual = string.Empty;
            Entidade.Matriz = Matrizes[0];
        }
        private async void GetMatrizes()
        {
            DAOLoja daoLoja = (DAOLoja)daoEntidade;
            Matrizes = new ObservableCollection<LojaModel>(await daoLoja.ListarMatrizes());
            Matrizes.Insert(0, new LojaModel(GetResource.GetString("matriz_nao_selecionada")));
        }

        public async void ChecaPropriedadesLoja(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cnpj":
                    var result = await daoEntidade.ListarPorId(Entidade.Cnpj);

                    if (result != null)
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Visible;
                        IsEnabled = false;
                    }
                    else
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Collapsed;
                        IsEnabled = true;
                    }

                    break;
            }
        }
    }
}
