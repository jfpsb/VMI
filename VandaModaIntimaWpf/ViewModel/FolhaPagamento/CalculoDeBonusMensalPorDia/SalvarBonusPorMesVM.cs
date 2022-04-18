using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public class SalvarBonusPorMesVM : ACadastrarViewModel<Bonus>
    {
        private DAOFuncionario daoFuncionario;
        private double _valorTotal;
        private double _valorDiario;
        private int _numDias;
        private DateTime _primeiroDia;
        private DateTime _ultimoDia;
        private ISalvarBonus _salvarBonus;
        private DateTime _dataEscolhida;
        private string _recebeRegularmenteHeader;
        private ObservableCollection<EntidadeComCampo<Model.Funcionario>> _funcionarios;
        private string caminhoImagemCalendario = Path.Combine(Path.GetTempPath(), "UltimoCalendario.png");
        private string caminhoFolhaPagamentoVMI = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Vanda Moda Íntima", "Folha De Pagamento");

        public ICommand AdicionarBonusComando { get; set; }

        public SalvarBonusPorMesVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate, DateTime dataEscolhida, double valorTotal, double valorDiario, int numDias, DateTime primeiroDia, DateTime ultimoDia, ISalvarBonus salvarBonus)
            : base(session, messageBoxService, issoEUmUpdate)
        {
            _numDias = numDias;
            _primeiroDia = primeiroDia;
            _ultimoDia = ultimoDia;
            _salvarBonus = salvarBonus;
            _valorDiario = valorDiario;
            RecebeRegularmenteHeader = _salvarBonus.RecebeRegularmenteHeader();
            ValorTotal = valorTotal;
            DataEscolhida = dataEscolhida;
            MessageBoxService = messageBoxService;
            daoFuncionario = new DAOFuncionario(session);
            daoEntidade = new DAOBonus(session);

            AposInserirNoBancoDeDados += SalvaImagemCalendarios;

            GetFuncionarios();
        }

        private void SalvaImagemCalendarios(AposInserirBDEventArgs e)
        {
            var marcados = Funcionarios.Where(w => w.IsChecked).Select(s => s.Entidade).ToList();
            string calendarioNome = "CalendarioPassagem.png";
            DateTime dataFolha = DataEscolhida.AddMonths(-1);

            if (_salvarBonus is SalvarAlmoco)
            {
                calendarioNome = "CalendarioAlimentacao.png";
            }

            foreach (var funcionario in marcados)
            {
                var diretorioFolha = Path.Combine(caminhoFolhaPagamentoVMI, funcionario.Nome, dataFolha.Year.ToString(), dataFolha.Month.ToString());
                Directory.CreateDirectory(diretorioFolha);

                if (File.Exists(Path.Combine(diretorioFolha, calendarioNome)))
                {
                    File.Delete(Path.Combine(diretorioFolha, calendarioNome));
                }

                File.Copy(caminhoImagemCalendario, Path.Combine(diretorioFolha, calendarioNome));
            }
        }

        protected async override Task<AposInserirBDEventArgs> ExecutarSalvar(object parametro)
        {
            var marcados = Funcionarios.Where(w => w.IsChecked).Select(s => s.Entidade).ToList();

            if (marcados.Count == 0)
            {
                MessageBoxService.Show("Nenhum Funcionário Foi Selecionado!");
            }
            else
            {
                // O mês sendo calculado é adicionado na folha referente ao mês anterior
                // para ser pago no vencimento desta folha
                DateTime dataFolha = DataEscolhida.AddMonths(-1);
                IList<Bonus> bonuses = new List<Bonus>();
                foreach (var funcionario in marcados)
                {
                    Bonus bonus = new Bonus();
                    var now = DateTime.Now;
                    bonus.Funcionario = funcionario;
                    bonus.Data = now;
                    bonus.Descricao = _salvarBonus.DescricaoBonus(_numDias, _valorDiario, _primeiroDia, _ultimoDia);
                    bonus.Valor = ValorTotal;
                    bonus.MesReferencia = dataFolha.Month;
                    bonus.AnoReferencia = dataFolha.Year;
                    bonuses.Add(bonus);
                }

                try
                {
                    await daoEntidade.Inserir(bonuses);
                    _result = true;
                    MessageBoxService.Show(_salvarBonus.MensagemInseridoSucesso(), _salvarBonus.MensagemCaption(),
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show(_salvarBonus.MensagemInseridoErro() +
                        $" Para mais detalhes acesse {Log.LogBanco}\n\n" +
                        $"{ex.Message}\n\n{ex.InnerException.Message}", _salvarBonus.MensagemCaption());
                }
            }

            AposInserirBDEventArgs e = new AposInserirBDEventArgs()
            {
                IssoEUmUpdate = IssoEUmUpdate,
                IdentificadorEntidade = Entidade.GetIdentifier(),
                Sucesso = _result,
                Parametro = parametro
            };

            return e;
        }
        public ObservableCollection<EntidadeComCampo<Model.Funcionario>> Funcionarios
        {
            get { return _funcionarios; }
            set
            {
                _funcionarios = value;
                OnPropertyChanged("Entidades");
            }
        }
        public double ValorTotal
        {
            get => _valorTotal;
            set
            {
                _valorTotal = value;
                OnPropertyChanged("ValorTotal");
            }
        }
        public DateTime DataEscolhida
        {
            get => _dataEscolhida;
            set
            {
                _dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
            }
        }
        public string RecebeRegularmenteHeader
        {
            get => _recebeRegularmenteHeader;
            set
            {
                _recebeRegularmenteHeader = value;
                OnPropertyChanged("RecebeRegularmenteHeader");
            }
        }
        private async void GetFuncionarios()
        {
            Funcionarios = new ObservableCollection<EntidadeComCampo<Model.Funcionario>>(EntidadeComCampo<Model.Funcionario>.CriarListaEntidadeComCampo(await daoFuncionario.Listar()));

            foreach (var e in Funcionarios)
            {
                e.Entidade.SetRegularmentePropriedade(_salvarBonus.RegularmentePropriedade());
            }
        }
        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
