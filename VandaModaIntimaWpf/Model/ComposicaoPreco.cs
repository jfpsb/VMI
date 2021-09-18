using System;

namespace VandaModaIntimaWpf.Model
{
    public class ComposicaoPreco : AModel
    {
        private long _id;
        private Loja _loja;
        private ProdutoGrade _produtoGrade;
        private DateTime _data;
        private double _precoCompra;
        private double _precoVenda;
        private double _frete;
        private bool _aplicaIcms = true;

        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }
        public ProdutoGrade ProdutoGrade
        {
            get => _produtoGrade;
            set
            {
                _produtoGrade = value;
                OnPropertyChanged("ProdutoGrade");
            }
        }
        public DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        public double ValorSimples
        {
            get
            {
                return PrecoVenda * Loja.UltimaAliquota.Simples;
            }
        }
        public double ValorIcms
        {
            get
            {
                if (AplicaIcms)
                    return PrecoCompra * Loja.UltimaAliquota.Icms;

                return 0;
            }
        }
        public double MargemContribuicao
        {
            get
            {
                return PrecoVenda - CustoTotal;
            }
        }
        public double CustoTotal
        {
            get => PrecoCompra + ValorIcms + ValorSimples + Frete;
        }
        public double PrecoCompra
        {
            get => _precoCompra;
            set
            {
                _precoCompra = value;
                OnPropertyChanged("PrecoCompra");
                OnPropertyChanged("CustoTotal");
                OnPropertyChanged("Lucro");
                OnPropertyChanged("MargemContribuicao");
            }
        }
        public double Frete
        {
            get => _frete;
            set
            {
                _frete = value;
                OnPropertyChanged("Frete");
                OnPropertyChanged("CustoTotal");
                OnPropertyChanged("Lucro");
                OnPropertyChanged("MargemContribuicao");
            }
        }
        public double Lucro
        {
            get
            {
                if (PrecoVenda == 0)
                    return 0;

                return MargemContribuicao / PrecoVenda;
            }
        }

        public double PrecoVenda
        {
            get => _precoVenda;
            set
            {
                _precoVenda = value;
                OnPropertyChanged("PrecoVenda");
                OnPropertyChanged("PrecoCompra");
                OnPropertyChanged("CustoTotal");
                OnPropertyChanged("Lucro");
                OnPropertyChanged("MargemContribuicao");
            }
        }

        public bool AplicaIcms
        {
            get => _aplicaIcms;
            set
            {
                _aplicaIcms = value;
                OnPropertyChanged("AplicaIcms");
                OnPropertyChanged("PrecoCompra");
                OnPropertyChanged("CustoTotal");
                OnPropertyChanged("Lucro");
                OnPropertyChanged("MargemContribuicao");
            }
        }
    }
}
