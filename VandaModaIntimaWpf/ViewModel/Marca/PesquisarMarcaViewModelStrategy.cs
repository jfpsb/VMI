using System;
using VandaModaIntimaWpf.Model.DAO;
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

        public void AbrirCadastrar(object parameter)
        {
            CadastrarMarca cadastrarMarca = new CadastrarMarca();
            cadastrarMarca.ShowDialog();
        }

        public bool? AbrirEditar(MarcaModel entidade)
        {
            EditarMarca editar = new EditarMarca(entidade.Nome);
            return editar.ShowDialog();
        }

        public void ExportarSQLInsert(object parameter, IDAO<MarcaModel> dao)
        {
            throw new NotImplementedException();
        }

        public void ExportarSQLUpdate(object parameter, IDAO<MarcaModel> dao)
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
