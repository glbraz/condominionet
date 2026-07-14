using System;
using System.Collections.Generic;

namespace CondomínioNet.Views
{
    
    // Camada View para Morador. Responsável exclusivamente pela interface com o usuário.
    // Herda de Tela e encapsula toda a lógica de apresentação.
    
    internal class VisãoMorador : Tela
    {
        private int _coluna;
        private int _linha;
        private int _largura;
        private int _altura;
        private List<string> _campos;

        public VisãoMorador(int coluna, int linha, Tela tela) : base()
        {
            _coluna = coluna;
            _linha = linha;

            _campos = new List<string>();
            _campos.Add("CPF            : ");
            _campos.Add("Nome           : ");
            _campos.Add("Telefone       : ");
            _campos.Add("Código Unidade : ");

            _largura = _campos[0].Length + 2 + 30;
            _altura = _campos.Count + 2 + 1;
        }

        
        // Desenha o formulário de cadastro de moradores.
        
        public void MostrarFormulario()
        {
            MontarMoldura(_coluna, _linha, _coluna + _largura, _linha + _altura);

            int linha = _linha + 1;
            Centralizar(_coluna, _coluna + _largura, linha, "Cadastro de Moradores");

            linha++;
            foreach (string campo in _campos)
            {
                Console.SetCursorPosition(_coluna + 1, linha);
                Console.Write(campo);
                linha++;
            }
        }

        
        // Captura o CPF do morador (chave primária).
        
        public string CapturarCPF()
        {
            int col = _coluna + 1 + _campos[0].Length;
            int lin = _linha + 2;
            Console.SetCursorPosition(col, lin);
            return Console.ReadLine() ?? "";
        }

        
        // Captura os dados do morador (exceto CPF).
        
        public (string nome, string telefone, string codigoUnidade) CapturarDados()
        {
            int col = _coluna + 1 + _campos[0].Length;
            int lin = _linha + 3;

            LimparArea(col, lin, _coluna + _largura - 2, lin + _altura - 5);

            Console.SetCursorPosition(col, lin);
            string nome = Console.ReadLine() ?? "";

            lin++;
            Console.SetCursorPosition(col, lin);
            string telefone = Console.ReadLine() ?? "";

            lin++;
            Console.SetCursorPosition(col, lin);
            string codigoUnidade = Console.ReadLine() ?? "";

            return (nome, telefone, codigoUnidade);
        }

        
        // Exibe os dados do morador encontrado.
        
        public void ExibirMorador(string nome, string telefone, string codigoUnidade)
        {
            int col = _coluna + 1 + _campos[0].Length;
            int lin = _linha + 3;

            Console.SetCursorPosition(col, lin);
            Console.Write(nome);

            lin++;
            Console.SetCursorPosition(col, lin);
            Console.Write(telefone);

            lin++;
            Console.SetCursorPosition(col, lin);
            Console.Write(codigoUnidade);
        }

        public int Coluna => _coluna;
        public int Linha => _linha;
        public int Largura => _largura;
        public int Altura => _altura;
    }
}
