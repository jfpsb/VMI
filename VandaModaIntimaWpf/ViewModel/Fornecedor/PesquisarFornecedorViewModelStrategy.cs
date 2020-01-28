using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View.Fornecedor;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class PesquisarFornecedorViewModelStrategy : IPesquisarViewModelStrategy<FornecedorModel>
    {
        public void AbrirAjuda(object parameter)
        {
            throw new NotImplementedException();
        }

        public void AbrirCadastrar(object parameter)
        {
            CadastrarFornecedorManualmente cadastrar = new CadastrarFornecedorManualmente();
            cadastrar.ShowDialog();
        }

        public bool? AbrirEditar(FornecedorModel entidade)
        {
            EditarFornecedor editar = new EditarFornecedor(entidade.Cnpj);
            return editar.ShowDialog();
        }

        public async void ExportarSQLInsert(object parameter, IDAO<FornecedorModel> dao)
        {
            IList<FornecedorModel> fornecedores = await dao.Listar();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Script SQL (*.sql)|*.sql";

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                using (StreamWriter sw = File.CreateText(fileName))
                {
                    foreach (FornecedorModel fornecedor in fornecedores)
                    {
                        string campos = "`cnpj`, `nome`, `Situacao`";
                        string valores = $"\"{fornecedor.Cnpj}\", \"{fornecedor.Nome}\", 'NORMAL'";

                        if (!string.IsNullOrEmpty(fornecedor.Fantasia))
                        {
                            campos += ", `fantasia`";
                            valores += $", \"{fornecedor.Fantasia}\"";
                        }

                        if(!string.IsNullOrEmpty(fornecedor.Telefone))
                        {
                            campos += ", `telefone`";
                            valores += $", '{new string(fornecedor.Telefone?.Where(c => char.IsDigit(c)).ToArray())}'";
                        }

                        if (!string.IsNullOrEmpty(fornecedor.Email))
                        {
                            campos += ", `email`";
                            valores += $", \"{fornecedor.Email}\"";
                        }

                        sw.WriteLine($"INSERT INTO fornecedor ({campos}) " +
                            $"SELECT * FROM (SELECT {valores}) AS tmp " +
                            $"WHERE NOT EXISTS (SELECT cnpj FROM fornecedor WHERE cnpj = '{fornecedor.Cnpj}');");
                    }
                }
            }
        }

        public async void ExportarSQLUpdate(object parameter, IDAO<FornecedorModel> dao)
        {
            IList<FornecedorModel> fornecedores = await dao.Listar();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Script SQL (*.sql)|*.sql";

            if(saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;

                if(File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                using(StreamWriter sw = File.CreateText(fileName))
                {
                    foreach(FornecedorModel fornecedor in fornecedores)
                    {
                        sw.WriteLine($"UPDATE fornecedor SET nome = \"{fornecedor.Nome} - {fornecedor.Fantasia}\", " +
                            $"fantasia = \"{fornecedor.Fantasia}\", " +
                            $"email = \"{fornecedor.Email}\", " +
                            $"telefone = '{new string(fornecedor.Telefone?.Where(c => char.IsDigit(c)).ToArray())}' WHERE cnpj LIKE '{fornecedor.Cnpj}';");
                    }
                }
            }
        }

        public string MensagemApagarEntidadeCerteza(FornecedorModel e)
        {
            return $"Tem Certeza Que Deseja Apagar o Fornecedor {e.Nome}?";
        }

        public string MensagemApagarMarcados()
        {
            return "Desejar Apagar os Fornecedores Marcados?";
        }

        public string MensagemEntidadeDeletada(FornecedorModel e)
        {
            return $"Fornecedor {e.Nome} Foi Deletado Com Sucesso";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "Fornecedor Não Foi Deletado";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "Fornecedores Apagados Com Sucesso";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "Erro ao Apagar Fornecedores";
        }

        public void RestauraEntidade(FornecedorModel original, FornecedorModel backup)
        {
            original.Cnpj = backup.Cnpj;
            original.Nome = backup.Nome;
            original.Fantasia = backup.Fantasia;
            original.Email = backup.Email;
            original.Telefone = backup.Telefone;
            original.Produtos = backup.Produtos;
        }

        public string TelaApagarCaption()
        {
            return "Apagar Fornecedor(es)";
        }
    }
}
