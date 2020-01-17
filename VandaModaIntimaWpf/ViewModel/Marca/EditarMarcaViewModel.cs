using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class EditarMarcaViewModel : CadastrarMarcaViewModel, IEditarViewModel
    {
        private bool IsEditted = false;
        public bool EdicaoComSucesso()
        {
            return IsEditted;
        }

        public async void PassaId(object Id)
        {
            Marca = await _session.LoadAsync<MarcaModel>(Id);
        }
    }
}
