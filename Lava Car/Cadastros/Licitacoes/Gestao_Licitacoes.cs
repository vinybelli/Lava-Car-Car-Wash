using Lava_Car.Cadastros.Pedidos.Classes;
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

namespace Lava_Car.Cadastros.Licitacoes
{
    public partial class Gestao_Licitacoes : Form
    {
        List<Classe_Pedidos> PedidosLicitacao = new List<Classe_Pedidos>();

        public Gestao_Licitacoes()
        {
            try
            {
                InitializeComponent();

                comboBox1.SelectedIndex = 0;

                dataGridView5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                dateTimePicker1.Value = DateTime.Today;
                dateTimePicker2.Value = DateTime.Today;

            
                BeginInvoke(new Action(ConfigureGridColumns));
           

                AtualizarPedidos();

                textBox1.Select();
            }
            catch
            {

            }
        }

        private void ConfigureGridColumns()
        {
            if (dataGridView5 == null || dataGridView5.Columns.Count == 0)
            {
                return;
            }

            dataGridView5.SuspendLayout();

            try
            {
                dataGridView5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                dataGridView5.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView5.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView5.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView5.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView5.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView5.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView5.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView5.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView5.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                dataGridView5.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch
            {
            }
            finally
            {
                dataGridView5.ResumeLayout();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Gestao_Licitacoes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                this.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Cadastro_Pedidos_Licitacao cadastroPedidos = new Cadastro_Pedidos_Licitacao(false, "", false);

            cadastroPedidos.ShowDialog();

            AtualizarPedidos();
        }

        public void AtualizarPedidos()
        {
            PedidosLicitacao.Clear();

            BuscarPedidosBD();

            dataGridView5.Rows.Clear();

            decimal totalPedidos = 0;
            int qntdPedidos = 0;

            for (int i = 0; i < PedidosLicitacao.Count; i++)
            {
                string nomeCliente = BuscarNomeCliente(PedidosLicitacao[i].Id_Cliente);

                if(nomeCliente != "")
                {
                    dataGridView5.Rows.Add(PedidosLicitacao[i].Data.ToString("dd/MM/yyyy HH:mm"),
                    nomeCliente,
                    PedidosLicitacao[i].Servico,
                    PedidosLicitacao[i].Veiculo,
                    PedidosLicitacao[i].Placa,
                    PedidosLicitacao[i].Valor,
                    PedidosLicitacao[i].Forma_Pagamento,
                    PedidosLicitacao[i].Situacao,
                    PedidosLicitacao[i].Observacao,
                    PedidosLicitacao[i].Id);
                }
                else
                {
                    dataGridView5.Rows.Add(PedidosLicitacao[i].Data.ToString("dd/MM/yyyy HH:mm"),
                    PedidosLicitacao[i].Cliente + " (Cliente Excluído)",
                    PedidosLicitacao[i].Servico,
                    PedidosLicitacao[i].Veiculo,
                    PedidosLicitacao[i].Placa,
                    PedidosLicitacao[i].Valor,
                    PedidosLicitacao[i].Forma_Pagamento,
                    PedidosLicitacao[i].Situacao,
                    PedidosLicitacao[i].Observacao,
                    PedidosLicitacao[i].Id);
                }

                

                totalPedidos += PedidosLicitacao[i].Valor;

                qntdPedidos++;
            }

            label7.Text = "R$ " + totalPedidos.ToString();

            label8.Text = qntdPedidos.ToString();

            if (totalPedidos > 0)
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

        public void BuscarPedidosBD()
        {
            string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string pago = "";

                if (comboBox1.SelectedIndex == 1)
                {
                    pago = "AND Situacao = 'Pago' ";
                }
                else if (comboBox1.SelectedIndex == 2)
                {
                    pago = "AND Situacao = 'Não Pago' ";
                }

                string cliente = "";

                if (textBox1.Text != "")
                {
                    cliente = " AND Cliente ILIKE '%" + textBox1.Text + "%'";
                }


                using (var command = new NpgsqlCommand("" +
                    "SELECT * " +
                    "FROM Pedidos_Licitacao " +
                    "WHERE Excluido = 'false' AND Data >= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd 00:00:00") + "' AND Data <= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd 23:59:59") + "' " + pago + cliente + "" +
                    "ORDER BY Id ASC", connection))
                {
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.HasRows == true)
                        {
                            int colunaId = dr.GetOrdinal("Id");
                            int colunaCliente = dr.GetOrdinal("Cliente");
                            int colunaIdCliente = dr.GetOrdinal("Id_Cliente");
                            int colunaVeiculo = dr.GetOrdinal("Veiculo");
                            int colunaPlaca = dr.GetOrdinal("Placa");
                            int colunaServico = dr.GetOrdinal("Servico");
                            int colunaData = dr.GetOrdinal("Data");
                            int colunaValor = dr.GetOrdinal("Valor");
                            int colunaFormaPag = dr.GetOrdinal("Forma_Pagamento");
                            int colunaSituacao = dr.GetOrdinal("Situacao");
                            int colunaObservacao = dr.GetOrdinal("Observacao");

                            while (dr.Read())
                            {
                                Classe_Pedidos pedido = new Classe_Pedidos();

                                pedido.Id = dr.GetInt32(colunaId);
                                pedido.Cliente = dr.GetString(colunaCliente);
                                pedido.Id_Cliente = dr.GetInt32(colunaIdCliente);
                                pedido.Veiculo = dr.GetString(colunaVeiculo);
                                pedido.Placa = dr.GetString(colunaPlaca);
                                pedido.Servico = dr.GetString(colunaServico);
                                pedido.Data = dr.GetDateTime(colunaData);
                                pedido.Valor = dr.GetDecimal(colunaValor);
                                pedido.Forma_Pagamento = dr.GetString(colunaFormaPag);
                                pedido.Situacao = dr.GetString(colunaSituacao);
                                pedido.Observacao = dr.GetString(colunaObservacao);

                                PedidosLicitacao.Add(pedido);
                            }
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int linha = dataGridView5.SelectedCells[0].RowIndex;

            Cadastro_Pedidos_Licitacao cadastroPedidosLicitacao = new Cadastro_Pedidos_Licitacao(true, dataGridView5.Rows[linha].Cells[9].Value.ToString(), false);

            cadastroPedidosLicitacao.ShowDialog();

            AtualizarPedidos();
        }

        private void dataGridView5_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int linha = e.RowIndex;

            Cadastro_Pedidos_Licitacao cadastroPedidosLicitacao = new Cadastro_Pedidos_Licitacao(true, dataGridView5.Rows[linha].Cells[9].Value.ToString(), false);

            cadastroPedidosLicitacao.ShowDialog();

            AtualizarPedidos();
        }

        public string BuscarNomeCliente(int id)
        {
            string nome = "";

            try
            {
                string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand("" +
                        "SELECT * " +
                        "FROM Clientes " +
                        "WHERE id = '" + id + "'", connection))
                    {
                        using (var dr = command.ExecuteReader())
                        {
                            if (dr.HasRows == true)
                            {
                                int colunaCliente = dr.GetOrdinal("Nome_Razao");

                                while (dr.Read())
                                {
                                    nome = dr.GetString(colunaCliente);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return nome;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AtualizarPedidos();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Funcoes.GerarPlanilhaExcel(dataGridView5, "Pedidos Licitações");
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                AtualizarPedidos();
            }
        }
    }
}
