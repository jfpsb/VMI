﻿using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.View.Avisos;
using VandaModaIntimaWpf.View.CompraDeFornecedor;
using VandaModaIntimaWpf.View.Contagem;
using VandaModaIntimaWpf.View.Despesa;
using VandaModaIntimaWpf.View.EntradaDeMercadoria;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.View.FolhaPagamento.CalculoDeBonusMensalPorDia;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.View.Funcionario;
using VandaModaIntimaWpf.View.Grade;
using VandaModaIntimaWpf.View.Loja;
using VandaModaIntimaWpf.View.Marca;
using VandaModaIntimaWpf.View.Pix;
using VandaModaIntimaWpf.View.PontoEletronico;
using VandaModaIntimaWpf.View.Produto;
using VandaModaIntimaWpf.View.RecebimentoCartao;
using VandaModaIntimaWpf.View.Representante;
using VandaModaIntimaWpf.View.TipoGrade;
using VandaModaIntimaWpf.View.VendaEmCartao;
using VandaModaIntimaWpf.ViewModel.Avisos;
using VandaModaIntimaWpf.ViewModel.CompraDeFornecedor;
using VandaModaIntimaWpf.ViewModel.Contagem;
using VandaModaIntimaWpf.ViewModel.Despesa;
using VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia;
using VandaModaIntimaWpf.ViewModel.Fornecedor;
using VandaModaIntimaWpf.ViewModel.Funcionario;
using VandaModaIntimaWpf.ViewModel.Grade;
using VandaModaIntimaWpf.ViewModel.Loja;
using VandaModaIntimaWpf.ViewModel.Marca;
using VandaModaIntimaWpf.ViewModel.Pix;
using VandaModaIntimaWpf.ViewModel.PontoEletronico;
using VandaModaIntimaWpf.ViewModel.Produto;
using VandaModaIntimaWpf.ViewModel.RecebimentoCartao;
using VandaModaIntimaWpf.ViewModel.Representante;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.TipoGrade;
using VandaModaIntimaWpf.ViewModel.VendaEmCartao;

