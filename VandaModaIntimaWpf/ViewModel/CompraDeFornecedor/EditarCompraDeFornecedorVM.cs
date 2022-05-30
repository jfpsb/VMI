﻿using NHibernate;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.ViewModel.CompraDeFornecedor
{
    public class EditarCompraDeFornecedorVM : CadastrarCompraDeFornecedorVM
    {
        public EditarCompraDeFornecedorVM() : base()
        {
            //TODO: parametros, compradeforncedor
            daoEntidade = new DAO<Model.CompraDeFornecedor>(_session);
            viewModelStrategy = new EditarCompraDeFornecedorVMStrategy();
            Arquivos = new System.Collections.ObjectModel.ObservableCollection<Model.ArquivosCompraFornecedor>(Entidade.Arquivos);
        }
    }
}
