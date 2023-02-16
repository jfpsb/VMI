using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace VandaModaIntimaWpf.ViewModel.VendaEmCartao
{
    public class LerCSVCaixaPagamentos : ILerCSVVendaEmCartao
    {
        public IList<Model.VendaEmCartao> GeraListaVendaEmCartao(string caminhoCSV, Model.Loja loja, Model.OperadoraCartao operadora)
        {
            Dictionary<string, Model.VendaEmCartao> dicionarioVendaEmCartao = new();

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
                    var values = line.Split(',');

                    var num_pedido = loja.Cnpj + values[5].TrimStart('0');
                    Model.VendaEmCartao vendaEmCartao;

                    if (dicionarioVendaEmCartao.ContainsKey(num_pedido))
                    {
                        vendaEmCartao = dicionarioVendaEmCartao[num_pedido];
                    }
                    else
                    {
                        vendaEmCartao = new Model.VendaEmCartao();

                        var data_hora = values[9];
                        var modalidade = values[25];
                        var valorBruto = values[14];
                        //Dependendo da modalidade o valor líquido está em local diferente
                        var valorLiquido = modalidade.ToUpper().Equals("CRÉDITO") && !values[10].Equals("Compra") ? values[23] : values[13];
                        var bandeira = values[12];

                        vendaEmCartao.DataHora = DateTime.Parse(data_hora, new CultureInfo("pt-BR"));
                        vendaEmCartao.ValorBruto = double.Parse(valorBruto, NumberStyles.Any, CultureInfo.InvariantCulture);
                        vendaEmCartao.ValorLiquido = double.Parse(valorLiquido, NumberStyles.Any, CultureInfo.InvariantCulture);
                        vendaEmCartao.Modalidade = modalidade.ToUpper();
                        vendaEmCartao.Bandeira = bandeira.ToUpper();
                        vendaEmCartao.NumPedidoCaixa = num_pedido;
                        vendaEmCartao.Loja = loja;
                        vendaEmCartao.OperadoraCartao = operadora;

                        dicionarioVendaEmCartao.Add(num_pedido, vendaEmCartao);
                    }

                    double valorParcelaBruto = double.Parse(values[17], NumberStyles.Any, CultureInfo.InvariantCulture);
                    double valorParcelaLiquido = double.Parse(values[13], NumberStyles.Any, CultureInfo.InvariantCulture);
                    DateTime dataPagamentoParcela = DateTime.Parse(values[1], CultureInfo.CurrentCulture);

                    Model.ParcelaCartao parcelaCartao = new Model.ParcelaCartao();
                    parcelaCartao.VendaEmCartao = vendaEmCartao;
                    parcelaCartao.DataPagamento = dataPagamentoParcela;
                    parcelaCartao.ValorBruto = valorParcelaBruto;
                    parcelaCartao.ValorLiquido = valorParcelaLiquido;

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
