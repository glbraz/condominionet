using System;
using System.Collections.Generic;

namespace CondomínioNet.Views
{
    
    // Camada View para Ocorrência. Responsável exclusivamente pela interface com o usuário.
    // Inclui os formulários de CRUD e os relatórios.
    
    internal class VisãoOcorrencia : Tela
    {
        private int _coluna;
        private int _linha;
        private int _largura;
        private int _altura;
        private List<string> _campos;

        private const int RPT_COL = 1;
        private const int RPT_ROW = 4;
        private const int RPT_CF = 78;
        private const int RPT_LF = 22;
        private const int RPT_ROW_TITLE = RPT_ROW + 1;
        private const int RPT_ROW_HEADER = RPT_ROW + 2;
        private const int RPT_ROW_DATA = RPT_ROW + 3;
        private const int RPT_ROW_LAST = RPT_LF - 1;
        private const int RPT_ROW_PROMPT = RPT_LF + 1;

        private const int RPT_COL_ID = 3;
        private const int RPT_COL_DESC = 15;
        private const int RPT_COL_LOCAL = 40;
        private const int RPT_COL_DATA = 56;
        private const int RPT_COL_STATUS = 68;

        public VisãoOcorrencia(int coluna, int linha, Tela tela) : base()
        {
            _coluna = coluna;
            _linha = linha;

            _campos = new List<string>();
            _campos.Add("ID             : ");
            _campos.Add("Descrição      : ");
            _campos.Add("Local          : ");
            _campos.Add("Data           : ");
            _campos.Add("CPF Morador    : ");
            _campos.Add("Status         : ");

            _largura = _campos[0].Length + 2 + 35;
            _altura = _campos.Count + 2 + 1;
        }

        
        // Desenha o formulário de cadastro de ocorrências.
        
        public void MostrarFormulario()
        {
            MontarMoldura(_coluna, _linha, _coluna + _largura, _linha + _altura);

            int linha = _linha + 1;
            Centralizar(_coluna, _coluna + _largura, linha, "Cadastro de Ocorrências");

            linha++;
            foreach (string campo in _campos)
            {
                Console.SetCursorPosition(_coluna + 1, linha);
                Console.Write(campo);
                linha++;
            }
        }

        
        // Captura o ID da ocorrência (chave primária).
        
        public string CapturarID()
        {
            int col = _coluna + 1 + _campos[0].Length;
            int lin = _linha + 2;
            Console.SetCursorPosition(col, lin);
            return Console.ReadLine() ?? "";
        }

        
        // Captura os dados da ocorrência (exceto ID).
        
        public (string descricao, string local, DateTime data, string cpfMorador, string status) CapturarDados()
        {
            int col = _coluna + 1 + _campos[0].Length;
            int lin = _linha + 3;

            LimparArea(col, lin, _coluna + _largura - 2, lin + _altura - 5);

            Console.SetCursorPosition(col, lin);
            string descricao = Console.ReadLine() ?? "";

            lin++;
            Console.SetCursorPosition(col, lin);
            string local = Console.ReadLine() ?? "";

            lin++;
            Console.SetCursorPosition(col, lin);
            DateTime data = ValidarData();

            lin++;
            Console.SetCursorPosition(col, lin);
            string cpfMorador = Console.ReadLine() ?? "";

            lin++;
            Console.SetCursorPosition(col, lin);
            Console.Write("[Aberta/Em Atendimento/Resolvida]");
            lin++;
            Console.SetCursorPosition(col, lin);
            string status = Console.ReadLine() ?? "Aberta";

            return (descricao, local, data, cpfMorador, status);
        }

        
        // Exibe os dados da ocorrência encontrada.
        
        public void ExibirOcorrencia(string descricao, string local, DateTime data, string cpfMorador, string status)
        {
            int col = _coluna + 1 + _campos[0].Length;
            int lin = _linha + 3;

            Console.SetCursorPosition(col, lin);
            Console.Write(descricao);

            lin++;
            Console.SetCursorPosition(col, lin);
            Console.Write(local);

            lin++;
            Console.SetCursorPosition(col, lin);
            Console.Write(data.ToString("dd/MM/yyyy"));

            lin++;
            Console.SetCursorPosition(col, lin);
            Console.Write(cpfMorador);

            lin++;
            Console.SetCursorPosition(col, lin);
            Console.Write(status);
        }

        
        // Desenha o cabeçalho de um relatório com moldura.
        
        public void MostrarFormularioRelatorio(string titulo)
        {
            MontarMoldura(RPT_COL, RPT_ROW, RPT_CF, RPT_LF);
            Centralizar(RPT_COL, RPT_CF, RPT_ROW_TITLE, titulo);

            Console.SetCursorPosition(RPT_COL_ID, RPT_ROW_HEADER);
            Console.Write("ID");
            Console.SetCursorPosition(RPT_COL_DESC, RPT_ROW_HEADER);
            Console.Write("Descrição");
            Console.SetCursorPosition(RPT_COL_LOCAL, RPT_ROW_HEADER);
            Console.Write("Local");
            Console.SetCursorPosition(RPT_COL_DATA, RPT_ROW_HEADER);
            Console.Write("Data");
            Console.SetCursorPosition(RPT_COL_STATUS, RPT_ROW_HEADER);
            Console.Write("Status");
        }

        
        // Exibe uma linha do relatório com dados de uma ocorrência.
        
        public void MostrarLinhaRelatorio(int linha, string id, string descricao, string local, DateTime data, string status)
        {
            string descTruncada = TruncarTexto(descricao, RPT_COL_LOCAL - RPT_COL_DESC - 1);
            string localTruncada = TruncarTexto(local, RPT_COL_DATA - RPT_COL_LOCAL - 1);

            Console.SetCursorPosition(RPT_COL_ID, linha);
            Console.Write(id);
            Console.SetCursorPosition(RPT_COL_DESC, linha);
            Console.Write(descTruncada);
            Console.SetCursorPosition(RPT_COL_LOCAL, linha);
            Console.Write(localTruncada);
            Console.SetCursorPosition(RPT_COL_DATA, linha);
            Console.Write(data.ToString("dd/MM/yyyy"));
            Console.SetCursorPosition(RPT_COL_STATUS, linha);
            Console.Write(status);
        }

        public int Coluna => _coluna;
        public int Linha => _linha;
        public int Largura => _largura;
        public int Altura => _altura;
        public int RPTRowData => RPT_ROW_DATA;
        public int RPTRowLast => RPT_ROW_LAST;
        public int RPTRowPrompt => RPT_ROW_PROMPT;
        public int RPTColIni => RPT_COL + 1;
        public int RPTColFim => RPT_CF - 1;

        
        // Valida entrada de data.
        
        private DateTime ValidarData()
        {
            string entrada = Console.ReadLine() ?? "";
            if (DateTime.TryParse(entrada, out DateTime resultado))
                return resultado;
            return DateTime.Today;
        }

        
        // Trunca um texto para não ultrapassar a largura máxima.
        
        private string TruncarTexto(string texto, int tamanhoMaximo)
        {
            if (texto.Length <= tamanhoMaximo)
                return texto;
            return texto.Substring(0, tamanhoMaximo - 1) + "…";
        }
    }
}
