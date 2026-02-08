using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Lava_Car.Cadastros.Agenda;
using Lava_Car.Cadastros.Agenda.Classes;
using Lava_Car.Cadastros.Clientes;
using Lava_Car.Cadastros.Despesas;
using Lava_Car.Cadastros.Licitacoes;
using Lava_Car.Cadastros.Pedidos;
using Lava_Car.Cadastros.Pedidos.Classes;
using Npgsql;
using Lava_Car;

namespace Lava_Car
{
    public partial class Sistema_Lava_Car : Form
    {
        List<Classe_Pedidos> Pedidos = new List<Classe_Pedidos>();
        List<Classe_Agenda> Agenda = new List<Classe_Agenda>();

        public Sistema_Lava_Car()
        {
            try
            {
                InitializeComponent();

                try
                {

                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    dataGridView5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                    dataGridView5.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView5.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView5.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView5.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView5.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView5.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView5.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView5.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView5.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView5.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView5.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                    dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                catch
                {
                    
                }

                checkBox1.Checked = true;

                comboBox1.SelectedIndex = 0;

                dateTimePicker1.Value = DateTime.Today;
                dateTimePicker2.Value = DateTime.Today;
                dateTimePicker3.Value = DateTime.Today;
                dateTimePicker4.Value = DateTime.Today.AddDays(2);

                AtualizarPedidos();
                AtualizarAgenda();

                textBox1.Select();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void clienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Cadastro_Pedido cadastroPedidos = new Cadastro_Pedido(false, "", false);

            cadastroPedidos.ShowDialog();

            AtualizarPedidos();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Cadastro_Agenda cadastroAgendamento = new Cadastro_Agenda(false, "");

            cadastroAgendamento.ShowDialog();

            AtualizarAgenda();
        }


        public void AtualizarPedidos()
        {
            Pedidos.Clear();

            BuscarPedidosBD();

            dataGridView5.Rows.Clear();

            decimal totalPedidos = 0;
            int qntdPedidos = 0;

            for(int i = 0; i < Pedidos.Count; i++)
            {
                string nomeCliente = BuscarNomeCliente(Pedidos[i].Id_Cliente);
                bool possuiAvarias = !string.IsNullOrWhiteSpace(Pedidos[i].Avarias) || Pedidos[i].PossuiImagem;
                string avariasTexto = possuiAvarias ? "Sim" : "Não";

                if(nomeCliente != "")
                {
                    dataGridView5.Rows.Add(Pedidos[i].Data.ToString("dd/MM/yyyy HH:mm"),
                    nomeCliente,
                    Pedidos[i].Servico,
                    Pedidos[i].Veiculo,
                    Pedidos[i].Placa,
                    avariasTexto,
                    Pedidos[i].Valor,
                    Pedidos[i].Forma_Pagamento,
                    Pedidos[i].Situacao,
                    Pedidos[i].Observacao,
                    Pedidos[i].Id);

                    totalPedidos += Pedidos[i].Valor;

                    qntdPedidos++;
                }
                else
                {
                    dataGridView5.Rows.Add(Pedidos[i].Data.ToString("dd/MM/yyyy HH:mm"),
                    Pedidos[i].Cliente + " (Cliente Excluído)",
                    Pedidos[i].Servico,
                    Pedidos[i].Veiculo,
                    Pedidos[i].Placa,
                    avariasTexto,
                    Pedidos[i].Valor,
                    Pedidos[i].Forma_Pagamento,
                    Pedidos[i].Situacao,
                    Pedidos[i].Observacao,
                    Pedidos[i].Id);

                    totalPedidos += Pedidos[i].Valor;

                    qntdPedidos++;
                }
            }

            label7.Text = "R$ " + totalPedidos.ToString();

            label8.Text = qntdPedidos.ToString();

            if(Pedidos.Count > 0)
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

                if(comboBox1.SelectedIndex == 1)
                {
                    pago = " AND Situacao = 'Pago' ";
                }
                else if (comboBox1.SelectedIndex == 2)
                {
                    pago = " AND Situacao = 'Não Pago' ";
                }

                string cliente = "";

                if(textBox1.Text != "")
                {
                    cliente = " AND Cliente ILIKE '%" + textBox1.Text + "%'";
                }

                using (var command = new NpgsqlCommand("" +
                    "SELECT Pedidos.*, " +
                    "CASE WHEN Imagens.Id_Pedido IS NULL THEN FALSE ELSE TRUE END AS Possui_Imagem " +
                    "FROM Pedidos " +
                    "LEFT JOIN (SELECT DISTINCT Id_Pedido FROM Pedidos_Imagens WHERE Excluido = FALSE) Imagens " +
                    "ON Imagens.Id_Pedido = Pedidos.Id " +
                    "WHERE Excluido = 'false' and Data >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd 00:00:00") + "' AND Data <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd 23:59:59") + "'" + pago + cliente + "" +
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
                            int colunaAvarias = dr.GetOrdinal("Avarias");
                            int colunaServico = dr.GetOrdinal("Servico");
                            int colunaData = dr.GetOrdinal("Data");
                            int colunaValor = dr.GetOrdinal("Valor");
                            int colunaFormaPag = dr.GetOrdinal("Forma_Pagamento");
                            int colunaSituacao = dr.GetOrdinal("Situacao");
                            int colunaObservacao = dr.GetOrdinal("Observacao");
                            int colunaPossuiImagem = dr.GetOrdinal("Possui_Imagem");

                            while (dr.Read())
                            {
                                Classe_Pedidos pedido = new Classe_Pedidos();

                                pedido.Id = dr.GetInt32(colunaId);
                                pedido.Cliente = dr.GetString(colunaCliente);
                                pedido.Id_Cliente = dr.GetInt32(colunaIdCliente);
                                pedido.Veiculo = dr.GetString(colunaVeiculo);
                                pedido.Placa = dr.GetString(colunaPlaca);
                                pedido.Avarias = dr.GetString(colunaAvarias);
                                pedido.Servico = dr.GetString(colunaServico);
                                pedido.Data = dr.GetDateTime(colunaData);
                                pedido.Valor = dr.GetDecimal(colunaValor);
                                pedido.Forma_Pagamento = dr.GetString(colunaFormaPag);
                                pedido.Situacao = dr.GetString(colunaSituacao);
                                pedido.Observacao = dr.GetString(colunaObservacao);
                                pedido.PossuiImagem = dr.GetBoolean(colunaPossuiImagem);

                                Pedidos.Add(pedido);
                            }
                        }
                    }
                }
            }
        }

        public void AtualizarAgenda()
        {
            Agenda.Clear();

            BuscarAgendamentoBD();

            dataGridView1.Rows.Clear();

            int qntdAgendamentos = 0;

            for (int i = 0; i < Agenda.Count; i++)
            {
                string nomeCliente = BuscarNomeCliente(Agenda[i].Id_Cliente);

                if(nomeCliente != "")
                {
                    dataGridView1.Rows.Add(Agenda[i].Id,
                    Agenda[i].Data,
                    Agenda[i].Horario,
                    nomeCliente,
                    Agenda[i].Veiculo,
                    Agenda[i].Servico,
                    Agenda[i].Observacao);

                    qntdAgendamentos++;
                }
                else
                {
                    dataGridView1.Rows.Add(Agenda[i].Id,
                    Agenda[i].Data,
                    Agenda[i].Horario,
                    Agenda[i].Cliente + " (Cliente Excluído)",
                    Agenda[i].Veiculo,
                    Agenda[i].Servico,
                    Agenda[i].Observacao);

                    qntdAgendamentos++;
                }
            }

            label10.Text = qntdAgendamentos.ToString();

            if (Agenda.Count > 0)
            {
                button1.Visible = true;
                button7.Visible = true;
                button8.Visible = true;
                button9.Visible = true;
            }
            else
            {
                button1.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
                button9.Visible = false;
            }
        }

        public void BuscarAgendamentoBD()
        {
            string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("" +
                    "SELECT * " +
                    "FROM Agendamento " +
                    "WHERE Excluido = 'false' and Data >= '" + dateTimePicker3.Value.ToString("yyyy-MM-dd 00:00:00") + "' AND Data <= '" + dateTimePicker4.Value.ToString("yyyy-MM-dd 23:59:59") + "' " +
                    "ORDER BY Id ASC", connection))
                {
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.HasRows == true)
                        {
                            int colunaId = dr.GetOrdinal("Id");
                            int colunaCliente = dr.GetOrdinal("Cliente");
                            int colunaIdCliente = dr.GetOrdinal("Id_Cliente");
                            int colunaData = dr.GetOrdinal("Data");
                            int colunaHorario = dr.GetOrdinal("Horario");
                            int colunaServico = dr.GetOrdinal("Servico");
                            int colunaVeiculo = dr.GetOrdinal("Veiculo");
                            int colunaObservacao = dr.GetOrdinal("Observacao");

                            while (dr.Read())
                            {
                                Classe_Agenda agenda = new Classe_Agenda();

                                agenda.Id = dr.GetInt32(colunaId);
                                agenda.Cliente = dr.GetString(colunaCliente);
                                agenda.Id_Cliente = dr.GetInt32(colunaIdCliente);
                                agenda.Data = dr.GetDateTime(colunaData).ToString("dd/MM/yyyy");
                                agenda.Horario = dr.GetTimeSpan(colunaHorario).ToString();
                                agenda.Servico = dr.GetString(colunaServico);
                                agenda.Veiculo = dr.GetString(colunaVeiculo);
                                agenda.Observacao = dr.GetString(colunaObservacao);

                                Agenda.Add(agenda);
                            }
                        }
                    }
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            AtualizarPedidos();
        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if(dateTimePicker1.Value > dateTimePicker2.Value)
            {
                dateTimePicker2.Value = dateTimePicker1.Value;
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker2.Value < dateTimePicker1.Value)
            {
                dateTimePicker1.Value = dateTimePicker2.Value;
            }
        }

        private void Menu_Cadastro_Click(object sender, EventArgs e)
        {
            Gestao_Clientes gestaoClientes = new Gestao_Clientes();

            gestaoClientes.ShowDialog();

            AtualizarPedidos();
            AtualizarAgenda();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            AtualizarAgenda();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int linha = dataGridView5.SelectedCells[0].RowIndex;

            Cadastro_Pedido cadastroPedido = new Cadastro_Pedido(true, dataGridView5.Rows[linha].Cells["dataGridViewTextBoxColumn16"].Value.ToString(), false);

            cadastroPedido.ShowDialog();

            AtualizarPedidos();
        }

        private void dataGridView5_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int linha = e.RowIndex;
            if (linha < 0 || linha >= dataGridView5.Rows.Count)
            {
                return;
            }

            var row = dataGridView5.Rows[linha];
            if (row.IsNewRow || row.Cells["dataGridViewTextBoxColumn16"].Value == null)
            {
                return;
            }

            Cadastro_Pedido cadastroPedido = new Cadastro_Pedido(true, row.Cells["dataGridViewTextBoxColumn16"].Value.ToString(), false);

            cadastroPedido.ShowDialog();

            AtualizarPedidos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int linha = dataGridView1.SelectedCells[0].RowIndex;

            Cadastro_Agenda cadastroAgendamento = new Cadastro_Agenda(true, dataGridView1.Rows[linha].Cells[0].Value.ToString());

            cadastroAgendamento.ShowDialog();

            AtualizarAgenda();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int linha = e.RowIndex;
            if (linha < 0 || linha >= dataGridView1.Rows.Count)
            {
                return;
            }

            var row = dataGridView1.Rows[linha];
            if (row.IsNewRow || row.Cells[0].Value == null)
            {
                return;
            }

            Cadastro_Agenda cadastroAgendamento = new Cadastro_Agenda(true, row.Cells[0].Value.ToString());

            cadastroAgendamento.ShowDialog();

            AtualizarAgenda();
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker3.Value > dateTimePicker4.Value)
            {
                dateTimePicker4.Value = dateTimePicker3.Value;
            }
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker4.Value < dateTimePicker3.Value)
            {
                dateTimePicker3.Value = dateTimePicker4.Value;
            }
        }

        private void despesasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Gestao_Despesas gestaoDepesas = new Gestao_Despesas();

                gestaoDepesas.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void button7_Click(object sender, EventArgs e)
        {
            int linha = dataGridView1.SelectedCells[0].RowIndex;

            Cadastro_Pedido cadastroPedido = new Cadastro_Pedido(false, dataGridView1.Rows[linha].Cells[0].Value.ToString(), true);

            cadastroPedido.ShowDialog();

            AtualizarPedidos();
            AtualizarAgenda();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int linha = dataGridView1.SelectedCells[0].RowIndex;

            Cadastro_Pedidos_Licitacao cadastroPedidoLicitacao = new Cadastro_Pedidos_Licitacao(false, dataGridView1.Rows[linha].Cells[0].Value.ToString(), true);

            cadastroPedidoLicitacao.ShowDialog();

            AtualizarPedidos();
            AtualizarAgenda();
        }

        private void Sistema_Lava_Car_Load(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated)
            {
                // Aguarde a criação do identificador do formulário
                this.CreateHandle();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Funcoes.GerarPlanilhaExcel(dataGridView5, "Pedidos");
        }
        

        private void button9_Click(object sender, EventArgs e)
        {
            Funcoes.GerarPlanilhaExcel(dataGridView1, "Agendamentos");
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

        private void licitacoesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Gestao_Licitacoes gestaoLicitacoes = new Gestao_Licitacoes();

                gestaoLicitacoes.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                dataGridView5.Columns["Column5"].Visible = true;
            }
            else
            {
                dataGridView5.Columns["Column5"].Visible = false;
            }
        }
    }
}
