using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;

namespace Lava_Car.Cadastros.Pedidos
{
    public partial class Anexos_Pedido : Form
    {
        private const string ConnectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";
        private readonly int _idPedido;
        private readonly List<PedidoImagem> _imagens = new List<PedidoImagem>();

        public Anexos_Pedido(int idPedido)
        {
            InitializeComponent();

            _idPedido = idPedido;
            labelPedido.Text = $"Pedido: #{_idPedido}";

            GarantirTabelaImagens();
            CarregarImagens();
        }

        private void buttonAdicionar_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                dialog.Multiselect = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                foreach (var arquivo in dialog.FileNames)
                {
                    SalvarImagem(arquivo);
                }

                CarregarImagens();
            }
        }

        private void buttonExcluir_Click(object sender, EventArgs e)
        {
            if (listViewImagens.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecione uma imagem para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacao = MessageBox.Show("Deseja realmente excluir esta imagem?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacao == DialogResult.No)
            {
                return;
            }

            var item = listViewImagens.SelectedItems[0];
            var imagem = item.Tag as PedidoImagem;
            if (imagem == null)
            {
                return;
            }

            ExcluirImagem(imagem.Id);
            CarregarImagens();
        }

        private void buttonFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listViewImagens_DoubleClick(object sender, EventArgs e)
        {
            if (listViewImagens.SelectedItems.Count == 0)
            {
                return;
            }

            var item = listViewImagens.SelectedItems[0];
            var imagem = item.Tag as PedidoImagem;
            if (imagem == null)
            {
                return;
            }

            using (var visualizar = new VisualizarImagem(imagem.NomeArquivo, imagem.Conteudo))
            {
                visualizar.ShowDialog();
            }
        }

        private void SalvarImagem(string caminhoArquivo)
        {
            var bytes = File.ReadAllBytes(caminhoArquivo);
            var nomeArquivo = Path.GetFileName(caminhoArquivo);

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "INSERT INTO Pedidos_Imagens (Id_Pedido, Nome_Arquivo, Conteudo, Data_Cadastro) " +
                    "VALUES (@idPedido, @nomeArquivo, @conteudo, @dataCadastro)", connection))
                {
                    command.Parameters.AddWithValue("@idPedido", _idPedido);
                    command.Parameters.AddWithValue("@nomeArquivo", nomeArquivo);
                    command.Parameters.Add("@conteudo", NpgsqlDbType.Bytea).Value = bytes;
                    command.Parameters.AddWithValue("@dataCadastro", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void ExcluirImagem(int idImagem)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "UPDATE Pedidos_Imagens SET Excluido = TRUE WHERE Id = @idImagem", connection))
                {
                    command.Parameters.AddWithValue("@idImagem", idImagem);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void GarantirTabelaImagens()
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "CREATE TABLE IF NOT EXISTS Pedidos_Imagens (" +
                    "Id SERIAL PRIMARY KEY, " +
                    "Id_Pedido INTEGER NOT NULL, " +
                    "Nome_Arquivo TEXT NOT NULL, " +
                    "Conteudo BYTEA NOT NULL, " +
                    "Data_Cadastro TIMESTAMP NOT NULL DEFAULT NOW(), " +
                    "Excluido BOOLEAN NOT NULL DEFAULT FALSE" +
                    ")", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void CarregarImagens()
        {
            _imagens.Clear();

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "SELECT Id, Nome_Arquivo, Conteudo " +
                    "FROM Pedidos_Imagens " +
                    "WHERE Id_Pedido = @idPedido AND Excluido = FALSE " +
                    "ORDER BY Id ASC", connection))
                {
                    command.Parameters.AddWithValue("@idPedido", _idPedido);
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            AtualizarLista();
                            return;
                        }

                        var colunaId = reader.GetOrdinal("Id");
                        var colunaNome = reader.GetOrdinal("Nome_Arquivo");
                        var colunaConteudo = reader.GetOrdinal("Conteudo");

                        while (reader.Read())
                        {
                            _imagens.Add(new PedidoImagem
                            {
                                Id = reader.GetInt32(colunaId),
                                NomeArquivo = reader.GetString(colunaNome),
                                Conteudo = reader.GetFieldValue<byte[]>(colunaConteudo)
                            });
                        }
                    }
                }
            }

            AtualizarLista();
        }

        private void AtualizarLista()
        {
            imageListImagens.Images.Clear();
            listViewImagens.Items.Clear();

            if (_imagens.Count == 0)
            {
                labelStatus.Text = "Nenhuma imagem anexada.";
                return;
            }

            labelStatus.Text = $"{_imagens.Count} imagem(ns) anexada(s).";

            for (int i = 0; i < _imagens.Count; i++)
            {
                var imagem = _imagens[i];
                var thumbnail = CriarThumbnail(imagem.Conteudo);
                imageListImagens.Images.Add(thumbnail);

                var item = new ListViewItem(imagem.NomeArquivo, i)
                {
                    Tag = imagem
                };

                listViewImagens.Items.Add(item);
            }
        }

        private static Image CriarThumbnail(byte[] conteudo)
        {
            using (var ms = new MemoryStream(conteudo))
            using (var img = Image.FromStream(ms))
            {
                return new Bitmap(img);
            }
        }

        private class PedidoImagem
        {
            public int Id { get; set; }
            public string NomeArquivo { get; set; }
            public byte[] Conteudo { get; set; }
        }

        private class VisualizarImagem : Form
        {
            private readonly PictureBox _pictureBox;

            public VisualizarImagem(string nomeArquivo, byte[] conteudo)
            {
                Text = nomeArquivo;
                StartPosition = FormStartPosition.CenterParent;
                BackColor = Color.FromArgb(40, 40, 40);
                ForeColor = Color.White;
                ShowIcon = false;
                WindowState = FormWindowState.Maximized;
                Width = 800;
                Height = 600;

                _pictureBox = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image = CarregarImagem(conteudo)
                };

                Controls.Add(_pictureBox);
            }

            private static Image CarregarImagem(byte[] conteudo)
            {
                using (var ms = new MemoryStream(conteudo))
                using (var img = Image.FromStream(ms))
                {
                    return new Bitmap(img);
                }
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _pictureBox?.Image?.Dispose();
                    _pictureBox?.Dispose();
                }

                base.Dispose(disposing);
            }
        }
    }
}
