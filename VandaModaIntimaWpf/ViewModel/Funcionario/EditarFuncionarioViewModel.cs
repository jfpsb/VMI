using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class EditarFuncionarioViewModel : CadastrarFuncionarioViewModel
    {
        public EditarFuncionarioViewModel(ISession session) : base(session) { }

        public override async void Salvar(object parameter)
        {
            if (Funcionario.Loja.Cnpj == null)
                Funcionario.Loja = null;

            _result = await daoFuncionario.Merge(Funcionario);

            if (_result)
            {
                await SetStatusBarSucesso($"Funcionário {Funcionario.Nome} Atualizado Com Sucesso");
            }
            else
            {
                SetStatusBarErro("Erro ao Atualizar Funcionário");
            }
        }
    }
}
