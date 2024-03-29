﻿using NHibernate;

namespace VandaModaIntimaWpf.ViewModel.Despesa
{
    public class EditarDespesaVM : CadastrarDespesaVM
    {
        public EditarDespesaVM(ISession session, Model.Despesa despesa) : base(session, true)
        {
            viewModelStrategy = new EditarDespesaVMStrategy();
            Entidade = despesa;
            Entidade.TipoDespesa = despesa.TipoDespesa; //Necessário para executar o evento OnPropertyChanged dessa propriedade ao abrir a tela
            Entidade.Representante = despesa.Representante;
            Entidade.Fornecedor = despesa.Fornecedor;
            Entidade.Familiar = despesa.Familiar;
            TipoDescricao = Entidade.Descricao = despesa.Descricao;
            if (despesa.DataVencimento != null)
            {
                ultimaDataVencimento = despesa.DataVencimento; // Tem que ficar antes da próxima linha
                InserirVencimentoFlag = true;
            }
            TipoDescricao = Entidade.Descricao;
        }
    }
}
