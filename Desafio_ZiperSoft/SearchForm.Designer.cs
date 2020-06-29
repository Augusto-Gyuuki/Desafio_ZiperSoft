namespace Desafio_ZiperSoft
{
    partial class SearchForm
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
            this.usersDataG = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.SearchBox = new System.Windows.Forms.TextBox();
            this.SearchBtn = new System.Windows.Forms.Button();
            this.RelatorioBtn = new System.Windows.Forms.Button();
            this.backBtn = new System.Windows.Forms.Button();
            this.ReportInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.usersDataG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReportInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // usersDataG
            // 
            this.usersDataG.AllowUserToAddRows = false;
            this.usersDataG.AllowUserToDeleteRows = false;
            this.usersDataG.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usersDataG.GridColor = System.Drawing.SystemColors.ControlLight;
            this.usersDataG.Location = new System.Drawing.Point(12, 70);
            this.usersDataG.Name = "usersDataG";
            this.usersDataG.RowTemplate.Height = 24;
            this.usersDataG.Size = new System.Drawing.Size(912, 368);
            this.usersDataG.TabIndex = 0;
            this.usersDataG.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.usersDataG_CellValueChanged);
            this.usersDataG.DoubleClick += new System.EventHandler(this.usersDataG_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.label1.Location = new System.Drawing.Point(100, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "Pesquisar Usuarios";
            // 
            // SearchBox
            // 
            this.SearchBox.ForeColor = System.Drawing.Color.Gray;
            this.SearchBox.Location = new System.Drawing.Point(383, 27);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(356, 22);
            this.SearchBox.TabIndex = 3;
            this.SearchBox.Text = "Pesquise por CPF OU CNPJ OU Endereço OU Nome";
            this.SearchBox.Enter += new System.EventHandler(this.SearchBox_Enter);
            this.SearchBox.Leave += new System.EventHandler(this.SearchBox_Leave);
            // 
            // SearchBtn
            // 
            this.SearchBtn.Location = new System.Drawing.Point(754, 23);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(82, 30);
            this.SearchBtn.TabIndex = 4;
            this.SearchBtn.Text = "Pesquisar";
            this.SearchBtn.UseVisualStyleBackColor = true;
            this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // RelatorioBtn
            // 
            this.RelatorioBtn.Location = new System.Drawing.Point(842, 23);
            this.RelatorioBtn.Name = "RelatorioBtn";
            this.RelatorioBtn.Size = new System.Drawing.Size(82, 30);
            this.RelatorioBtn.TabIndex = 4;
            this.RelatorioBtn.Text = "Relatorio";
            this.RelatorioBtn.UseVisualStyleBackColor = true;
            this.RelatorioBtn.Click += new System.EventHandler(this.RelatorioBtn_Click);
            // 
            // backBtn
            // 
            this.backBtn.Location = new System.Drawing.Point(12, 23);
            this.backBtn.Name = "backBtn";
            this.backBtn.Size = new System.Drawing.Size(82, 30);
            this.backBtn.TabIndex = 4;
            this.backBtn.Text = "Voltar";
            this.backBtn.UseVisualStyleBackColor = true;
            this.backBtn.Click += new System.EventHandler(this.backBtn_Click);
            // 
            // ReportInfoBindingSource
            // 
            this.ReportInfoBindingSource.DataSource = typeof(Desafio_ZiperSoft.SearchForm.ReportInfo);
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 450);
            this.Controls.Add(this.backBtn);
            this.Controls.Add(this.RelatorioBtn);
            this.Controls.Add(this.SearchBtn);
            this.Controls.Add(this.SearchBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.usersDataG);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchForm";
            this.Text = "Pesquisa";
            this.Load += new System.EventHandler(this.SearchForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.usersDataG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReportInfoBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView usersDataG;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SearchBox;
        private System.Windows.Forms.Button SearchBtn;
        private System.Windows.Forms.Button RelatorioBtn;
        private System.Windows.Forms.Button backBtn;
        private System.Windows.Forms.BindingSource ReportInfoBindingSource;
    }
}