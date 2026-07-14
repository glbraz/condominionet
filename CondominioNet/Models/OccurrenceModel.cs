using System;

namespace CondomínioNet.Models
{
    internal class Occurrence
    {
        private string _id;
        private string _descricao;
        private string _local;
        private DateTime _data;
        private string _cpfMorador;
        private string _status;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Descricao
        {
            get { return _descricao; }
            set { _descricao = value; }
        }

        public string Local
        {
            get { return _local; }
            set { _local = value; }
        }

        public DateTime Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public string CpfMorador
        {
            get { return _cpfMorador; }
            set { _cpfMorador = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public Occurrence(string id, string descricao, string local, DateTime data, string cpfMorador, string status)
        {
            _id = id;
            _descricao = descricao;
            _local = local;
            _data = data;
            _cpfMorador = cpfMorador;
            _status = status;
        }

        public Occurrence()
        {
            _id = "";
            _descricao = "";
            _local = "";
            _data = DateTime.Today;
            _cpfMorador = "";
            _status = "Aberta";
        }

        public string ToFileFormat()
        {
            return $"{_id}|{_descricao}|{_local}|{_data:dd/MM/yyyy}|{_cpfMorador}|{_status}";
        }

        public static Occurrence FromFileFormat(string linha)
        {
            string[] partes = linha.Split('|');
            if (partes.Length == 6)
            {
                return new Occurrence(
                    partes[0],
                    partes[1],
                    partes[2],
                    DateTime.Parse(partes[3]),
                    partes[4],
                    partes[5]
                );
            }
            return null;
        }
    }
}
