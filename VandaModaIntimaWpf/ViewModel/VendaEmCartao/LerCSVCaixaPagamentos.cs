using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace VandaModaIntimaWpf.ViewModel.VendaEmCartao
{
    public class LerCSVCaixaPagamentos : ILerCSVVendaEmCartao
    {
        public IList<Model.VendaEmCartao> GeraListaVendaEmCartao(string caminhoCSV, Model.Loja loja, Model.OperadoraCartao operadora)
        {
            Dictionary<string, Model.VendaEmCartao> dicionarioVendaEmCartao = new();

            using (var reader = new StreamReader(caminhoCSV, Encoding.GetEncoding(1256), true))
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

                    var comprovante_venda = loja.Cnpj + values[4].TrimStart('0');
                    var valorBrutoParcela = double.Parse(values[14], NumberStyles.AllowCurrencySymbol | NumberStyles.Currency);
                    var valorLiquidoParcela = double.Parse(values[16], NumberStyles.AllowCurrencySymbol | NumberStyles.Currency);

                    Model.VendaEmCartao vendaEmCartao;

                    if (dicionarioVendaEmCartao.ContainsKey(comprovante_venda))
                    {
                        vendaEmCartao = dicionarioVendaEmCartao[comprovante_venda];
                    }
                    else
                    {
                        vendaEmCartao = new Model.VendaEmCartao();

                        var data = values[0];
                        var modalidade = values[10];
                        var valorBrutoTransacao = values[13];
                        var valorTaxaMDR = double.Parse(values[15], NumberStyles.AllowCurrencySymbol | NumberStyles.Currency);
                        var aliquotaTaxa = valorTaxaMDR / valorBrutoParcela;
                        var bandeira = values[11];

                        vendaEmCartao.DataHora = DateTime.Parse(data, new CultureInfo("pt-BR"));
                        vendaEmCartao.ValorBruto = double.Parse(valorBrutoTransacao, NumberStyles.AllowCurrencySymbol | NumberStyles.Currency);
                        vendaEmCartao.ValorLiquido = vendaEmCartao.ValorBruto * (1.0 - aliquotaTaxa);
                        vendaEmCartao.Modalidade = modalidade.ToUpper();
                        vendaEmCartao.Bandeira = bandeira.ToUpper();
                        vendaEmCartao.NumPedidoCaixa = comprovante_venda;
                        vendaEmCartao.Loja = loja;
                        vendaEmCartao.OperadoraCartao = operadora;

                        dicionarioVendaEmCartao.Add(comprovante_venda, vendaEmCartao);
                    }

                    DateTime dataPagamentoParcela = DateTime.Parse(values[19], CultureInfo.CurrentCulture);

                    Model.ParcelaCartao parcelaCartao = new Model.ParcelaCartao();
                    parcelaCartao.VendaEmCartao = vendaEmCartao;
                    parcelaCartao.DataPagamento = dataPagamentoParcela;
                    parcelaCartao.ValorBruto = valorBrutoParcela;
                    parcelaCartao.ValorLiquido = valorLiquidoParcela;

                    vendaEmCartao.Parcelas.Add(parcelaCartao);
                }

                foreach (var item in dicionarioVendaEmCartao)
                {
                    vendas.Add(item.Value);
                }

                return vendas;
            }
        }
    }
}
