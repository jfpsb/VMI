using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.ViewModel.VendaEmCartao
{
    public class LerCSVRede : ILerCSVVendaEmCartao
    {
        public IList<Model.VendaEmCartao> GeraListaVendaEmCartao(string caminhoCSV, Model.Loja loja, Model.OperadoraCartao operadora)
        {
            using (var reader = new StreamReader(caminhoCSV))
            {
                List<Model.VendaEmCartao> vendas = new();

                //Consome primeira linha (cabeçalho)
                if (!reader.EndOfStream)
                {
                    reader.ReadLine();
                }

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    if (values[2].Equals("estornada")) continue;

                    var data = values[0];
                    var hora = values[1];
                    var valorBruto = values[4];
                    var valorLiquido = values[15];
                    var modalidade = values[5];
                    var bandeira = values[8];
                    var nsu = values[16];

                    Model.VendaEmCartao vendaEmCartao = new Model.VendaEmCartao();

                    vendaEmCartao.DataHora = DateTime.Parse($"{data} {hora}", new CultureInfo("pt-BR"));

                    if (vendaEmCartao.DataHora.Hour >= 1 && vendaEmCartao.DataHora.Hour <= 7)
                    {
                        vendaEmCartao.DataHora = vendaEmCartao.DataHora.AddHours(12);
                    }

                    vendaEmCartao.ValorBruto = double.Parse(valorBruto, NumberStyles.Any, CultureInfo.CurrentCulture);
                    vendaEmCartao.ValorLiquido = double.Parse(valorLiquido, NumberStyles.Any, CultureInfo.CurrentCulture);
                    vendaEmCartao.Modalidade = modalidade.ToUpper();
                    vendaEmCartao.Bandeira = bandeira.ToUpper();
                    vendaEmCartao.NsuRede = nsu;
                    vendaEmCartao.Loja = loja;
                    vendaEmCartao.OperadoraCartao = operadora;

                    int numParcelas = int.Parse(values[7]);
                    double valorParcelaBruto = vendaEmCartao.ValorBruto / numParcelas;
                    double valorParcelaLiquido = vendaEmCartao.ValorLiquido / numParcelas;
                    int diasParaPagamento = 1; //Guarda em quantos dias úteis será paga a parcela da venda

                    if (vendaEmCartao.Modalidade.Equals("CRÉDITO"))
                    {
                        diasParaPagamento = 31; //Se for crédito muda para 31, se não, continua em 1 dia (débito)
                    }

                    for (int i = 0; i < numParcelas; i++)
                    {
                        Model.ParcelaCartao parcelaCartao = new Model.ParcelaCartao();
                        parcelaCartao.VendaEmCartao = vendaEmCartao;

                        //Data de pagamento de parcela é sempre calculada a partir da data original da compra,
                        //independente da data de parcela anterior
                        DateTime dataPagamentoParcelaOriginal = vendaEmCartao.DataHora.Date.AddDays((i + 1) * diasParaPagamento);
                        DateTime dataPagamentoParcela = dataPagamentoParcelaOriginal;

                        //Se data de pagamento cair em dia de sábado, domingo ou feriado nacional, adiciono um dia até que encontre
                        //um dia útil
                        while (dataPagamentoParcela.DayOfWeek == DayOfWeek.Saturday ||
                            dataPagamentoParcela.DayOfWeek == DayOfWeek.Sunday ||
                            FeriadoJsonUtil.IsDataFeriadoNacional(dataPagamentoParcela))
                        {
                            dataPagamentoParcela = dataPagamentoParcela.AddDays(1);
                        }

                        parcelaCartao.DataPagamento = dataPagamentoParcela;
                        parcelaCartao.ValorBruto = valorParcelaBruto;
                        parcelaCartao.ValorLiquido = valorParcelaLiquido;

                        //Adiciono 31 dias para data de pagamento de próxima parcela
                        //Se houver mais de uma parcela então é venda em crédito
                        //então somo 31 dias
                        dataPagamentoParcela = dataPagamentoParcela.AddDays(31);

                        vendaEmCartao.Parcelas.Add(parcelaCartao);
                    }

                    vendas.Add(vendaEmCartao);
                }

                return vendas;
            }
        }
    }
}
