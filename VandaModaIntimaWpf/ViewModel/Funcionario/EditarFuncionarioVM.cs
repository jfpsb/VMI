using NHibernate;
using System;
using System.Collections.ObjectModel;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class EditarFuncionarioVM : CadastrarFuncionarioVM
    {
        public EditarFuncionarioVM(ISession session, Model.Funcionario funcionario) : base(session, true)
        {
            viewModelStrategy = new EditarFuncionarioVMStrategy();
            Entidade = funcionario;
            ChavesPix = new ObservableCollection<Model.ChavePix>(Entidade.ChavesPix);
            ContasBancarias = new ObservableCollection<Model.ContaBancaria>(Entidade.ContasBancarias);

            SetInicioAquisitivo();
            //InicioFerias = InicioConcessivo;
        }

        private void SetInicioAquisitivo()
        {
            DateTime inicioAquisitivo = Entidade.Admissao.Value;

            while (inicioAquisitivo.AddYears(1) < DateTime.Now)
            {
                inicioAquisitivo = inicioAquisitivo.AddYears(1);
            }

            InicioAquisitivo = inicioAquisitivo;
        }
    }
}
