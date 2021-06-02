using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Despesa
{
    public class CadastrarDespesaVM : ACadastrarViewModel<Model.Despesa>
    {
        private ObservableCollection<Model.TipoDespesa> _tiposDespesa;
        private ObservableCollection<Model.Fornecedor> _fornecedores;
        private bool _isDespesaEmpresarial;
        private bool _IsDespesaFamiliar;
        private bool _isOutrosDespesas;
        private string _tipoDescricao;

        public CadastrarDespesaVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            daoEntidade = new Model.DAO.DAO<Model.Despesa>(session);
            Entidade = new Model.Despesa();
            viewModelStrategy = new CadastrarDespesaVMStrategy();
            PropertyChanged += CadastrarDespesaVM_PropertyChanged;
        }

        private void CadastrarDespesaVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TipoDescricao":
                    if (TipoDescricao.Equals("OUTROS"))
                    {
                        IsOutrosDespesas = true;
                        Entidade.Descricao = string.Empty;
                    }
                    else
                    {
                        IsOutrosDespesas = false;
                        Entidade.Descricao = TipoDescricao;
                    }
                    break;
            }
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TipoDespesa":
                    if (Entidade.TipoDespesa.Nome.Equals("Despesa Empresarial"))
                    {
                        IsDespesaEmpresarial = true;
                        IsDespesaFamiliar = false;

                        Entidade.Familiar = string.Empty;
                        Entidade.Fornecedor = Fornecedores[0];
                    }
                    else if (Entidade.TipoDespesa.Nome.Equals("Despesa Familiar"))
                    {
                        IsDespesaEmpresarial = false;
                        IsDespesaFamiliar = true;

                        Entidade.Fornecedor = null;
                        OnPropertyChanged("TipoDescricao");
                    }
                    else if (Entidade.TipoDespesa.Nome.Equals("Outras Despesas"))
                    {
                        IsDespesaEmpresarial = false;
                        IsDespesaFamiliar = false;

                        Entidade.Familiar = string.Empty;
                        Entidade.Fornecedor = null;
                    }
                    break;
            }
        }

        public override void ResetaPropriedades()
        {
            Entidade = new Model.Despesa();
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (Entidade.Valor <= 0.0)
            {
                BtnSalvarToolTip += "Informe Um Valor Válido Para A Despesa!\n";
                valido = false;
            }

            if (Entidade.Descricao.Trim().Equals(string.Empty))
            {
                BtnSalvarToolTip += "Informe Uma Descrição Válida Para A Despesa!\n";
                valido = false;
            }

            return valido;
        }

        public ObservableCollection<Model.TipoDespesa> TiposDespesa { get => _tiposDespesa; set => _tiposDespesa = value; }
        public ObservableCollection<Model.Fornecedor> Fornecedores { get => _fornecedores; set => _fornecedores = value; }
        public bool IsDespesaEmpresarial { get => _isDespesaEmpresarial; set => _isDespesaEmpresarial = value; }
        public bool IsDespesaFamiliar { get => _IsDespesaFamiliar; set => _IsDespesaFamiliar = value; }
        public bool IsOutrosDespesas { get => _isOutrosDespesas; set => _isOutrosDespesas = value; }
        public string TipoDescricao { get => _tipoDescricao; set => _tipoDescricao = value; }
    }
}
