using System;
using System.Collections.Generic;
using System.Linq;

namespace CondomínioNet
{
    
    // Classe responsável por toda a interface visual com o usuário no console.
    // Fornece métodos para desenhar molduras, centralizar textos, capturar entradas e exibir menus.
    
    internal class Tela
    {
        private ConsoleColor _corFundo;
        private ConsoleColor _corTexto;

        public Tela(ConsoleColor corFundo, ConsoleColor corTexto)
        {
            _corFundo = corFundo;
            _corTexto = corTexto;
        }

        public Tela() { }

        
        // Limpa a tela, define cores e desenha uma moldura com o título.
        
        public void PrepararTela(string titulo, int colunaInicial, int linhaInicial, int colunaFinal, int linhaFinal)
        {
            Console.BackgroundColor = _corFundo;
            Console.ForegroundColor = _corTexto;
            Console.Clear();
            MontarMoldura(colunaInicial, linhaInicial, colunaFinal, linhaFinal);
            Centralizar(colunaInicial, colunaFinal, linhaInicial + 1, titulo);
        }

        
        // Centraliza um texto horizontal na tela entre duas colunas.
        
        public void Centralizar(int colunaInicial, int colunaFinal, int linha, string texto)
        {
            int coluna = colunaInicial + ((colunaFinal - colunaInicial - texto.Length) / 2);
            Console.SetCursorPosition(coluna, linha);
            Console.Write(texto);
        }

        
        // Exibe uma pergunta e retorna a resposta do usuário em minúsculas.
        
        public string Perguntar(string pergunta, int linha, int colunaInicial, int colunaFinal)
        {
            LimparArea(colunaInicial, linha, colunaFinal, linha);
            Console.SetCursorPosition(colunaInicial, linha);
            Console.Write(pergunta);
            return Console.ReadLine()?.ToLower() ?? "";
        }

        
        // Limpa toda a área dentro de um retângulo da tela.
        
        public void LimparArea(int colunaInicial, int linhaInicial, int colunaFinal, int linhaFinal)
        {
            for (int coluna = colunaInicial; coluna <= colunaFinal; coluna++)
            {
                for (int linha = linhaInicial; linha <= linhaFinal; linha++)
                {
                    Console.SetCursorPosition(coluna, linha);
                    Console.Write(" ");
                }
            }
        }

        
        // Desenha uma moldura retangular usando caracteres especiais.
        
        public void MontarMoldura(int colunaInicial, int linhaInicial, int colunaFinal, int linhaFinal)
        {
            LimparArea(colunaInicial, linhaInicial, colunaFinal, linhaFinal);

            // Linhas horizontais
            for (int coluna = colunaInicial; coluna <= colunaFinal; coluna++)
            {
                Console.SetCursorPosition(coluna, linhaInicial);
                Console.Write('═');
                Console.SetCursorPosition(coluna, linhaFinal);
                Console.Write('═');
            }

            // Linhas verticais
            for (int linha = linhaInicial; linha <= linhaFinal; linha++)
            {
                Console.SetCursorPosition(colunaInicial, linha);
                Console.Write('║');
                Console.SetCursorPosition(colunaFinal, linha);
                Console.Write('║');
            }

            // Cantos
            Console.SetCursorPosition(colunaInicial, linhaInicial);
            Console.Write('╔');
            Console.SetCursorPosition(colunaFinal, linhaInicial);
            Console.Write('╗');
            Console.SetCursorPosition(colunaInicial, linhaFinal);
            Console.Write('╚');
            Console.SetCursorPosition(colunaFinal, linhaFinal);
            Console.Write('╝');
        }

        
        // Exibe um menu com opções e retorna a opção escolhida.
        
        public string MostrarMenu(int colunaInicial, int linhaInicial, List<string> opcoes)
        {
            int colunaFinal = colunaInicial + opcoes[0].Length + 1;
            int linhaFinal = linhaInicial + opcoes.Count + 2;

            MontarMoldura(colunaInicial, linhaInicial, colunaFinal, linhaFinal);
            
            for (int i = 0; i < opcoes.Count; i++)
            {
                Console.SetCursorPosition(colunaInicial + 1, linhaInicial + 1 + i);
                Console.Write(opcoes[i]);
            }
            
            Console.SetCursorPosition(colunaInicial + 1, linhaInicial + 1 + opcoes.Count);
            Console.Write("Opção: ");
            return Console.ReadLine() ?? "";
        }
    }
}
