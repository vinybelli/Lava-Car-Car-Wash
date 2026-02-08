using Lava_Car.Cadastros.Clientes.Classes;
using Npgsql;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Lava_Car.Cadastros.Clientes
{
    public partial class Cadastro_Cliente : Form
    {
        Classe_Clientes Cliente = new Classe_Clientes();

        bool EditarCliente = false;

        string Id = "";

        public Cadastro_Cliente(bool editarCliente, string id)
        {
            InitializeComponent();

            GarantirTabelaClientes();

            comboBox7.SelectedIndex = 0;

            EditarCliente = editarCliente;

            if(editarCliente == true)
            {
                button7.Visible = true;

                Id = id;

                BuscarCliente();
            }
            else
            {
                button7.Visible = false;
            }

            checkBox1.Checked = true;
        }

        private void comboBox7_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(comboBox7.SelectedIndex == 0)
            {
                label3.Visible = false;
                textBox2.Visible = false;
                label5.Visible = false;
                maskedTextBox2.Visible = false;
                label2.Text = "Nome";
                label2.Location = new System.Drawing.Point(48, 55);
                label4.Text = "CPF";
                label4.Location = new System.Drawing.Point(61, 83);
                maskedTextBox1.Mask = "000,000,000-00";
            }
            else
            {
                label3.Visible = true;
                textBox2.Visible = true;
                label5.Visible = true;
                maskedTextBox2.Visible = true;
                label2.Text = "Razão Social";
                label2.Location = new System.Drawing.Point(9, 55);
                label4.Text = "CNPJ";
                label4.Location = new System.Drawing.Point(51, 83);
                maskedTextBox1.Mask = "00,000,000/0000-00";
            }
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void Clientes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 27)
            {
                this.Close();
            }
        }

        private void Clientes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 113)
            {
                Salvar();
            }
        }

        private void button6_Click(object sender, System.EventArgs e)
        {
            Salvar();
        }

        public void Salvar()
        {
            if (comboBox7.SelectedIndex == -1
                || textBox1.Text == "")
            {
                MessageBox.Show("Todos os campos destacados em amarelo devem ser preenchidos!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmacao = MessageBox.Show("Deseja inserir este Cliente?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(confirmacao == DialogResult.No)
            {
                return;
            }

            SalvarCliente();

            MessageBox.Show("Cliente inserido com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        public void SalvarCliente()
        {
            if (EditarCliente)
            {
                string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(
                        "UPDATE Clientes SET " +
                        "Nome_Razao = @nomeRazao, " +
                        "Nome_Fantasia = @nomeFantasia, " +
                        "CPF_CNPJ = @cpfCNPJ, " +
                        "Telefone = @telefone, " +
                        "Inscricao_Estadual = @inscricaoEstadual, " +
                        "Tipo_Cliente = @tipoCliente, " +
                        "Data_Alteracao = @dataAlteracao " +
                        "WHERE Id = '" + int.Parse(Id) + "'", connection))
                    {
                        command.Parameters.AddWithValue("@nomeRazao", textBox1.Text);
                        command.Parameters.AddWithValue("@nomeFantasia", textBox2.Text);
                        command.Parameters.AddWithValue("@cpfCNPJ", maskedTextBox1.Text.Replace(",", "."));
                        command.Parameters.AddWithValue("@telefone", maskedTextBox3.Text);
                        command.Parameters.AddWithValue("@inscricaoEstadual", maskedTextBox2.Text.Replace(",", "."));
                        command.Parameters.AddWithValue("@tipoCliente", comboBox7.Text);
                        command.Parameters.AddWithValue("@data", DateTime.Now);
                        command.Parameters.AddWithValue("@dataAlteracao", DateTime.Now);
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
                        "INSERT INTO Clientes (Nome_Razao, Nome_Fantasia, CPF_CNPJ, Telefone, Inscricao_Estadual, Tipo_Cliente, Data, Data_Alteracao) " +
                        "VALUES (@nomeRazao, @nomeFantasia, @cpfCNPJ, @telefone, @inscricaoEstadual, @tipoCliente, @data, @dataAlteracao)", connection))
                    {
                        command.Parameters.AddWithValue("@nomeRazao", textBox1.Text);
                        command.Parameters.AddWithValue("@nomeFantasia", textBox2.Text);
                        command.Parameters.AddWithValue("@cpfCNPJ", maskedTextBox1.Text.Replace(",", "."));
                        command.Parameters.AddWithValue("@telefone", maskedTextBox3.Text);
                        command.Parameters.AddWithValue("@inscricaoEstadual", maskedTextBox2.Text.Replace(",","."));
                        command.Parameters.AddWithValue("@tipoCliente", comboBox7.Text);
                        command.Parameters.AddWithValue("@data", DateTime.Now);
                        command.Parameters.AddWithValue("@dataAlteracao", DateTime.Now);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void button7_Click(object sender, System.EventArgs e)
        {
            DeletarCliente();
        }

        private void DeletarCliente()
        {
            DialogResult confirmacao = MessageBox.Show("Deseja realmente Excluir este Cliente?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacao == DialogResult.No)
            {
                return;
            }

            ExcluirClienteBD();

            MessageBox.Show("Cliente excluído com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void ExcluirClienteBD()
        {
            string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "UPDATE Clientes " +
                    "SET Excluido = 'true', Data_Alteracao = '" + DateTime.Now + "' " +
                    "WHERE Id = '" + Id + "'", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                label6.Text = "Celular";
                label6.Location = new System.Drawing.Point(43, 131);
                maskedTextBox3.Mask = "(00) 00000-0000";
            }
            else
            {
                label6.Text = "Telefone";
                label6.Location = new System.Drawing.Point(33, 131);
                maskedTextBox3.Mask = "(00) 0000-0000";
            }
        }

        private void GarantirTabelaClientes()
        {
            string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "CREATE TABLE IF NOT EXISTS Clientes (" +
                    "Id SERIAL PRIMARY KEY, " +
                    "Nome_Razao TEXT NOT NULL, " +
                    "Nome_Fantasia TEXT, " +
                    "CPF_CNPJ TEXT, " +
                    "Telefone TEXT, " +
                    "Inscricao_Estadual TEXT, " +
                    "Tipo_Cliente TEXT, " +
                    "Data TIMESTAMP NOT NULL DEFAULT NOW(), " +
                    "Data_Alteracao TIMESTAMP NOT NULL DEFAULT NOW(), " +
                    "Excluido BOOLEAN NOT NULL DEFAULT FALSE" +
                    ")", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void BuscarCliente()
        {
            BuscarClienteBD();

            for (int i = 0; i < comboBox7.Items.Count; i++)
            {
                if (comboBox7.Items[i].ToString() == Cliente.Tipo_Cliente)
                {
                    comboBox7.SelectedIndex = i;
                    break;
                }
            }

            textBox1.Text = Cliente.Nome_Razao;
            textBox2.Text = Cliente.Nome_Fantasia;
            maskedTextBox1.Text = Cliente.CPF_CNPJ;
            maskedTextBox2.Text = Cliente.IE;

            if (Cliente.Telefone.Length == 14)
            {
                checkBox1.Checked = false;
            }
            else
            {
                checkBox1.Checked = true;
            }

            maskedTextBox3.Text = Cliente.Telefone;
        }

        private void BuscarClienteBD()
        {
            string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("" +
                    "SELECT * " +
                    "FROM Clientes " +
                    "WHERE id = '" + Id + "'", connection))
                {
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.HasRows == true)
                        {
                            int colunaId = dr.GetOrdinal("Id");
                            int colunaCliente = dr.GetOrdinal("Nome_Razao");
                            int colunaNomeFantasia = dr.GetOrdinal("Nome_Fantasia");
                            int colunaCpfCnpj = dr.GetOrdinal("CPF_CNPJ");
                            int colunaTelefone = dr.GetOrdinal("Telefone");
                            int colunaInscricaoEstadual = dr.GetOrdinal("Inscricao_Estadual");
                            int colunaTipoCliente = dr.GetOrdinal("Tipo_Cliente");

                            while (dr.Read())
                            {
                                Cliente.Id = dr.GetInt32(colunaId);
                                Cliente.Nome_Razao = dr.GetString(colunaCliente);
                                Cliente.Nome_Fantasia = dr.GetString(colunaNomeFantasia);
                                Cliente.CPF_CNPJ = dr.GetString(colunaCpfCnpj);
                                Cliente.Telefone = dr.GetString(colunaTelefone);
                                Cliente.IE = dr.GetString(colunaInscricaoEstadual);
                                Cliente.Tipo_Cliente = dr.GetString(colunaTipoCliente);
                            }
                        }
                    }
                }
            }
        }
    }
}
