using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContagemModel = VandaModaIntimaWpf.Model.Contagem;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    class EditarContagemViewModel : CadastrarContagemViewModel, IEditarViewModel
    {
        private bool IsEditted = false;

        public override async void Salvar(object parameter)
        {
            var result = IsEditted = await _daoContagem.Atualizar(Contagem);

            if (result)
            {
                await SetStatusBarSucesso("Contagem Atualizada Com Sucesso");
            }
            else
            {
                SetStatusBarErro("Erro ao Atualizar Contagem");
            }
        }

        public bool EdicaoComSucesso()
        {
            return IsEditted;
        }

        public async void PassaId(object Id)
        {
            Contagem = await _session.LoadAsync<ContagemModel>(Id);
        }
    }
}
