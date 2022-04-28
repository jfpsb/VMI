using System.Collections.Generic;
using System.Linq;
using VandaModaIntimaWpf.Model;
using ContagemModel = VandaModaIntimaWpf.Model.Contagem;
using ContagemProdutoModel = VandaModaIntimaWpf.Model.ContagemProduto;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    public class VisualizarContagemProdutoVM : ObservableObject
    {
        private ContagemModel _contagem;

        public VisualizarContagemProdutoVM(ContagemModel _contagem)
        {
            this._contagem = _contagem;
            ContagemGroupBy = _contagem.Contagens
                .GroupBy(gb => gb.ProdutoGrade.CodBarra)
                .Select(s => new ContagemProdutoModel
                {
                    ProdutoGrade = s.First().ProdutoGrade,
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

        public IList<ContagemProdutoModel> ContagemGroupBy { get; set; }
    }
}
