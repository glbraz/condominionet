using System;
using System.Collections.Generic;
using System.IO;
using CondomínioNet.Models;
using CondomínioNet.Views;

namespace CondomínioNet.Controllers
{
    
    // Controller para gerenciar operações com Unidades.
    // Herda de BaseCRUD e implementa a busca e persistência específica.
    // Usa a camada View para toda a interface com o usuário.
    
    internal class UnitController : BaseCRUD<UnitModel>
    {
        private VisãoUnidade _visao;

        public UnitController(int coluna, int linha, Tela tela) : base(coluna, linha, tela)
        {
            _visao = new VisãoUnidade(coluna, linha, tela);

            // Dados pré-carregados para testes
            _registros.Add(new UnitModel("A101", "A", 101, "Apartamento", 75.5));
            _registros.Add(new UnitModel("B205", "B", 205, "Apartamento", 85.0));
            _registros.Add(new UnitModel("B301", "B", 301, "Cobertura", 150.0));

            _largura = _visao.Largura;
            _altura = _visao.Altura;
        }

        protected override string ObterTitulo()
        {
            return "Cadastro de Unidades";
        }

        protected override string ObterChavePrimaria()
        {
            return "Código";
        }


        // Busca uma unidade pelo código.

        public UnitModel BuscarPorCodigo(string codigo)
        {
            codigo = codigo.Trim().ToUpper();

            foreach (UnitModel unidade in _registros)
            {
                if (unidade.Codigo.Trim().ToUpper() == codigo)
                    return unidade;
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
                _modeloAtual.Codigo = _visao.CapturarCodigo();
            }
            else
            {
                var (bloco, numero, tipo, area) = _visao.CapturarDados();
                _modeloAtual.Bloco = bloco;
                _modeloAtual.Numero = numero;
                _modeloAtual.Tipo = tipo;
                _modeloAtual.Area = area;
            }
        }

        protected override void ExibirDados()
        {
            UnitModel unidade = _registros[_posicao];
            _visao.ExibirUnidade(unidade.Bloco, unidade.Numero, unidade.Tipo, unidade.Area);
        }

        protected override bool Buscar()
        {
            string codigo = _modeloAtual.Codigo.Trim().ToUpper();

            for (int i = 0; i < _registros.Count; i++)
            {
                if (_registros[i].Codigo.Trim().ToUpper() == codigo)
                {
                    _posicao = i;
                    return true;
                }
            }

            return false;
        }

        protected override void AtualizarRegistro()
        {
            _registros[_posicao].Bloco = _modeloAtual.Bloco;
            _registros[_posicao].Numero = _modeloAtual.Numero;
            _registros[_posicao].Tipo = _modeloAtual.Tipo;
            _registros[_posicao].Area = _modeloAtual.Area;
        }

        protected override void AdicionarRegistro()
        {
            _registros.Add(new UnitModel(
                _modeloAtual.Codigo,
                _modeloAtual.Bloco,
                _modeloAtual.Numero,
                _modeloAtual.Tipo,
                _modeloAtual.Area
            ));
        }

        
        // Carrega unidades de um arquivo de texto.
        
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
                        UnitModel unidade = UnitModel.FromFileFormat(linha);
                        if (unidade != null)
                            _registros.Add(unidade);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar unidades: {ex.Message}");
            }
        }

        
        // Salva unidades em um arquivo de texto.
        
        public void SalvarNoArquivo(string nomeArquivo)
        {
            try
            {
                using (StreamWriter escritor = new StreamWriter(nomeArquivo))
                {
                    foreach (UnitModel unidade in _registros)
                    {
                        escritor.WriteLine(unidade.ToFileFormat());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar unidades: {ex.Message}");
            }
        }
        
    }
}
