using System;
using System.Collections.Generic;
using System.IO;
using CondomínioNet.Models;
using CondomínioNet.Views;

namespace CondomínioNet.Controllers
{
    
    // Controller para gerenciar operações com Moradores.
    // Herda de BaseCRUD e implementa a busca e persistência específica.
    // Usa a camada View para toda a interface com o usuário.
    // Valida se as unidades existem antes de adicionar um morador.
    
    internal class ResidentController : BaseCRUD<ResidentModel>
    {
        private VisãoMorador _visao;
        private UnitController _unidadeController;

        public ResidentController(int coluna, int linha, Tela tela, UnitController unidadeController) 
            : base(coluna, linha, tela)
        {
            _visao = new VisãoMorador(coluna, linha, tela);
            _unidadeController = unidadeController;

            // Dados pré-carregados para testes
            _registros.Add(new ResidentModel("123.456.789-00", "João Silva", "(11) 98765-4321", "A101"));
            _registros.Add(new ResidentModel("987.654.321-00", "Maria Santos", "(11) 97654-3210", "B205"));

            _largura = _visao.Largura;
            _altura = _visao.Altura;
        }

        protected override string ObterTitulo()
        {
            return "Cadastro de Moradores";
        }

        protected override string ObterChavePrimaria()
        {
            return "CPF";
        }

        
        // Busca um morador pelo CPF.
        
        public ResidentModel BuscarPorCPF(string cpf)
        {
            foreach (ResidentModel morador in _registros)
            {
                if (morador.Cpf == cpf)
                    return morador;
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
                _modeloAtual.Cpf = _visao.CapturarCPF();
            }
            else
            {
                var (nome, telefone, codigoUnidade) = _visao.CapturarDados();
                _modeloAtual.Nome = nome;
                _modeloAtual.Telefone = telefone;
                _modeloAtual.CodigoUnidade = codigoUnidade;
            }
        }

        protected override void ExibirDados()
        {
            ResidentModel morador = _registros[_posicao];
            _visao.ExibirMorador(morador.Nome, morador.Telefone, morador.CodigoUnidade);
        }

        protected override bool Buscar()
        {
            for (int i = 0; i < _registros.Count; i++)
            {
                if (_registros[i].Cpf == _modeloAtual.Cpf)
                {
                    _posicao = i;
                    return true;
                }
            }
            return false;
        }

        protected override void AtualizarRegistro()
        {
            _registros[_posicao].Nome = _modeloAtual.Nome;
            _registros[_posicao].Telefone = _modeloAtual.Telefone;
            _registros[_posicao].CodigoUnidade = _modeloAtual.CodigoUnidade;
        }

        protected override void AdicionarRegistro()
        {
            // Valida se a unidade existe
            if (_unidadeController.BuscarPorCodigo(_modeloAtual.CodigoUnidade) == null)
            {
                int colini = _coluna + 1;
                int colfin = _coluna + _largura - 1;
                int linha = _linha + _altura - 1;
                _tela.Perguntar("Unidade não existe! Pressione Enter.", linha, colini, colfin);
                return;
            }

            _registros.Add(new ResidentModel(
                _modeloAtual.Cpf,
                _modeloAtual.Nome,
                _modeloAtual.Telefone,
                _modeloAtual.CodigoUnidade
            ));
        }

        
        // Carrega moradores de um arquivo de texto.
        
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
                        ResidentModel morador = ResidentModel.FromFileFormat(linha);
                        if (morador != null)
                            _registros.Add(morador);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar moradores: {ex.Message}");
            }
        }

        
        // Salva moradores em um arquivo de texto.
        
        public void SalvarNoArquivo(string nomeArquivo)
        {
            try
            {
                using (StreamWriter escritor = new StreamWriter(nomeArquivo))
                {
                    foreach (ResidentModel morador in _registros)
                    {
                        escritor.WriteLine(morador.ToFileFormat());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar moradores: {ex.Message}");
            }
        }
    }
}
