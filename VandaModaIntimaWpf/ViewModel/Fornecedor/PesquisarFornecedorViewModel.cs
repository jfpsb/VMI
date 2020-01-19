﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class PesquisarFornecedorViewModel : APesquisarViewModel<FornecedorModel>
    {
        private DAOFornecedor daoFornecedor;
        private ObservableCollection<EntidadeComCampo<FornecedorModel>> fornecedores;
        private int pesquisarPor;

        public ICommand AbrirCadastrarOnlineComando { get; set; }
        private enum OpcoesPesquisa
        {
            Cnpj,
            Nome,
            Email
        }
        public PesquisarFornecedorViewModel() : base("Fornecedor")
        {
            AbrirCadastrarOnlineComando = new RelayCommand(AbrirCadastrarOnline);
            excelStrategy = new ExcelStrategy(new FornecedorExcelStrategy());
            daoFornecedor = new DAOFornecedor(_session);
            PropertyChanged += PesquisarViewModel_PropertyChanged;
            //Seleciona o index da combobox e por padrão realiza a pesquisa ao atualizar a propriedade
            //Lista todos os produtos ao abrir tela porque texto está vazio
            PesquisarPor = 0;
        }

        public override void AbrirCadastrar(object parameter)
        {
            CadastrarFornecedorManualmente cadastrar = new CadastrarFornecedorManualmente();
            cadastrar.ShowDialog();
            OnPropertyChanged("TermoPesquisa");
        }

        private void AbrirCadastrarOnline(object p)
        {
            CadastrarFornecedorOnline cadastrarFornecedorOnline = new CadastrarFornecedorOnline();
            cadastrarFornecedorOnline.ShowDialog();
            OnPropertyChanged("TermoPesquisa");
        }

        public override async void AbrirApagarMsgBox(object parameter)
        {
            TelaApagarDialog telaApagarDialog = new TelaApagarDialog("Tem Certeza Que Deseja Apagar o Fornecedor " + EntidadeSelecionada.Entidade.Nome + "?", "Apagar Fornecedor");
            bool? result = telaApagarDialog.ShowDialog();

            if (result == true)
            {
                bool deletado = await daoFornecedor.Deletar(EntidadeSelecionada.Entidade);

                if (deletado)
                {
                    SetStatusBarItemDeletado($"Fornecedor {EntidadeSelecionada.Entidade.Nome} Foi Deletado Com Sucesso");
                    OnPropertyChanged("TermoPesquisa");
                    await ResetarStatusBar();
                }
                else
                {
                    MensagemStatusBar = "Fornecedor Não Foi Deletado";
                }
            }
        }

        public override void AbrirEditar(object parameter)
        {
            FornecedorModel fornecedorBkp = (FornecedorModel)EntidadeSelecionada.Entidade.Clone();

            EditarFornecedor editar = new EditarFornecedor(EntidadeSelecionada.Entidade.Cnpj);
            var result = editar.ShowDialog();

            if (result.HasValue && result == true)
            {
                OnPropertyChanged("TermoPesquisa");
            }
            else
            {
                EntidadeSelecionada.Entidade.Cnpj = fornecedorBkp.Cnpj;
                EntidadeSelecionada.Entidade.Nome = fornecedorBkp.Nome;
                EntidadeSelecionada.Entidade.Fantasia = fornecedorBkp.Fantasia;
                EntidadeSelecionada.Entidade.Email = fornecedorBkp.Email;
                EntidadeSelecionada.Entidade.Telefone = fornecedorBkp.Telefone;
            }
        }

        public override void ChecarItensMarcados(object parameter)
        {
            int marcados = 0;

            foreach (EntidadeComCampo<FornecedorModel> em in Fornecedores)
            {
                if (em.IsChecked)
                    marcados++;
            }

            if (marcados > 1)
                VisibilidadeBotaoApagarSelecionado = Visibility.Visible;
            else
                VisibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        }

        public override async void ApagarMarcados(object parameter)
        {
            var Mensagem = (IMessageable)parameter;
            var resultMsgBox = Mensagem.MensagemSimOuNao("Desejar Apagar os Fornecedores Marcados?", "Apagar Fornecedores");

            if (resultMsgBox == MessageBoxResult.Yes)
            {
                IList<FornecedorModel> AApagar = new List<FornecedorModel>();

                foreach (EntidadeComCampo<FornecedorModel> em in Fornecedores)
                {
                    if (em.IsChecked)
                        AApagar.Add(em.Entidade);
                }

                bool result = await daoFornecedor.Deletar(AApagar);

                if (result)
                {
                    Mensagem.MensagemDeAviso("Fornecedores Apagados Com Sucesso");
                    OnPropertyChanged("TermoPesquisa");
                }
                else
                {
                    Mensagem.MensagemDeErro("Erro ao Apagar Fornecedores");
                }
            }
        }
        public override async void ExportarExcel(object parameter)
        {
            base.ExportarExcel(parameter);
            IsThreadLocked = true;
            await new Excel<FornecedorModel>(excelStrategy).Salvar(EntidadeComCampo<FornecedorModel>.ConverterIList(Fornecedores));
            IsThreadLocked = false;
            SetStatusBarExportadoComSucesso();
        }
        public override async void GetItems(string termo)
        {
            switch (pesquisarPor)
            {
                case (int)OpcoesPesquisa.Cnpj:
                    Fornecedores = new ObservableCollection<EntidadeComCampo<FornecedorModel>>(EntidadeComCampo<FornecedorModel>.ConverterIList(await daoFornecedor.ListarPorCnpj(termo)));
                    break;
                case (int)OpcoesPesquisa.Nome:
                    Fornecedores = new ObservableCollection<EntidadeComCampo<FornecedorModel>>(EntidadeComCampo<FornecedorModel>.ConverterIList(await daoFornecedor.ListarPorNome(termo)));
                    break;
                case (int)OpcoesPesquisa.Email:
                    Fornecedores = new ObservableCollection<EntidadeComCampo<FornecedorModel>>(EntidadeComCampo<FornecedorModel>.ConverterIList(await daoFornecedor.ListarPorEmail(termo)));
                    break;
            }
        }

        public override async void ImportarExcel(object parameter)
        {
            var OpenFileDialog = (IOpenFileDialog)parameter;

            string path = OpenFileDialog.OpenFileDialog();

            if (path != null)
            {
                IsThreadLocked = true;
                await new Excel<FornecedorModel>(excelStrategy, path).Importar();
                IsThreadLocked = false;
                OnPropertyChanged("TermoPesquisa");
            }
        }
        public override void AbrirAjuda(object parameter)
        {
            throw new System.NotImplementedException();
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
        public ObservableCollection<EntidadeComCampo<FornecedorModel>> Fornecedores
        {
            get { return fornecedores; }
            set
            {
                fornecedores = value;
                OnPropertyChanged("Fornecedores");
            }
        }
    }
}
