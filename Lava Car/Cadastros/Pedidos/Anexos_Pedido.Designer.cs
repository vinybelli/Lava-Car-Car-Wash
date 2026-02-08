namespace Lava_Car.Cadastros.Pedidos
{
    partial class Anexos_Pedido
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listViewImagens = new System.Windows.Forms.ListView();
            this.imageListImagens = new System.Windows.Forms.ImageList(this.components);
            this.buttonAdicionar = new System.Windows.Forms.Button();
            this.buttonExcluir = new System.Windows.Forms.Button();
            this.buttonFechar = new System.Windows.Forms.Button();
            this.labelPedido = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewImagens
            // 
            this.listViewImagens.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewImagens.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.listViewImagens.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewImagens.ForeColor = System.Drawing.Color.White;
            this.listViewImagens.HideSelection = false;
            this.listViewImagens.LargeImageList = this.imageListImagens;
            this.listViewImagens.Location = new System.Drawing.Point(12, 44);
            this.listViewImagens.MultiSelect = false;
            this.listViewImagens.Name = "listViewImagens";
            this.listViewImagens.Size = new System.Drawing.Size(760, 445);
            this.listViewImagens.TabIndex = 2;
            this.listViewImagens.UseCompatibleStateImageBehavior = false;
            this.listViewImagens.View = System.Windows.Forms.View.LargeIcon;
            this.listViewImagens.DoubleClick += new System.EventHandler(this.listViewImagens_DoubleClick);
            // 
            // imageListImagens
            // 
            this.imageListImagens.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListImagens.ImageSize = new System.Drawing.Size(120, 90);
            this.imageListImagens.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // buttonAdicionar
            // 
            this.buttonAdicionar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdicionar.BackColor = System.Drawing.Color.Green;
            this.buttonAdicionar.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonAdicionar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdicionar.Font = new System.Drawing.Font("Montserrat", 8.999999F);
            this.buttonAdicionar.ForeColor = System.Drawing.Color.White;
            this.buttonAdicionar.Location = new System.Drawing.Point(430, 500);
            this.buttonAdicionar.Name = "buttonAdicionar";
            this.buttonAdicionar.Size = new System.Drawing.Size(110, 30);
            this.buttonAdicionar.TabIndex = 3;
            this.buttonAdicionar.Text = "Adicionar";
            this.buttonAdicionar.UseVisualStyleBackColor = false;
            this.buttonAdicionar.Click += new System.EventHandler(this.buttonAdicionar_Click);
            // 
            // buttonExcluir
            // 
            this.buttonExcluir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExcluir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(13)))), ((int)(((byte)(0)))));
            this.buttonExcluir.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonExcluir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExcluir.Font = new System.Drawing.Font("Montserrat", 8.999999F);
            this.buttonExcluir.ForeColor = System.Drawing.Color.White;
            this.buttonExcluir.Location = new System.Drawing.Point(546, 500);
            this.buttonExcluir.Name = "buttonExcluir";
            this.buttonExcluir.Size = new System.Drawing.Size(110, 30);
            this.buttonExcluir.TabIndex = 4;
            this.buttonExcluir.Text = "Excluir";
            this.buttonExcluir.UseVisualStyleBackColor = false;
            this.buttonExcluir.Click += new System.EventHandler(this.buttonExcluir_Click);
            // 
            // buttonFechar
            // 
            this.buttonFechar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFechar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.buttonFechar.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFechar.Font = new System.Drawing.Font("Montserrat", 8.999999F);
            this.buttonFechar.ForeColor = System.Drawing.Color.White;
            this.buttonFechar.Location = new System.Drawing.Point(662, 500);
            this.buttonFechar.Name = "buttonFechar";
            this.buttonFechar.Size = new System.Drawing.Size(110, 30);
            this.buttonFechar.TabIndex = 5;
            this.buttonFechar.Text = "Fechar";
            this.buttonFechar.UseVisualStyleBackColor = false;
            this.buttonFechar.Click += new System.EventHandler(this.buttonFechar_Click);
            // 
            // labelPedido
            // 
            this.labelPedido.AutoSize = true;
            this.labelPedido.Font = new System.Drawing.Font("Montserrat", 8.999999F);
            this.labelPedido.ForeColor = System.Drawing.Color.White;
            this.labelPedido.Location = new System.Drawing.Point(12, 15);
            this.labelPedido.Name = "labelPedido";
            this.labelPedido.Size = new System.Drawing.Size(73, 15);
            this.labelPedido.TabIndex = 6;
            this.labelPedido.Text = "Pedido: #";
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Montserrat", 8.999999F);
            this.labelStatus.ForeColor = System.Drawing.Color.Silver;
            this.labelStatus.Location = new System.Drawing.Point(12, 508);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(151, 15);
            this.labelStatus.TabIndex = 7;
            this.labelStatus.Text = "Nenhuma imagem.";
            // 
            // Anexos_Pedido
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(784, 541);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelPedido);
            this.Controls.Add(this.buttonFechar);
            this.Controls.Add(this.buttonExcluir);
            this.Controls.Add(this.buttonAdicionar);
            this.Controls.Add(this.listViewImagens);
            this.Font = new System.Drawing.Font("Montserrat", 8.999999F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Anexos_Pedido";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Anexos do Pedido";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewImagens;
        private System.Windows.Forms.ImageList imageListImagens;
        private System.Windows.Forms.Button buttonAdicionar;
        private System.Windows.Forms.Button buttonExcluir;
        private System.Windows.Forms.Button buttonFechar;
        private System.Windows.Forms.Label labelPedido;
        private System.Windows.Forms.Label labelStatus;
    }
}
