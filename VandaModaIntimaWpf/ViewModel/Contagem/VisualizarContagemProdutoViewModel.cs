using System.Collections.Generic;
using System.Linq;
using VandaModaIntimaWpf.Model;
using ContagemModel = VandaModaIntimaWpf.Model.Contagem;
using ContagemProdutoModel = VandaModaIntimaWpf.Model.ContagemProduto;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    public class VisualizarContagemProdutoViewModel : ObservableObject
    {
        private ContagemModel _contagem;
        private IList<ContagemProdutoModel> _contagemGroupBy;

        public VisualizarContagemProdutoViewModel(ContagemModel _contagem)
        {
            this._contagem = _contagem;
            _contagemGroupBy = _contagem.Contagens
                .GroupBy(gb => gb.Produto.Cod_Barra)
                .Select(s => new ContagemProdutoModel
                {
                    Produto = s.First().Produto,
                    Quant = s.Sum(sum => sum.Quant)
                }).ToList();
        }

        public ContagemModel Contagem
        {
            get
            {
                return _contagem;
            }

            set
            {
                _contagem = value;
                OnPropertyChanged("Contagem");
            }
        }

        public IList<ContagemProdutoModel> ContagemGroupBy
        {
            get
            {
                return _contagemGroupBy;
            }

            set
            {
                _contagemGroupBy = value;
            }
        }
    }
}
