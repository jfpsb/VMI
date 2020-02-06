using System;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class EditarLojaViewModel : CadastrarLojaViewModel, IEditarViewModel
    {
        private bool IsEditted = false;
        public override async void Salvar(object parameter)
        {
            if (Loja.Matriz != null && Loja.Matriz.Nome.Equals("SELECIONE UMA LOJA"))
                Loja.Matriz = null;

            var result = IsEditted = await daoLoja.Atualizar(Loja);

            if (result)
            {
                await SetStatusBarSucesso($"Loja {Loja.Cnpj} Atualizada Com Sucesso");
            }
            else
            {
                SetStatusBarErro("Erro ao Atualizar Loja");
            }
        }

        public async void PassaId(object Id)
        {
            Loja = await _session.LoadAsync<LojaModel>(Id);
        }

        public bool EdicaoComSucesso()
        {
            return IsEditted;
        }

        public new LojaModel Loja
        {
            get { return lojaModel; }
            set
            {
                lojaModel = value;
                OnPropertyChanged("Loja");
                OnPropertyChanged("MatrizComboBox");
            }
        }

        public LojaModel MatrizComboBox
        {
            get
            {
                if (Loja.Matriz == null)
                {
                    Loja.Matriz = new LojaModel("SELECIONE UMA LOJA");
                }

                return Loja.Matriz;
            }

            set
            {
                Loja.Matriz = value;
                OnPropertyChanged("MatrizComboBox");
            }
        }
    }
}
