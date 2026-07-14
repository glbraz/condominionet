using System;
using System.Collections.Generic;

namespace CondomínioNet.Controllers
{
    
    // Classe base genérica que implementa o padrão CRUD padrão para qualquer modelo.
    // Reduz significativamente a repetição de código entre os controllers.
    // Todos os controllers herdam dessa classe e ganham o fluxo CRUD automático.
    
    internal abstract class BaseCRUD<T> where T : class, new()
    {
        protected int _coluna;
        protected int _linha;
        protected int _largura;
        protected int _altura;
        protected int _posicao;
        protected List<string> _campos;
        protected List<T> _registros;
        protected Tela _tela;
        protected T _modeloAtual;

        protected BaseCRUD(int coluna, int linha, Tela tela)
        {
            _coluna = coluna;
            _linha = linha;
            _tela = tela;
            _registros = new List<T>();
            _campos = new List<string>();
            _modeloAtual = new T();
        }

        
        // Retorna o título que será exibido no formulário.
        // Deve ser sobrescrito pela classe derivada.
        
        protected abstract string ObterTitulo();

        
        // Desenha o formulário com moldura e rótulos dos campos.
        
        protected virtual void MostrarFormulario()
        {
            _tela.MontarMoldura(_coluna, _linha,
                _coluna + _largura, _linha + _altura);

            int linha = _linha + 1;
            _tela.Centralizar(_coluna, _coluna + _largura, linha, ObterTitulo());

            linha++;
            foreach (string campo in _campos)
            {
                Console.SetCursorPosition(_coluna + 1, linha);
                Console.Write(campo);
                linha++;
            }
        }

        
        // Captura dados do usuário. Parametro "PK" para chave primária, "DT" para dados.
        // Deve ser sobrescrito pela classe derivada para tratamento específico.
        
        protected abstract void EntrarDados(string which);

        
        // Exibe os dados do registro encontrado.
        // Deve ser sobrescrito pela classe derivada.
        
        protected abstract void ExibirDados();

        
        // Fluxo completo do CRUD: busca → exibe → oferece menu.
        
        public virtual void CRUD()
        {
            MostrarFormulario();
            EntrarDados("PK");

            bool encontrado = Buscar();

            if (encontrado)
            {
                ExibirDados();
                ProcessarOpcoesCRUD();
            }
            else
            {
                ProcessarNaoEncontrado();
            }
        }

        
        // Método abstrato para buscar o registro.
        // Deve ser implementado pela classe derivada.
        
        protected abstract bool Buscar();

        
        // Menu de opções quando o registro é encontrado.
        
        protected virtual void ProcessarOpcoesCRUD()
        {
            int colini = _coluna + 1;
            int colfin = _coluna + _largura - 1;
            int linha = _linha + _altura - 1;

            string resposta = _tela.Perguntar("Deseja alterar/excluir/voltar (A/E/V): ",
                linha, colini, colfin);

            if (resposta == "a")
                Alterar(colini, colfin, linha);
            else if (resposta == "e")
                Excluir(colini, colfin, linha);
        }

        
        // Altera um registro existente.
        
        protected virtual void Alterar(int colini, int colfin, int linha)
        {
            EntrarDados("DT");
            string resposta = _tela.Perguntar("Confirma alteração (S/N): ", linha, colini, colfin);
            if (resposta == "s")
            {
                AtualizarRegistro();
            }
        }

        
        // Exclui um registro existente.
        
        protected virtual void Excluir(int colini, int colfin, int linha)
        {
            string resposta = _tela.Perguntar("Confirma exclusão (S/N): ", linha, colini, colfin);
            if (resposta == "s")
            {
                _registros.RemoveAt(_posicao);
            }
        }

        
        // Menu quando o registro não é encontrado.
        
        protected virtual void ProcessarNaoEncontrado()
        {
            int colini = _coluna + 1;
            int colfin = _coluna + _largura - 1;
            int linha = _linha + _altura - 1;

            string chavePrimaria = ObterChavePrimaria();
            string resposta = _tela.Perguntar($"{chavePrimaria} não encontrado. Deseja cadastrar (S/N): ",
                linha, colini, colfin);

            if (resposta == "s")
            {
                EntrarDados("DT");
                resposta = _tela.Perguntar("Confirma cadastro (S/N): ", linha, colini, colfin);
                if (resposta == "s")
                {
                    AdicionarRegistro();
                }
            }
        }

        
        // Retorna o nome da chave primária (para mensagens).
        // Deve ser sobrescrito pela classe derivada.
        
        protected abstract string ObterChavePrimaria();

        
        // Atualiza o registro com os novos dados.
        // Deve ser sobrescrito pela classe derivada.
        
        protected abstract void AtualizarRegistro();

        
        // Adiciona um novo registro à lista.
        // Deve ser sobrescrito pela classe derivada.
        
        protected abstract void AdicionarRegistro();

        public List<T> Registros => _registros;
    }
}
