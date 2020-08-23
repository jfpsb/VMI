using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using FuncionarioModel = VandaModaIntimaWpf.Model.Funcionario;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class CadastrarFuncionarioViewModel : ACadastrarViewModel
    {
        protected DAOFuncionario daoFuncionario;
        private DAOLoja daoLoja;
        private FuncionarioModel funcionario;
        public ObservableCollection<LojaModel> Lojas { get; set; }

        public CadastrarFuncionarioViewModel(ISession session)
        {
            _session = session;
            daoFuncionario = new DAOFuncionario(_session);
            daoLoja = new DAOLoja(_session);
            funcionario = new FuncionarioModel();
            Funcionario.PropertyChanged += CadastrarViewModel_PropertyChanged;
            GetLojas();
            Funcionario.Loja = Lojas[0];
        }

        public FuncionarioModel Funcionario
        {
            get => funcionario;
            set
            {
                funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }
        public override async void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cpf":
                    var result = await daoFuncionario.ListarPorId(funcionario.Cpf);

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
        private async void GetLojas()
        {
            Lojas = new ObservableCollection<LojaModel>(await daoLoja.ListarExcetoDeposito());
        }

        public override void ResetaPropriedades()
        {
            Funcionario = new FuncionarioModel();
            Funcionario.Loja = Lojas[0];
        }

        //TODO: colocar strings em resources
        public override async void Salvar(object parameter)
        {
            if (Funcionario.Loja.Cnpj == null)
                Funcionario.Loja = null;

            string funcionarioJson = JsonConvert.SerializeObject(Funcionario);
            var couchDbResponse = await couchDbClient.CreateDocument(Funcionario.Cpf, funcionarioJson);

            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs()
            {
                CouchDbResponse = couchDbResponse,
                MensagemSucesso = "LOG de Inserção de Funcionário Criado com Sucesso",
                MensagemErro = "Erro ao Criar Log de Inserção de Funcionário",
                ObjetoSalvo = Funcionario
            };

            ChamaAposCriarDocumento(e);
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Funcionario.Cpf) || string.IsNullOrEmpty(Funcionario.Nome) || Funcionario.Salario <= 0.0)
                return false;

            return true;
        }

        public override void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
