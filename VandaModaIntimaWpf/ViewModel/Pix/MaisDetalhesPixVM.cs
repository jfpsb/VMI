using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel.Pix
{
    public class MaisDetalhesPixVM : ObservableObject
    {
        private Model.Pix.Pix _pix;
        public MaisDetalhesPixVM(Model.Pix.Pix pix)
        {
            _pix = pix;
        }

        public Model.Pix.Pix Pix
        {
            get
            {
                return _pix;
            }

            set
            {
                _pix = value;
            }
        }
    }
}
