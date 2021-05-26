namespace VandaModaIntimaWpf.ViewModel.CompraDeFornecedor
{
    public class EditarCompraDeFornecedorVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro Ao Editar Compra De Fornecedor!";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Compra De Fornecedor Foi Editada Com Sucesso!";
        }

        public string MessageBoxCaption()
        {
            return "Edição De Compra De Fornecedor";
        }
    }
}
