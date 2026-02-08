using Lava_Car.Cadastros.Despesas.Classes;
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

namespace Lava_Car.Cadastros.Despesas
{
    public partial class Cadastro_Despesas : Form
    {
        Classe_Despesas Despesa = new Classe_Despesas();

        bool EditarDespesa = false;
        string Id = "";

        public Cadastro_Despesas(bool editarDespesa, string id)
        {
            InitializeComponent();

            GarantirTabelaDespesas();

            dateTimePicker1.Value = DateTime.Now;

            EditarDespesa = editarDespesa;

            if (editarDespesa)
            {
                button7.Visible = true;

                Id = id;

                BuscarDespesa();
            }
            else
            {
                button7.Visible = false;
            }
        }

        private void Cadastro_Despesas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 27)
            {
                this.Close();
            }
        }

        private void Cadastro_Despesas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue == 113)
            {
                Salvar();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        public void Salvar()
        {
            if (dateTimePicker1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Todos os campos destacados em amarelo devem ser preenchidos!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal valor;

            if (decimal.TryParse(textBox3.Text, out valor) == false)
            {
                MessageBox.Show("O valor da Despesa deve ser numérico!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmacao = MessageBox.Show("Deseja inserir esta Despesa?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacao == DialogResult.No)
            {
                return;
            }

            SalvarDespesaBD();

            MessageBox.Show("Despesa inserida com sucesso!","Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        public void SalvarDespesaBD()
        {
            if (EditarDespesa)
            {
                string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(
                        "UPDATE Despesas SET " +
                        "Despesa = @despesa, " +
                        "Valor = CAST(@valor AS NUMERIC), " +
                        "Observacao = @observacao, " +
                        "Data = @data, " +
                        "Data_Alteracao = @dataAlteracao " +
                        "WHERE Id = '" + int.Parse(Id) + "'", connection))
                    {
                        command.Parameters.AddWithValue("@despesa", textBox2.Text);
                        command.Parameters.AddWithValue("@observacao", textBox1.Text);
                        command.Parameters.AddWithValue("@valor", textBox3.Text.Replace(",", "."));
                        command.Parameters.AddWithValue("@data", DateTime.Parse(dateTimePicker1.Text));
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
                        "INSERT INTO Despesas (Despesa, Valor, Observacao, Data, Data_Alteracao) " +
                        "VALUES (@despesa, CAST(@valor AS NUMERIC), @observacao, @data, @dataAlteracao)", connection))
                    {
                        command.Parameters.AddWithValue("@despesa", textBox2.Text);
                        command.Parameters.AddWithValue("@observacao", textBox1.Text);
                        command.Parameters.AddWithValue("@valor", textBox3.Text.Replace(",","."));
                        command.Parameters.AddWithValue("@data", DateTime.Parse(dateTimePicker1.Text));
                        command.Parameters.AddWithValue("@dataAlteracao", DateTime.Now);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void GarantirTabelaDespesas()
        {
            string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "CREATE TABLE IF NOT EXISTS Despesas (" +
                    "Id SERIAL PRIMARY KEY, " +
                    "Despesa TEXT NOT NULL, " +
                    "Valor NUMERIC(12,2) NOT NULL DEFAULT 0, " +
                    "Observacao TEXT, " +
                    "Data DATE NOT NULL, " +
                    "Data_Alteracao TIMESTAMP NOT NULL DEFAULT NOW(), " +
                    "Excluido BOOLEAN NOT NULL DEFAULT FALSE" +
                    ")", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void BuscarDespesa()
        {
            BuscarDespesaBD();

            dateTimePicker1.Value = Despesa.Data;
            textBox2.Text = Despesa.Despesa;
            textBox3.Text = Despesa.Valor.ToString();
            textBox1.Text = Despesa.Observacao;
        }

        public void BuscarDespesaBD()
        {
            string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();


                using (var command = new NpgsqlCommand("" +
                    "SELECT * " +
                    "FROM Despesas " +
                    "WHERE Id = '" + Id + "' " +
                    "ORDER BY Data ASC", connection))
                {
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.HasRows == true)
                        {
                            int colunaId = dr.GetOrdinal("Id");
                            int colunaDespesa = dr.GetOrdinal("Despesa");
                            int colunaData = dr.GetOrdinal("Data");
                            int colunaValor = dr.GetOrdinal("Valor");
                            int colunaObservacao = dr.GetOrdinal("Observacao");

                            while (dr.Read())
                            {
                                Despesa.Id = dr.GetInt32(colunaId);
                                Despesa.Despesa = dr.GetString(colunaDespesa);
                                Despesa.Data = dr.GetDateTime(colunaData);
                                Despesa.Valor = dr.GetDecimal(colunaValor);
                                Despesa.Observacao = dr.GetString(colunaObservacao);
                            }
                        }
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DeletarDespesa();
        }

        private void DeletarDespesa()
        {
            DialogResult confirmacao = MessageBox.Show("Deseja realmente Excluir esta Despesa?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacao == DialogResult.No)
            {
                return;
            }

            ExcluirDespesaBD();

            MessageBox.Show("Despesa excluída com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void ExcluirDespesaBD()
        {
            string connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "UPDATE Despesas " +
                    "SET Excluido = 'true', Data_Alteracao = '" + DateTime.Now + "' " +
                    "WHERE Id = '" + Id + "'", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
