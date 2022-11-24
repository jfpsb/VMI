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

                    var data = values[0];
                    var hora = values[1];
                    var valorBruto = values[3];
                    var valorLiquido = values[15];
                    var modalidade = values[5];
                    var bandeira = values[8];
                    var nsu = values[16];

                    Model.VendaEmCartao vendaEmCartao = new Model.VendaEmCartao();

                    vendaEmCartao.DataHora = DateTime.Parse($"{data} {hora}", new CultureInfo("pt-BR"));
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
                    //Em caso de venda em débito
                    DateTime dataPagamentoParcela = vendaEmCartao.DataHora.Date.AddDays(1); //Dia de pagamento de primeira parcela

                    if (vendaEmCartao.Modalidade.Equals("CRÉDITO"))
                    {
                        dataPagamentoParcela = vendaEmCartao.DataHora.Date.AddDays(31); //Dia de pagamento de primeira parcela
                    }

                    for (int i = 0; i < numParcelas; i++)
                    {
                        Model.ParcelaCartao parcelaCartao = new Model.ParcelaCartao();
                        parcelaCartao.VendaEmCartao = vendaEmCartao;

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
