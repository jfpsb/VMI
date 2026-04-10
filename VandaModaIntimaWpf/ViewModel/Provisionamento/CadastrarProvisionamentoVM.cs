using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Provisionamento
{
    public class CadastrarProvisionamentoVM : ACadastrarViewModel<Model.Provisionamento>
    {
        private DAOFuncionario daoFuncionario;

        public ICommand ImportarCSVComando { get; set; }

        public CadastrarProvisionamentoVM(ISession session, bool isUpdate = false) : base(session, isUpdate)
        {
            viewModelStrategy = new CadastrarProvisionamentoVMStrategy();
            daoEntidade = new DAOProvisionamento(_session);
            daoFuncionario = new DAOFuncionario(_session);
            Entidades = new ObservableCollection<Model.Provisionamento>();

            ImportarCSVComando = new RelayCommand(ImportarCSV);
        }

        private async void ImportarCSV(object parameter)
        {
            try
            {
                if (parameter == null)
                    throw new Exception("Parâmetro de comando não configurado.");

                var openFileDialog = parameter as IOpenFileDialog;
                var caminhoArquivo = openFileDialog.OpenFileDialog();

                if (caminhoArquivo != null)
                {
                    try
                    {
                        var linhas = File.ReadAllLines(caminhoArquivo).Skip(1);

                        foreach (var linha in linhas)
                        {
                            var colunas = linha.Split(';').Select(c => c.Replace("\"", "").Replace(".", "").Replace("-", "")).ToArray();

                            var prov = new Model.Provisionamento
                            {
                                Funcionario = await daoFuncionario.ListarPorId(colunas[0]), //CPF
                                Ano = int.Parse(colunas[7].Split('/')[1]), //Competencia Apuracao Ano
                                UltimaRemuneracao = double.Parse(colunas[11]) //Base Remuneracao Total
                            };

                            var stringMes = colunas[7].Split('/')[0];

                            if (stringMes == "13o") //Arquivo CSV referente ao décimo terceiro
                            {
                                prov.DecimoTerceiro = 0; //Não provisiona
                                prov.Mes = 12; //Salvo como mês 12
                            }
                            else
                            {
                                prov.DecimoTerceiro = prov.UltimaRemuneracao / 12;
                                prov.Mes = int.Parse(colunas[7].Split('/')[0]);
                            }

                            //prov.AvisoPrevio = prov.UltimaRemuneracao / 12; //Fazer calculo correto depois

                            prov.MultaFgts = prov.UltimaRemuneracao * 0.08 * 0.4; //40% de 8%
                            prov.SalarioBase = prov.Funcionario.Salario;

                            Entidades.Add(prov);
                        }
                    }
                    catch (Exception ex)
                    {
                        _messageBoxService.Show($"Erro Ao Ler Arquivo CSV.\n\n{ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _messageBoxService.Show(ex.Message);
            }
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ResetaPropriedades(AposCRUDEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (Entidades.Count == 0)
                return false;

            return true;
        }
    }
}
