using Lava_Car.Cadastros.Agenda.Classes;
using Lava_Car.Cadastros.Clientes;
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

namespace Lava_Car.Cadastros.Agenda
{
    public partial class Cadastro_Agenda : Form
    {
        List<Classe_Clientes> Clientes = new List<Classe_Clientes>();

        Classe_Agenda Agenda = new Classe_Agenda();

        bool EditarAgenda;

        string Id = "";

        public Cadastro_Agenda(bool editarAgenda, string id)
        {
            InitializeComponent();

            GarantirTabelaAgendamento();

            AtualizarClientes();

            EditarAgenda = editarAgenda;

            if (editarAgenda)
            {
                button7.Visible = true;

                Id = id;

                BuscarAgendamento();
            }
            else
            {
                button7.Visible = false;
            }

            comboBox2.SelectedIndex = 3;

            comboBox1.Select();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public void Salvar()
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Todos os campos destacados em amarelo devem ser preenchidos!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmacao = MessageBox.Show("Deseja inserir este Agendamento?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacao == DialogResult.No)
            {
                return;
            }

            SalvarAgendaBD();

            MessageBox.Show("Agendamento inserido com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        public void SalvarAgendaBD()
        {
            if (EditarAgenda)
            {
                string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(
                        "UPDATE Agendamento SET " +
                        "Data = @data, " +
                        "Horario = @horario, " +
                        "Cliente = @cliente, " +
                        "Id_Cliente = @idCliente, " +
                        "Servico = @servico, " +
                        "Veiculo = @veiculo, " +
                        "Observacao = @observacao " +
                        "WHERE Id = '" + int.Parse(Id) + "'", connection))
                    {
                        command.Parameters.AddWithValue("@data", dateTimePicker1.Value);
                        command.Parameters.AddWithValue("@horario", TimeSpan.Parse(dateTimePicker2.Value.ToString("HH:mm:00")));
                        command.Parameters.AddWithValue("@cliente", comboBox1.Text);
                        command.Parameters.AddWithValue("@idCliente", Clientes[comboBox1.SelectedIndex].Id);
                        command.Parameters.AddWithValue("@servico", comboBox2.Text);
                        command.Parameters.AddWithValue("@observacao", textBox1.Text);
                        command.Parameters.AddWithValue("@veiculo", textBox6.Text);
                        command.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(
                        "INSERT INTO Agendamento (Data, Horario, Cliente, Id_Cliente, Servico, Veiculo, Observacao) " +
                        "VALUES (@data, @horario, @cliente, @idCliente, @servico, @veiculo, @observacao)", connection))
                    {
                        command.Parameters.AddWithValue("@data", dateTimePicker1.Value);
                        command.Parameters.AddWithValue("@horario", TimeSpan.Parse(dateTimePicker2.Value.ToString("HH:mm:00")));
                        command.Parameters.AddWithValue("@cliente", comboBox1.Text);
                        command.Parameters.AddWithValue("@idCliente", Clientes[comboBox1.SelectedIndex].Id);
                        command.Parameters.AddWithValue("@servico", comboBox2.Text);
                        command.Parameters.AddWithValue("@veiculo", textBox6.Text);
                        command.Parameters.AddWithValue("@observacao", textBox1.Text);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void AtualizarClientes()
        {
            Clientes.Clear();

            BuscarClientesBD();

            comboBox1.Items.Clear();

            for (int i = 0; i < Clientes.Count; i++)
            {
                comboBox1.Items.Add(Clientes[i].Nome_Razao);
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
                            int colunaCliente = dr.GetOrdinal("Nome_Razao");
                            int colunaId = dr.GetOrdinal("Id");

                            while (dr.Read())
                            {
                                Classe_Clientes clientePj = new Classe_Clientes();

                                clientePj.Nome_Razao = dr.GetString(colunaCliente);
                                clientePj.Id = dr.GetInt32(colunaId);

                                Clientes.Add(clientePj);
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cadastro_Cliente clientes = new Cadastro_Cliente(false, "");

            clientes.ShowDialog();
        }

        private void GarantirTabelaAgendamento()
        {
            string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "CREATE TABLE IF NOT EXISTS Agendamento (" +
                    "Id SERIAL PRIMARY KEY, " +
                    "Data DATE NOT NULL, " +
                    "Horario TIME NOT NULL, " +
                    "Cliente TEXT NOT NULL, " +
                    "Id_Cliente INTEGER NOT NULL, " +
                    "Servico TEXT, " +
                    "Veiculo TEXT, " +
                    "Observacao TEXT, " +
                    "Excluido BOOLEAN NOT NULL DEFAULT FALSE" +
                    ")", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void BuscarAgendamento()
        {
            BuscarAgendamentoBD();

            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (comboBox1.Items[i].ToString() == Agenda.Cliente)
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < comboBox2.Items.Count; i++)
            {
                if (comboBox2.Items[i].ToString() == Agenda.Servico)
                {
                    comboBox2.SelectedIndex = i;
                    break;
                }
            }

            dateTimePicker1.Value = DateTime.Parse(Agenda.Data);
            dateTimePicker2.Value = DateTime.Parse(Agenda.Horario);
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
                    "WHERE Id = '" + Id + "'", connection))
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
                            int colunaObservacao = dr.GetOrdinal("Observacao");

                            while (dr.Read())
                            {
                                Agenda.Id = dr.GetInt32(colunaId);
                                Agenda.Cliente = dr.GetString(colunaCliente);
                                Agenda.Id_Cliente = dr.GetInt32(colunaIdCliente);
                                Agenda.Data = dr.GetDateTime(colunaData).ToString();
                                Agenda.Horario = dr.GetTimeSpan(colunaHorario).ToString();
                                Agenda.Servico = dr.GetString(colunaServico);
                                Agenda.Observacao = dr.GetString(colunaObservacao);
                            }
                        }
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DeletarAgendamento();
        }

        private void DeletarAgendamento()
        {
            DialogResult confirmacao = MessageBox.Show("Deseja realmente Excluir este Agendamento?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacao == DialogResult.No)
            {
                return;
            }

            ExcluirAgendamento();

            MessageBox.Show("Agendamento excluído com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void ExcluirAgendamento()
        {
            string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "UPDATE Agendamento " +
                    "SET Excluido = 'true' " +
                    "WHERE Id = '" + Id + "'", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void Cadastro_Agenda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                this.Close();
            }
        }

        private void Cadastro_Agenda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 113)
            {
                Salvar();
            }
        }
    }
}
