using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    var values = line.Split(';');

                    var data = values[1];
                    var hora = values[2];
                    var rv = values[4];
                    var parcelas = int.Parse(values[6]);
                    var valorBruto = values[7];

                    var modalidade = "CRÉDITO";
                    var bandeira = "CREDISHOP";


                    Model.VendaEmCartao vendaEmCartao = new Model.VendaEmCartao();

                    vendaEmCartao.DataHora = DateTime.Parse($"{data} {hora}", new CultureInfo("pt-BR"));
                    vendaEmCartao.ValorBruto = double.Parse(valorBruto, NumberStyles.Any, CultureInfo.CurrentCulture);

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

                    vendas.Add(vendaEmCartao);
                }

                return vendas;
            }
        }
    }
}
