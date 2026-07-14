using System;
using System.Collections.Generic;
using System.IO;
using CondomínioNet.Models;
using CondomínioNet.Views;

namespace CondomínioNet.Controllers
{
    
    // Controller para gerenciar operações com Ocorrências.
    // Herda de BaseCRUD e implementa a busca e persistência específica.
    // Inclui dois relatórios: ocorrências em aberto e por unidade.
    // Usa a camada View para toda a interface com o usuário.
    
    internal class OccurrenceController : BaseCRUD<Occurrence>
    {
        private VisãoOcorrencia _visao;
        private UnitController _unidadeController;
        private ResidentController _moradorController;

        public OccurrenceController(int coluna, int linha, Tela tela,
            UnitController unidadeController, ResidentController moradorController)
            : base(coluna, linha, tela)
        {
            _visao = new VisãoOcorrencia(coluna, linha, tela);
            _unidadeController = unidadeController;
            _moradorController = moradorController;

            // Dados pré-carregados para testes
            // Nota: Os CPFs usados aqui DEVEM existir em ResidentController
            _registros.Add(new Occurrence("OC001", "Vazamento de água", "Cozinha",
                DateTime.Parse("20/06/2026"), "123.456.789-00", "Aberta"));
            _registros.Add(new Occurrence("OC002", "Porta quebrada", "Entrada",
                DateTime.Parse("21/06/2026"), "987.654.321-00", "Em Atendimento"));
            _registros.Add(new Occurrence("OC003", "Ar-condicionado quebrado", "Sala",
                DateTime.Parse("22/06/2026"), "123.456.789-00", "Em Atendimento"));

            _largura = _visao.Largura;
            _altura = _visao.Altura;
        }

        protected override string ObterTitulo()
        {
            return "Cadastro de Ocorrências";
        }

        protected override string ObterChavePrimaria()
        {
            return "ID";
        }

        
        // Busca uma ocorrência pelo ID.
        
        public Occurrence BuscarPorID(string id)
        {
            foreach (Occurrence ocorrencia in _registros)
            {
                if (ocorrencia.Id == id)
                    return ocorrencia;
            }
            return null;
        }

        protected override void MostrarFormulario()
        {
            _visao.MostrarFormulario();
        }

        protected override void EntrarDados(string which)
        {
            if (which == "PK")
            {
                _modeloAtual.Id = _visao.CapturarID();
            }
            else
            {
                var (descricao, local, data, cpfMorador, status) = _visao.CapturarDados();
                _modeloAtual.Descricao = descricao;
                _modeloAtual.Local = local;
                _modeloAtual.Data = data;
                _modeloAtual.CpfMorador = cpfMorador;
                _modeloAtual.Status = status;
            }
        }

        protected override void ExibirDados()
        {
            Occurrence ocorrencia = _registros[_posicao];
            _visao.ExibirOcorrencia(ocorrencia.Descricao, ocorrencia.Local,
                ocorrencia.Data, ocorrencia.CpfMorador, ocorrencia.Status);
        }

        protected override bool Buscar()
        {
            for (int i = 0; i < _registros.Count; i++)
            {
                if (_registros[i].Id == _modeloAtual.Id)
                {
                    _posicao = i;
                    return true;
                }
            }
            return false;
        }

        protected override void AtualizarRegistro()
        {
            _registros[_posicao].Descricao = _modeloAtual.Descricao;
            _registros[_posicao].Local = _modeloAtual.Local;
            _registros[_posicao].Data = _modeloAtual.Data;
            _registros[_posicao].CpfMorador = _modeloAtual.CpfMorador;
            _registros[_posicao].Status = _modeloAtual.Status;
        }

        protected override void AdicionarRegistro()
        {
            _registros.Add(new Occurrence(
                _modeloAtual.Id,
                _modeloAtual.Descricao,
                _modeloAtual.Local,
                _modeloAtual.Data,
                _modeloAtual.CpfMorador,
                _modeloAtual.Status
            ));
        }


