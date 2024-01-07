using Microsoft.Reporting.WinForms;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for TelaRelatorioHoraExtra.xaml
    /// </summary>
    public partial class TelaRelatorioHoraExtraFaltas : Window
    {
        private IList<Model.FolhaPagamento> _folhas;
        private DAOHoraExtra daoHoraExtra;
        private DAOTipoHoraExtra daoTipoHora;
        private DAOFaltas daoFaltas;
        private DateTime dataEscolhida;
        public TelaRelatorioHoraExtraFaltas()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        public TelaRelatorioHoraExtraFaltas(ISession session, IList<Model.FolhaPagamento> folhas, DateTime data)
        {
            InitializeComponent();
            _folhas = folhas;
            daoHoraExtra = new DAOHoraExtra(session);
            daoFaltas = new DAOFaltas(session);
            daoTipoHora = new DAOTipoHoraExtra(session);
            dataEscolhida = data;
        }

        private async void HoraExtraFaltaReportViewer_Load(object sender, EventArgs e)
        {
            HoraExtraFaltasDataSet horaExtraFaltasDataSet = new HoraExtraFaltasDataSet();

            var listaOrdenadaPorLoja = _folhas.OrderBy(o => o.Funcionario.Loja.Cnpj);

            foreach (var folha in listaOrdenadaPorLoja)
            {
                var falta = await daoFaltas.ListarFaltasPorMesFuncionarioSoma(dataEscolhida.Year, dataEscolhida.Month, folha.Funcionario);
                var possuiHorasExtras = (await daoHoraExtra.ListarPorMesFuncionario(dataEscolhida.Year, dataEscolhida.Month, folha.Funcionario)).Any();

                if (falta == null) falta = new Model.Faltas();

                var herow = horaExtraFaltasDataSet.HoraExtra.NewHoraExtraRow();

                herow.nome_funcionario = folha.Funcionario.Nome;
                herow.nome_loja = folha.Funcionario.Loja.Nome;
                herow.mes_referencia = folha.MesReferencia;

                herow.faltas = falta.TotalEmString;

                var he_60 = await daoHoraExtra.ListarSomaPorMesFuncionarioTipoHoraExtra(folha.Mes, folha.Ano, folha.Funcionario, await daoTipoHora.ListarPorId(2));
                var he_100 = await daoHoraExtra.ListarSomaPorMesFuncionarioTipoHoraExtra(folha.Mes, folha.Ano, folha.Funcionario, await daoTipoHora.ListarPorId(1));

                if (he_60 != null)
                {
                    herow.hora_extra_60 = he_60.TotalEmString;
                }
                else
                {
                    herow.hora_extra_60 = "-- : --";
                }

                if (he_100 != null)
                {
                    herow.hora_extra_100 = he_100.TotalEmString;
                }
                else
                {
                    herow.hora_extra_100 = "-- : --";
                }

                herow.recebe_vale_transporte = folha.Funcionario.RecebePassagem && !folha.Funcionario.Nome.Contains("VANUSA") ? "SIM" : "NÃO";

                if (folha.Funcionario.RecebePassagem && !folha.Funcionario.Nome.Contains("VANUSA"))
                {
                    var bonus_passagem = folha.Bonus.Where(w => w.Descricao.Contains("ÔNIBUS")).FirstOrDefault();
                    if (bonus_passagem != null)
                    {
                        herow.valor_vale_transporte = bonus_passagem.Valor.ToString("C", CultureInfo.CurrentCulture);
                    }
                }
                else
                {
                    herow.valor_vale_transporte = "R$ 0,00";
                }

                string comissoes = "";

                var somaComissao = folha.Bonus.Where(w => w.Descricao.StartsWith("COMISSÃO") && w.PagoEmFolha).Sum(s => s.Valor);
                var somaGratificacao = folha.Bonus.Where(w => w.Descricao.StartsWith("GRATIFICAÇÃO") && w.PagoEmFolha).Sum(s => s.Valor);
                var valorAdicionalCaixa = folha.Bonus.Where(w => w.Descricao.Contains("ADICIONAL") && w.PagoEmFolha).Sum(s => s.Valor);

                //Contratado como vendedor mas exercendo função de caixa. Adicional entra como comissão
                if (folha.Funcionario.Funcao.Nome.Equals("VENDEDOR") && valorAdicionalCaixa != 0.0)
                {
                    somaComissao += valorAdicionalCaixa;
                }
                else if (folha.Funcionario.Funcao.Nome.Contains("CAIXA"))
                {
                    //Contratado como operador de caixa.
                    comissoes += "ADICIONAL DE CAIXA 17%\n";
                }

                if (somaComissao > 0.0)
                    comissoes += $"COMISSÃO = {somaComissao.ToString("C", CultureInfo.CurrentCulture)}\n";
                if (somaGratificacao > 0.0)
                    comissoes += $"GRATIFICAÇÃO = {somaGratificacao.ToString("C", CultureInfo.CurrentCulture)}\n";

                foreach (var bonus in folha.Bonus.Where(w => w.PagoEmFolha))
                {
                    if (bonus.Descricao.StartsWith("COMISSÃO") || bonus.Descricao.StartsWith("ADICIONAL")
                        || bonus.Descricao.StartsWith("AUXÍLIO") || bonus.Descricao.StartsWith("PASSAGEM")
                        || bonus.Descricao.StartsWith("GRATIFICAÇÃO"))
                    {
                        continue; //Insere bonus que não tem tratamento especial
                    }

                    comissoes += $"{bonus.Descricao.Replace(" (PAGO EM FOLHA)", "")} = {bonus.Valor.ToString("C", CultureInfo.CurrentCulture)}\n";
                }

                if (comissoes.Length > 0)
                    herow.comissoes = comissoes.Remove(comissoes.Length - 1, 1); //Remove o último \n

                horaExtraFaltasDataSet.HoraExtra.AddHoraExtraRow(herow);
            }

            ReportDataSource reportDataSource = new ReportDataSource("DataSetHoraExtraFalta", horaExtraFaltasDataSet.Tables[0]);

            ReportViewerUtil.ConfiguraReportViewer(HoraExtraFaltaReportViewer,
                "VandaModaIntimaWpf.View.FolhaPagamento.Relatorios.RelatorioHoraExtraFaltas.rdlc",
                reportDataSource);

            HoraExtraFaltaReportViewer.LocalReport.Refresh();
            HoraExtraFaltaReportViewer.RefreshReport();
        }
    }
}
