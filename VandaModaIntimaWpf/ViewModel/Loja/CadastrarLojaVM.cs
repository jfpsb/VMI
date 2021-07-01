using NHibernate;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
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
        public CadastrarLojaVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            viewModelStrategy = new CadastrarLojaVMStrategy();
            daoEntidade = new DAOLoja(_session);
            Entidade = new LojaModel();
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
            BtnSalvarToolTip = "";
            bool valido = true;

            if (string.IsNullOrEmpty(Entidade.Cnpj?.Trim()))
            {
                BtnSalvarToolTip += "O Campo de CNPJ Não Pode Ser Vazio!\n";
                valido = false;
            }

            if (string.IsNullOrEmpty(Entidade.Nome?.Trim()))
            {
                BtnSalvarToolTip += "O Campo de Nome Não Pode Ser Vazio!\n";
                valido = false;
            }

            if (Entidade.Aluguel.ToString()?.Trim().Length == 0 || Entidade.Aluguel <= 0.0)
            {
                BtnSalvarToolTip += "O Campo de Aluguel Não Pode Ser Vazio Ou Inválido!\n";
                return false;
            }

            return valido;
        }
        public override void ResetaPropriedades()
        {
            Entidade = new LojaModel();
            Entidade.Cnpj = Entidade.Nome = Entidade.Telefone = Entidade.Endereco = Entidade.InscricaoEstadual = string.Empty;
            Entidade.Matriz = Matrizes[0];
        }
        private async Task GetMatrizes()
        {
            DAOLoja daoLoja = (DAOLoja)daoEntidade;
            Matrizes = new ObservableCollection<LojaModel>(await daoLoja.ListarMatrizes());
            Matrizes.Insert(0, new LojaModel(GetResource.GetString("matriz_nao_selecionada")));
        }

        public async override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
