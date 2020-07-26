using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View.Loja;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    class PesquisarLojaViewModelStrategy : IPesquisarViewModelStrategy<LojaModel>
    {
        public void AbrirAjuda(object parameter)
        {
            AjudaLoja ajudaLoja = new AjudaLoja();
            ajudaLoja.ShowDialog();
        }

        public bool? AbrirCadastrar(object parameter, ISession session)
        {
            CadastrarLojaViewModel cadastrarLojaViewModel = new CadastrarLojaViewModel(session);
            CadastrarLoja cadastrar = new CadastrarLoja()
            {
                DataContext = cadastrarLojaViewModel
            };
            return cadastrar.ShowDialog();
        }

        public bool? AbrirEditar(LojaModel clone, ISession session)
        {
            EditarLojaViewModel editarLojaViewModel = new EditarLojaViewModel(session)
            {
                Loja = clone
            };

            EditarLoja editar = new EditarLoja()
            {
                DataContext = editarLojaViewModel
            };

            return editar.ShowDialog();
        }

        public void AbrirExportarSQL(object parameter, IList<LojaModel> entidades)
        {
            throw new NotImplementedException();
        }

        public string MensagemApagarEntidadeCerteza(LojaModel e)
        {
            return string.Format(GetResource.GetString("certeza_deletar_loja"), e.Nome);
        }

        public string MensagemApagarMarcados()
        {
            return GetResource.GetString("deseja_apagar_loja_marcadas");
        }

        public string MensagemEntidadeDeletada(LojaModel e)
        {
            return string.Format(GetResource.GetString("loja_deletada_com_sucesso"), e.Nome);
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return GetResource.GetString("loja_nao_deletada");
        }

        public string MensagemEntidadesDeletadas()
        {
            return GetResource.GetString("lojas_deletadas_com_sucesso");
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return GetResource.GetString("lojas_nao_deletadas");
        }

        public void RestauraEntidade(LojaModel original, LojaModel backup)
        {
            original.Cnpj = backup.Cnpj;
            original.Nome = backup.Nome;
            original.Telefone = backup.Telefone;
            original.Endereco = backup.Endereco;
            original.InscricaoEstadual = backup.InscricaoEstadual;
            original.Matriz = backup.Matriz;
        }

        public string TelaApagarCaption()
        {
            return GetResource.GetString("apagar_lojas");
        }
    }
}
