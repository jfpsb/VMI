using NHibernate;
using System;
using System.Collections.Generic;
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
            return $"Tem Certeza Que Deseja Apagar a Marca {e.Nome}?";
        }

        public string MensagemApagarMarcados()
        {
            return "Desejar Apagar as Marcas Selecionadas?";
        }

        public string MensagemEntidadeDeletada(MarcaModel e)
        {
            return $"Marca {e.Nome} Foi Deletada Com Sucesso";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "Marca Não Foi Deletada";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "Marcas Apagadas Com Sucesso";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "Erro ao Apagar Marcas";
        }

        public void RestauraEntidade(MarcaModel original, MarcaModel backup)
        {
            throw new NotImplementedException();
        }

        public string TelaApagarCaption()
        {
            return "Apagar Marca(s)";
        }
    }
}