namespace VandaModaIntimaWpf.ViewModel
{
    class VandaModaIntimaVM : ObservableObject
    {
        public VandaModaIntimaVM()
        {
            WindowService.RegistrarWindow<SalvarCompraDeFornecedor, CadastrarCompraDeFornecedorVM>();
            WindowService.RegistrarWindow<SalvarCompraDeFornecedor, EditarCompraDeFornecedorVM>();
            WindowService.RegistrarWindow<CadastrarContagem, CadastrarContagemVM>();
            WindowService.RegistrarWindow<EditarContagem, EditarContagemVM>();
            WindowService.RegistrarWindow<SalvarDespesa, CadastrarDespesaVM>();
            WindowService.RegistrarWindow<SalvarDespesa, EditarDespesaVM>();
            WindowService.RegistrarWindow<SalvarEntradaDeMercadoria, CadastrarEntradaDeMercadoriaVM>();
            WindowService.RegistrarWindow<SelecionaMultiplasLojas, SelecionaMultiplasLojasVM>();

            WindowService.RegistrarWindow<CalculoBonusMensalPorDiaView, CalculoDeBonusMensalPorDiaVM>();
            WindowService.RegistrarWindow<SalvarBonusDeFuncionario, SalvarBonusPorMesVM>();

            WindowService.RegistrarWindow<AdicionarAdiantamento, AdicionarAdiantamentoVM>();
            WindowService.RegistrarWindow<AdicionarBonus, AdicionarBonusVM>();
            WindowService.RegistrarWindow<AdicionarFaltas, AdicionarFaltasVM>();
            WindowService.RegistrarWindow<AdicionarHoraExtra, AdicionarHoraExtraVM>();
            WindowService.RegistrarWindow<AdicionarMetaIndividual, AdicionarMetaIndividualVM>();
            WindowService.RegistrarWindow<AdicionarObservacaoFolha, AdicionarObservacaoFolhaVM>();
            WindowService.RegistrarWindow<AdicionarSalarioLiquido, AdicionarSalarioLiquidoVM>();
            WindowService.RegistrarWindow<AdicionarTotalVendido, AdicionarTotalVendidoVM>();
            WindowService.RegistrarWindow<GerenciarParcelas, GerenciarParcelasVM>();
            WindowService.RegistrarWindow<View.FolhaPagamento.MaisDetalhes, FolhaPagamento.MaisDetalhesVM>();
            WindowService.RegistrarWindow<VisualizarHoraExtraFaltas, VisualizarHoraExtraFaltasVM>();
            WindowService.RegistrarWindow<VisualizarDadosBancarios, VisualizarDadosBancariosVM>();

            WindowService.RegistrarWindow<SalvarFornecedor, CadastrarFornecedorManualmenteVM>();
            WindowService.RegistrarWindow<SalvarFornecedor, CadastrarFornecedorOnlineVM>();
            WindowService.RegistrarWindow<SalvarFornecedor, EditarFornecedorVM>();

            WindowService.RegistrarWindow<SalvarFuncionario, CadastrarFuncionarioVM>();
            WindowService.RegistrarWindow<SalvarFuncionario, EditarFuncionarioVM>();

            WindowService.RegistrarWindow<CadastrarGrade, CadastrarGradeVM>();

            WindowService.RegistrarWindow<SalvarLoja, CadastrarLojaVM>();
            WindowService.RegistrarWindow<SalvarLoja, EditarLojaVM>();

            WindowService.RegistrarWindow<CadastrarMarca, CadastrarMarcaVM>();

            WindowService.RegistrarWindow<SalvarProduto, CadastrarProdutoVM>();
            WindowService.RegistrarWindow<SalvarProduto, EditarProdutoVM>();

            WindowService.RegistrarWindow<CadastrarRecebimentoCartao, CadastrarRecebimentoVM>();
            WindowService.RegistrarWindow<View.RecebimentoCartao.MaisDetalhes, RecebimentoCartao.MaisDetalhesVM>();

            WindowService.RegistrarWindow<SalvarRepresentante, CadastrarRepresentanteVM>();
            WindowService.RegistrarWindow<SalvarRepresentante, EditarRepresentanteVM>();

            WindowService.RegistrarWindow<CadastrarTipoGrade, CadastrarTipoGradeVM>();

            WindowService.RegistrarWindow<TelaDeAviso, TelaDeAvisoVM>();

            WindowService.RegistrarWindow<RegistrarPonto, RegistrarPontoVM>();

            WindowService.RegistrarWindow<TelaDeLoginSimples, InserirSenhaPontoVM>();

            WindowService.RegistrarWindow<SalvarEntradaDeMercadoria, EditarEntradaDeMercadoriaVM>();

            WindowService.RegistrarWindow<TrocarSenhaFuncionario, TrocarSenhaFuncionarioVM>();

            WindowService.RegistrarWindow<PesquisarRepresentante, PesquisarRepresentanteVM>();

            WindowService.RegistrarWindow<ConsolidarPontosEletronicos, ConsolidarPontosEletronicosVM>();

            WindowService.RegistrarWindow<ConfirmarConsolidacaoPontosEletronicos, ConfirmarConsolidacaoPontosEletronicosVM>();

            WindowService.RegistrarWindow<ApresentaQRCodePix, ApresentaQRCodePixVM>();

            WindowService.RegistrarWindow<ConfigurarCredenciaisPix, ConfiguraCredenciaisPixVM>();

            WindowService.RegistrarWindow<MaisDetalhesPix, MaisDetalhesPixVM>();

            WindowService.RegistrarWindow<ListarCobrancasPix, ListarCobrancasPixVM>();

            WindowService.RegistrarWindow<TelaExportarImprimirPontoEletronico, TelaExportarImprimirPontoEletronicoVM>();

            WindowService.RegistrarWindow<PesquisarVendaEmCartao, PesquisarVendaEmCartaoVM>();
            WindowService.RegistrarWindow<CadastrarVendaEmCartao, CadastrarVendaEmCartaoVM>();

            WindowService.RegistrarWindow<PesquisarEntradasPorFornecedor, PesquisarEntradasPorFornecedorVM>();

            WindowService.RegistrarWindow<PesquisaParcelaCartao, PesquisaParcelaCartaoVM>();
            
            WindowService.RegistrarWindow<VisualizarMargensDeLucro, VisualizarMargensDeLucroVM>();

            try
            {
                SessionProvider.MainSessionFactory = SessionProvider.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            ResourceDictionary resourceDictionary = new ResourceDictionary();

            switch (CultureInfo.CurrentCulture.Name)
            {
                case "pt-BR":
                    resourceDictionary.Source = new Uri(@"..\Resources\Linguagem\PT-BR.xaml", UriKind.Relative);
                    break;
                case "en-US":
                    resourceDictionary.Source = new Uri(@"..\Resources\Linguagem\EN-US.xaml", UriKind.Relative);
                    break;
                default:
                    resourceDictionary.Source = new Uri(@"..\Resources\Linguagem\EN-US.xaml", UriKind.Relative);
                    break;
            }

            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }
    }
}