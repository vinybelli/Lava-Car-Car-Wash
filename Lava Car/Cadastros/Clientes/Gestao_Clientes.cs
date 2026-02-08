using Lava_Car.Cadastros.Clientes.Classes;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lava_Car.Cadastros.Clientes
{
    public partial class Gestao_Clientes : Form
    {
        List<Classe_Clientes> Clientes = new List<Classe_Clientes>();

        public Gestao_Clientes()
        {
            InitializeComponent();

            AtualizarClientes();

            textBox1.Select();
        }

        public void AtualizarClientes()
        {
            Clientes.Clear();

            BuscarClientesBD();

            dataGridView5.Rows.Clear();

            int qntdClientes = 0;

            for (int i = 0; i < Clientes.Count; i++)
            {
                dataGridView5.Rows.Add(Clientes[i].Id,
                    Clientes[i].Nome_Razao,
                    Clientes[i].Telefone,
                    Clientes[i].Tipo_Cliente,
                    Clientes[i].Data.ToString("dd/MM/yyyy"));

                qntdClientes++;
            }

            label8.Text = qntdClientes.ToString();

            if(Clientes.Count > 0)
            {
                button4.Visible = true;
                button10.Visible = true;
            }
            else
            {
                button4.Visible = false;
                button10.Visible = false;
            }
        }

        public void BuscarClientesBD()
        {
            string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("" +
                    "SELECT * " +
                    "FROM Clientes " +
                    "WHERE Excluido = 'false' " +
                    "ORDER BY Nome_Razao ASC", connection))
                {
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.HasRows == true)
                        {
                            int colunaId = dr.GetOrdinal("Id");
                            int colunaCliente = dr.GetOrdinal("Nome_Razao");
                            int colunaTelefone = dr.GetOrdinal("Telefone");
                            int colunaTipo_Cliente = dr.GetOrdinal("Tipo_Cliente");
                            int colunaData = dr.GetOrdinal("Data");

                            while (dr.Read())
                            {
                                Classe_Clientes cliente = new Classe_Clientes();

                                cliente.Id = dr.GetInt32(colunaId);
                                cliente.Nome_Razao = dr.GetString(colunaCliente);
                                cliente.Tipo_Cliente = dr.GetString(colunaTipo_Cliente);
                                cliente.Data = dr.GetDateTime(colunaData);

                                if(dr.GetString(colunaTelefone).Replace("(","").Replace(")", "").Replace(" ", "").Replace("-", "") == "")
                                {
                                    cliente.Telefone = "";
                                }
                                else
                                {
                                    cliente.Telefone = dr.GetString(colunaTelefone);
                                }

                                Clientes.Add(cliente);
                            }
                        }
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Cadastro_Cliente clientes = new Cadastro_Cliente(false, "");

            clientes.ShowDialog();

            AtualizarClientes();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView5.SelectedCells.Count == 0)
            {
                return;
            }

            int linha = dataGridView5.SelectedCells[0].RowIndex;
            if (linha < 0 || linha >= dataGridView5.Rows.Count)
            {
                return;
            }

            var row = dataGridView5.Rows[linha];
            if (row.IsNewRow || row.Cells[0].Value == null)
            {
                return;
            }

            Cadastro_Cliente cadastroPedidosLicitacao = new Cadastro_Cliente(true, row.Cells[0].Value.ToString());

            cadastroPedidosLicitacao.ShowDialog();

            AtualizarClientes();
        }

        private void dataGridView5_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int linha = e.RowIndex;
            if (linha < 0 || linha >= dataGridView5.Rows.Count)
            {
                return;
            }

            var row = dataGridView5.Rows[linha];
            if (row.IsNewRow || row.Cells[0].Value == null)
            {
                return;
            }

            Cadastro_Cliente cadastroPedidosLicitacao = new Cadastro_Cliente(true, row.Cells[0].Value.ToString());

            cadastroPedidosLicitacao.ShowDialog();

            AtualizarClientes();
        }

        private void Gestao_Clientes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                this.Close();
            }

            //if (e.KeyChar == 13)
            //{
            //    Filtrar();
            //}
        }

        private void Filtrar()
        {
            for(int i = 0; i < dataGridView5.Rows.Count; i++)
            {
                if (!dataGridView5.Rows[i].Cells[1].Value.ToString().ToUpper().Contains(textBox1.Text.ToUpper()))
                {
                    dataGridView5.Rows[i].Visible = false;
                }
                else
                {
                    dataGridView5.Rows[i].Visible = true;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Filtrar();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Funcoes.GerarPlanilhaExcel(dataGridView5, "Clientes");
        }
    }
}
