using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel.VendaEmCartao
{
    public class LerCSVCredishop : ILerCSVVendaEmCartao
    {
        public IList<Model.VendaEmCartao> GeraListaVendaEmCartao(string caminhoCSV, Model.Loja loja, Model.OperadoraCartao operadora)
        {
            using (var reader = new StreamReader(caminhoCSV))
            {
                List<Model.VendaEmCartao> vendas = new();
                Dictionary<string, Model.VendaEmCartao> dicionarioVendaEmCartao = new();

                //Não possui cabeçalho
                //Cada linha do arquivo csv de conciliação é uma parcela

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var rv = loja.Cnpj + values[3].TrimStart('0');
                    var dataVenda = values[6];
                    var valorBrutoParcela = Convert.ToDouble(values[7], CultureInfo.InvariantCulture);
                    var dataVencimento = DateTime.ParseExact(values[8], "dd/MM/yyyy", new CultureInfo("pt-BR")); //Data que a parcela é paga
                    var totalParcelas = Convert.ToInt32(values[5].Substring(2));

                    var modalidade = "CRÉDITO";
                    var bandeira = "CREDISHOP";

                    Model.VendaEmCartao vendaEmCartao;

                    if (dicionarioVendaEmCartao.ContainsKey(rv))
                    {
                        //Se for parcela de uma venda já lida antes, recupero do dicionário
                        vendaEmCartao = dicionarioVendaEmCartao[rv];
                    }
                    else
                    {
                        //Se for parcela de venda ainda não lida, crio o objeto de venda em cartão
                        vendaEmCartao = new Model.VendaEmCartao
                        {
                            DataHora = DateTime.ParseExact($"{dataVenda}", "dd/MM/yyyy", new CultureInfo("pt-BR")),
                            ValorBruto = valorBrutoParcela * totalParcelas,
                            Modalidade = modalidade.ToUpper(),
                            Bandeira = bandeira.ToUpper(),
                            RvCredishop = rv,
                            Loja = loja,
                            OperadoraCartao = operadora
                        };

                        if (totalParcelas > 1)
                        {
                            vendaEmCartao.ValorLiquido = vendaEmCartao.ValorBruto * 0.955; //taxa 4,5%
                        }
                        else
                        {
                            vendaEmCartao.ValorLiquido = vendaEmCartao.ValorBruto * 0.96; //taxa 4.0%
                        }

                        dicionarioVendaEmCartao.Add(rv, vendaEmCartao);
                    }

                    ParcelaCartao parcelaCartao = new ParcelaCartao();
                    parcelaCartao.VendaEmCartao = vendaEmCartao;
                    parcelaCartao.DataPagamento = dataVencimento;
                    parcelaCartao.ValorBruto = valorBrutoParcela;

                    if (totalParcelas > 1)
                    {
                        parcelaCartao.ValorLiquido = parcelaCartao.ValorBruto * 0.955; //taxa 4,5%
                    }
                    else
                    {
                        parcelaCartao.ValorLiquido = parcelaCartao.ValorBruto * 0.96; //taxa 4.0%
                    }

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
