using Newtonsoft.Json;
using NHibernate;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class CadastrarLojaViewModel : ACadastrarViewModel<LojaModel>
    {
        public ObservableCollection<LojaModel> Matrizes { get; set; }
        public CadastrarLojaViewModel(ISession session) : base(session)
        {
            cadastrarViewModelStrategy = new CadastrarLojaViewModelStrategy();
            daoEntidade = new DAOLoja(_session);
            Entidade = new LojaModel();
            Entidade.PropertyChanged += Entidade_PropertyChanged;
            GetMatrizes();
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

        public override async void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
        protected override void ExecutarAntesCriarDocumento()
        {
            if (Entidade.Matriz.Cnpj == null)
                Entidade.Matriz = null;
        }

        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
