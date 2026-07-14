using System;

namespace CondomínioNet.Models
{
    internal class UnitModel
    {
        private string _codigo;
        private string _bloco;
        private int _numero;
        private string _tipo;
        private double _area;

        public string Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        public string Bloco
        {
            get { return _bloco; }
            set { _bloco = value; }
        }

        public int Numero
        {
            get { return _numero; }
            set { _numero = value; }
        }

        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        public double Area
        {
            get { return _area; }
            set { _area = value; }
        }

        public UnitModel(string codigo, string bloco, int numero, string tipo, double area)
        {
            _codigo = codigo;
            _bloco = bloco;
            _numero = numero;
            _tipo = tipo;
            _area = area;
        }

        public UnitModel()
        {
            _codigo = "";
            _bloco = "";
            _numero = 0;
            _tipo = "";
            _area = 0.0;
        }

        public string ToFileFormat()
        {
            return $"{_codigo}|{_bloco}|{_numero}|{_tipo}|{_area}";
        }

        public static UnitModel FromFileFormat(string linha)
        {
            string[] partes = linha.Split('|');
            if (partes.Length == 5)
            {
                return new UnitModel(
                    partes[0],
                    partes[1],
                    int.Parse(partes[2]),
                    partes[3],
                    double.Parse(partes[4])
                );
            }
            return null;
        }
    }
}
