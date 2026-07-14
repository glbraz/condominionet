using System;
using System.Collections.Generic;

namespace CondomínioNet.Views
{
    
    // Camada View para Unidade. Responsável exclusivamente pela interface com o usuário.
    // Herda de Tela e encapsula toda a lógica de apresentação.
    
    internal class VisãoUnidade : Tela
    {
        private int _coluna;
        private int _linha;
        private int _largura;
        private int _altura;
        private List<string> _campos;

        public VisãoUnidade(int coluna, int linha, Tela tela) : base()
        {
            _coluna = coluna;
            _linha = linha;

            _campos = new List<string>();
            _campos.Add("Código         : ");
            _campos.Add("Bloco          : ");
            _campos.Add("Número         : ");
            _campos.Add("Tipo           : ");
            _campos.Add("Área (m²)      : ");

            _largura = _campos[0].Length + 2 + 30;
            _altura = _campos.Count + 2 + 1;
        }

        
        // Desenha o formulário de cadastro de unidades.
        
        public void MostrarFormulario()
        {
            MontarMoldura(_coluna, _linha, _coluna + _largura, _linha + _altura);

            int linha = _linha + 1;
            Centralizar(_coluna, _coluna + _largura, linha, "Cadastro de Unidades");

            linha++;
            foreach (string campo in _campos)
            {
                Console.SetCursorPosition(_coluna + 1, linha);
                Console.Write(campo);
                linha++;
            }
        }

        
        // Captura o código da unidade (chave primária).
        
        public string CapturarCodigo()
        {
            int col = _coluna + 1 + _campos[0].Length;
            int lin = _linha + 2;
            Console.SetCursorPosition(col, lin);
            return Console.ReadLine() ?? "";
        }

        
        // Captura os dados da unidade (exceto código).
        
        public (string bloco, int numero, string tipo, double area) CapturarDados()
        {
            int col = _coluna + 1 + _campos[0].Length;
            int lin = _linha + 3;

            LimparArea(col, lin, _coluna + _largura - 2, lin + _altura - 5);

            Console.SetCursorPosition(col, lin);
            string bloco = Console.ReadLine() ?? "";

            lin++;
            Console.SetCursorPosition(col, lin);
            int numero = ValidarInteiro();

            lin++;
            Console.SetCursorPosition(col, lin);
            string tipo = Console.ReadLine() ?? "";

            lin++;
            Console.SetCursorPosition(col, lin);
            double area = ValidarDouble();

            return (bloco, numero, tipo, area);
        }

        
        // Exibe os dados da unidade encontrada.
        
        public void ExibirUnidade(string bloco, int numero, string tipo, double area)
        {
            int col = _coluna + 1 + _campos[0].Length;
            int lin = _linha + 3;

            Console.SetCursorPosition(col, lin);
            Console.Write(bloco);

            lin++;
            Console.SetCursorPosition(col, lin);
            Console.Write(numero);

            lin++;
            Console.SetCursorPosition(col, lin);
            Console.Write(tipo);

            lin++;
            Console.SetCursorPosition(col, lin);
            Console.Write(area);
        }

        public int Coluna => _coluna;
        public int Linha => _linha;
        public int Largura => _largura;
        public int Altura => _altura;

        
        // Valida entrada de inteiro.
        
        private int ValidarInteiro()
        {
            string entrada = Console.ReadLine() ?? "0";
            if (int.TryParse(entrada, out int resultado))
                return resultado;
            return 0;
        }

        
        // Valida entrada de double.
        
        private double ValidarDouble()
        {
            string entrada = Console.ReadLine() ?? "0";
            if (double.TryParse(entrada, out double resultado))
                return resultado;
            return 0.0;
        }
    }
}
