using System;
using System.Collections.Generic;
using CondomínioNet.Controllers;

namespace CondomínioNet
{
    
    // Programa principal do CondomínioNet.
    // Responsável por instanciar os controllers, gerenciar o menu principal
    // e orquestrar o fluxo do sistema.
    
    internal class Program
    {
        static void Main(string[] args)
        {
            // Inicializa a interface
            Tela tela = new Tela(ConsoleColor.Cyan, ConsoleColor.Black);

            // Instancia os controllers
            UnitController UnitController = new UnitController(10, 5, tela);
            ResidentController ResidentController = new ResidentController(10, 3, tela, UnitController);
            OccurrenceController OccurrenceController = new OccurrenceController(10, 5, tela,
                UnitController, ResidentController);

            // Carrega dados dos arquivos
            UnitController.CarregarDoArquivo("unidades.txt");
            ResidentController.CarregarDoArquivo("moradores.txt");
            OccurrenceController.CarregarDoArquivo("ocorrencias.txt");

            // Menu principal
            List<string> opcoes = new List<string>();
            opcoes.Add("1 - Unidades                   ");
            opcoes.Add("2 - Moradores                  ");
            opcoes.Add("3 - Ocorrências                ");
            opcoes.Add("4 - Ocorrências em Aberto      ");
            opcoes.Add("5 - Ocorrências por Unidade    ");
            opcoes.Add("0 - Sair                       ");

            string opcao = "";

            // Laço principal do programa
            while (true)
            {
                tela.PrepararTela("CondomínioNet - Sistema de Gestão de Condomínio", 0, 0, 79, 24);
                opcao = tela.MostrarMenu(2, 2, opcoes);

                // Processa a opção escolhida
                if (opcao == "0")
                    break;

                if (opcao == "1")
                    UnitController.CRUD();
                else if (opcao == "2")
                    ResidentController.CRUD();
                else if (opcao == "3")
                    OccurrenceController.CRUD();
                else if (opcao == "4")
                    OccurrenceController.RelatorioAbertos();
                else if (opcao == "5")
                    OccurrenceController.RelatorioPorUnidade();
            }

            // Salva os dados antes de encerrar
            UnitController.SalvarNoArquivo("unidades.txt");
            ResidentController.SalvarNoArquivo("moradores.txt");
            OccurrenceController.SalvarNoArquivo("ocorrencias.txt");

            Console.Clear();
            Console.WriteLine("Encerrando CondomínioNet. Até logo!");
        }
    }
}
