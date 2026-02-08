using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lava_Car.Cadastros.Pedidos.Classes
{
    public class Classe_Pedidos
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public int Id_Cliente { get; set; }
        public string Veiculo { get; set; }
        public string Placa { get; set; }
        public string Servico { get; set; }
        public DateTime Data { get; set; }
        public DateTime Data_Alteracao { get; set; }
        public decimal Valor { get; set; }
        public string Avarias { get; set; }
        public string Observacao { get; set; }
        public string Forma_Pagamento { get; set; }
        public string Situacao { get; set; }
        public bool Excluido { get; set; }
        public bool PossuiImagem { get; set; }
    }
}