        // Relatório de ocorrências em aberto ou em atendimento.

        public void RelatorioAbertos()
        {
            _visao.MostrarFormularioRelatorio("Ocorrências em Aberto");

            int linha = _visao.RPTRowData;
            int quantidade = 0;

            foreach (Occurrence ocorrencia in _registros)
            {
                if (ocorrencia.Status == "Aberta" ||
                    ocorrencia.Status == "Em Atendimento")
                {
                    if (linha <= _visao.RPTRowLast)
                    {
                        _visao.MostrarLinhaRelatorio(
                            linha,
                            ocorrencia.Id,
                            ocorrencia.Descricao,
                            ocorrencia.Local,
                            ocorrencia.Data,
                            ocorrencia.Status);

                        linha++;
                        quantidade++;
                    }
                }
            }

            if (quantidade == 0)
            {
                Console.SetCursorPosition(_visao.RPTColIni, _visao.RPTRowData);
                Console.Write("Nenhuma ocorrência em aberto");
            }

            _tela.Perguntar(
                "Pressione Enter para voltar: ",
                _visao.RPTRowPrompt,
                _visao.RPTColIni,
                _visao.RPTColFim);
        }


        // Relatório de ocorrências de uma unidade específica.

        public void RelatorioPorUnidade()
        {
            _tela.PrepararTela("CondomínioNet - Ocorrências por Unidade", 0, 0, 79, 24);

            string codigoUnidade = _tela.Perguntar(
                "Digite o código da unidade: ",
                10,
                2,
                78);

            if (_unidadeController.BuscarPorCodigo(codigoUnidade) == null)
            {
                _tela.Perguntar("Unidade não encontrada! Pressione Enter.", 12, 2, 78);
                return;
            }

            _visao.MostrarFormularioRelatorio(
                $"Ocorrências da Unidade {codigoUnidade}");

            int linha = _visao.RPTRowData;
            int quantidade = 0;

            foreach (Occurrence ocorrencia in _registros)
            {
                ResidentModel morador =
                    _moradorController.BuscarPorCPF(ocorrencia.CpfMorador);

                if (morador != null &&
                    morador.CodigoUnidade.Trim().ToUpper() ==
                    codigoUnidade.Trim().ToUpper())
                {
                    if (linha <= _visao.RPTRowLast)
                    {
                        _visao.MostrarLinhaRelatorio(
                            linha,
                            ocorrencia.Id,
                            ocorrencia.Descricao,
                            ocorrencia.Local,
                            ocorrencia.Data,
                            ocorrencia.Status);

                        linha++;
                        quantidade++;
                    }
                }
            }

            if (quantidade == 0)
            {
                Console.SetCursorPosition(_visao.RPTColIni, _visao.RPTRowData);
                Console.Write("Nenhuma ocorrência encontrada para esta unidade");
            }

            _tela.Perguntar(
                "Pressione Enter para voltar: ",
                _visao.RPTRowPrompt,
                _visao.RPTColIni,
                _visao.RPTColFim);
        }


        // Carrega ocorrências de um arquivo de texto.

        public void CarregarDoArquivo(string nomeArquivo)
        {
            if (!File.Exists(nomeArquivo))
                return;

            try
            {
                using (StreamReader leitor = new StreamReader(nomeArquivo))
                {
                    string linha;
                    while ((linha = leitor.ReadLine()) != null)
                    {
                        Occurrence ocorrencia = Occurrence.FromFileFormat(linha);
                        if (ocorrencia != null)
                            _registros.Add(ocorrencia);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar ocorrências: {ex.Message}");
            }
        }

        
        // Salva ocorrências em um arquivo de texto.
        
        public void SalvarNoArquivo(string nomeArquivo)
        {
            try
            {
                using (StreamWriter escritor = new StreamWriter(nomeArquivo))
                {
                    foreach (Occurrence ocorrencia in _registros)
                    {
                        escritor.WriteLine(ocorrencia.ToFileFormat());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar ocorrências: {ex.Message}");
            }
        }
    }
}
