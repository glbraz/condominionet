using System;

namespace CondomínioNet.Models
{
    internal class ResidentModel
    {
        private string _cpf;
        private string _nome;
        private string _telefone;
        private string _codigoUnidade;

        public string Cpf
        {
            get { return _cpf; }
            set { _cpf = value; }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public string Telefone
        {
            get { return _telefone; }
            set { _telefone = value; }
        }

        public string CodigoUnidade
        {
            get { return _codigoUnidade; }
            set { _codigoUnidade = value; }
        }

        public ResidentModel(string cpf, string nome, string telefone, string codigoUnidade)
        {
            _cpf = cpf;
            _nome = nome;
            _telefone = telefone;
            _codigoUnidade = codigoUnidade;
        }

        public ResidentModel()
        {
            _cpf = "";
            _nome = "";
            _telefone = "";
            _codigoUnidade = "";
        }

        public string ToFileFormat()
        {
            return $"{_cpf}|{_nome}|{_telefone}|{_codigoUnidade}";
        }

        public static ResidentModel FromFileFormat(string linha)
        {
            string[] partes = linha.Split('|');
            if (partes.Length == 4)
            {
                return new ResidentModel(
                    partes[0],
                    partes[1],
                    partes[2],
                    partes[3]
                );
            }
            return null;
        }
    }
}
