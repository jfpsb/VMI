using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View.Marca;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class PesquisarMarcaViewModelStrategy : IPesquisarViewModelStrategy<MarcaModel>
    {
        public void AbrirAjuda(object parameter)
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(object parameter, ISession session)
        {
            CadastrarMarcaViewModel cadastrarMarcaViewModel = new CadastrarMarcaViewModel(session);
            CadastrarMarca cadastrarMarca = new CadastrarMarca()
            {
                DataContext = cadastrarMarcaViewModel
            };
            return cadastrarMarca.ShowDialog();
        }

        public bool? AbrirEditar(MarcaModel clone, ISession session)
        {
            return false;
        }
        public void AbrirExportarSQL(object parameter, IList<MarcaModel> entidades)
        {
            throw new NotImplementedException();
        }

        public string MensagemApagarEntidadeCerteza(MarcaModel e)
        {
            return string.Format(StringResource.GetString("certeza_deletar_marca"), e.Nome);
        }

        public string MensagemApagarMarcados()
        {
            return StringResource.GetString("deseja_apagar_marca_marcadas");
        }

        public string MensagemEntidadeDeletada(MarcaModel e)
        {
            return string.Format(StringResource.GetString("marca_deletada_com_sucesso"), e.Nome);
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return StringResource.GetString("marca_nao_deletada");
        }

        public string MensagemEntidadesDeletadas()
        {
            return StringResource.GetString("marcas_deletadas_com_sucesso");
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return StringResource.GetString("marcas_nao_deletadas");
        }

        public void RestauraEntidade(MarcaModel original, MarcaModel backup)
        {
            throw new NotImplementedException();
        }

        public string TelaApagarCaption()
        {
            return StringResource.GetString("apagar_marcas");
        }
    }
}
