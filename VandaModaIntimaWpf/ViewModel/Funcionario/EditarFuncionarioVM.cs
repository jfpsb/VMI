using NHibernate;
using System;
using System.Collections.ObjectModel;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class EditarFuncionarioVM : CadastrarFuncionarioVM
    {
        /// <summary>
        /// Construtor de ViewModel para editar dados de funcionário.
        /// </summary>
        /// <param name="session">Session atual do NHibernate</param>
        /// <param name="funcionario">Objeto do funcionário que terá os dados editados</param>
        /// <param name="indexAba">Número da aba que deve estar selecionada ao abrir tela. Padrão é zero (primeira aba)</param>
        public EditarFuncionarioVM(ISession session, Model.Funcionario funcionario, int indexAba = 0) : base(session, true)
        {
            viewModelStrategy = new EditarFuncionarioVMStrategy();
            Entidade = funcionario;
            ChavesPix = new ObservableCollection<Model.ChavePix>(Entidade.ChavesPix);
            ContasBancarias = new ObservableCollection<Model.ContaBancaria>(Entidade.ContasBancarias);
            FeriasRegistradas = new ObservableCollection<Model.Ferias>(Entidade.Ferias);

            var task = GetFaltas();
            task.Wait();

            SetInicioAquisitivo();
            IndexAba = indexAba;
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
