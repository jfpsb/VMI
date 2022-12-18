using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.ViewModel.VendaEmCartao
{
    public class LerCSVCredishop : ILerCSVVendaEmCartao
    {
        public IList<Model.VendaEmCartao> GeraListaVendaEmCartao(string caminhoCSV, Model.Loja loja, Model.OperadoraCartao operadora)
        {
            using (var reader = new StreamReader(caminhoCSV))
            {
                List<Model.VendaEmCartao> vendas = new();

                //Não possui cabeçalho

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var data = values[1];
                    var hora = values[2].Remove(values[2].Length - 2);
                    var rv = loja.Cnpj + values[4].TrimStart('0');
                    var parcelas = int.Parse(values[7]);
                    var valorBruto = values[8].Insert(values[8].Length - 2, ".");

                    var modalidade = "CRÉDITO";
                    var bandeira = "CREDISHOP";


                    Model.VendaEmCartao vendaEmCartao = new Model.VendaEmCartao();

                    vendaEmCartao.DataHora = DateTime.ParseExact($"{data} {hora}", "yyyyMMdd HHmmss", new CultureInfo("pt-BR"));
                    vendaEmCartao.ValorBruto = double.Parse(valorBruto, NumberStyles.Any, CultureInfo.InvariantCulture);

                    if (parcelas > 1)
                    {
                        vendaEmCartao.ValorLiquido = vendaEmCartao.ValorBruto * 0.955; //taxa 4,5%
                    }
                    else
                    {
                        vendaEmCartao.ValorLiquido = vendaEmCartao.ValorBruto * 0.96; //taxa 4.0%
                    }

                    vendaEmCartao.Modalidade = modalidade.ToUpper();
                    vendaEmCartao.Bandeira = bandeira.ToUpper();
                    vendaEmCartao.RvCredishop = rv;
                    vendaEmCartao.Loja = loja;
                    vendaEmCartao.OperadoraCartao = operadora;

                    for (int i = 0; i < parcelas; i++)
                    {
                        //Data de pagamento de parcela é sempre calculada a partir da data original da compra, independente da data de parcela anterior
                        //De acordo com a credishop são 32 dias de prazo para crédito para cada parcela
                        DateTime dataPagamentoParcelaOriginal = vendaEmCartao.DataHora.Date.AddDays((i + 1) * 32);
                        DateTime dataPagamentoParcela = dataPagamentoParcelaOriginal;

                        ParcelaCartao parcelaCartao = new ParcelaCartao();
                        parcelaCartao.VendaEmCartao = vendaEmCartao;
                        parcelaCartao.ValorBruto = vendaEmCartao.ValorBruto / parcelas;
                        parcelaCartao.ValorLiquido = vendaEmCartao.ValorLiquido / parcelas;

                        //Se data de pagamento cair em dia de sábado, domingo ou feriado nacional, adiciono um dia até que encontre
                        //um dia útil
                        while (dataPagamentoParcela.DayOfWeek == DayOfWeek.Saturday ||
                            dataPagamentoParcela.DayOfWeek == DayOfWeek.Sunday ||
                            FeriadoJsonUtil.IsDataFeriadoNacional(dataPagamentoParcela))
                        {
                            dataPagamentoParcela = dataPagamentoParcela.AddDays(1);
                        }

                        parcelaCartao.DataPagamento = dataPagamentoParcela;

                        vendaEmCartao.Parcelas.Add(parcelaCartao);
                    }

                    vendas.Add(vendaEmCartao);
                }

                return vendas;
            }
        }
    }
}
