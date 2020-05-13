namespace Indicadores
{
    partial class frmIndicadores
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIndicadores));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnCSV = new System.Windows.Forms.Button();
            this.gpBandeira = new System.Windows.Forms.GroupBox();
            this.rbdVisa = new System.Windows.Forms.RadioButton();
            this.rbMaster = new System.Windows.Forms.RadioButton();
            this.gpTipo = new System.Windows.Forms.GroupBox();
            this.rbdDebito = new System.Windows.Forms.RadioButton();
            this.rbdCredito = new System.Windows.Forms.RadioButton();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.gpBandeira.SuspendLayout();
            this.gpTipo.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Arquivo";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(663, 113);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(15, 168);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1153, 580);
            this.dataGridView1.TabIndex = 11;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(15, 99);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(642, 51);
            this.richTextBox1.TabIndex = 12;
            this.richTextBox1.Text = "";
            // 
            // btnCSV
            // 
            this.btnCSV.Image = ((System.Drawing.Image)(resources.GetObject("btnCSV.Image")));
            this.btnCSV.Location = new System.Drawing.Point(705, 113);
            this.btnCSV.Name = "btnCSV";
            this.btnCSV.Size = new System.Drawing.Size(42, 23);
            this.btnCSV.TabIndex = 13;
            this.btnCSV.UseVisualStyleBackColor = true;
            this.btnCSV.Click += new System.EventHandler(this.btnCSV_Click);
            // 
            // gpBandeira
            // 
            this.gpBandeira.Controls.Add(this.rbdVisa);
            this.gpBandeira.Controls.Add(this.rbMaster);
            this.gpBandeira.Location = new System.Drawing.Point(15, 12);
            this.gpBandeira.Name = "gpBandeira";
            this.gpBandeira.Size = new System.Drawing.Size(143, 59);
            this.gpBandeira.TabIndex = 14;
            this.gpBandeira.TabStop = false;
            this.gpBandeira.Text = "Bandeira";
            // 
            // rbdVisa
            // 
            this.rbdVisa.AutoSize = true;
            this.rbdVisa.Location = new System.Drawing.Point(90, 29);
            this.rbdVisa.Name = "rbdVisa";
            this.rbdVisa.Size = new System.Drawing.Size(45, 17);
            this.rbdVisa.TabIndex = 15;
            this.rbdVisa.TabStop = true;
            this.rbdVisa.Text = "Visa";
            this.rbdVisa.UseVisualStyleBackColor = true;
            // 
            // rbMaster
            // 
            this.rbMaster.AutoSize = true;
            this.rbMaster.Location = new System.Drawing.Point(6, 29);
            this.rbMaster.Name = "rbMaster";
            this.rbMaster.Size = new System.Drawing.Size(78, 17);
            this.rbMaster.TabIndex = 15;
            this.rbMaster.TabStop = true;
            this.rbMaster.Text = "Mastercard";
            this.rbMaster.UseVisualStyleBackColor = true;
            // 
            // gpTipo
            // 
            this.gpTipo.Controls.Add(this.rbdDebito);
            this.gpTipo.Controls.Add(this.rbdCredito);
            this.gpTipo.Location = new System.Drawing.Point(175, 12);
            this.gpTipo.Name = "gpTipo";
            this.gpTipo.Size = new System.Drawing.Size(133, 59);
            this.gpTipo.TabIndex = 15;
            this.gpTipo.TabStop = false;
            this.gpTipo.Text = "Tipo";
            // 
            // rbdDebito
            // 
            this.rbdDebito.AutoSize = true;
            this.rbdDebito.Location = new System.Drawing.Point(70, 29);
            this.rbdDebito.Name = "rbdDebito";
            this.rbdDebito.Size = new System.Drawing.Size(56, 17);
            this.rbdDebito.TabIndex = 16;
            this.rbdDebito.TabStop = true;
            this.rbdDebito.Text = "Débito";
            this.rbdDebito.UseVisualStyleBackColor = true;
            // 
            // rbdCredito
            // 
            this.rbdCredito.AutoSize = true;
            this.rbdCredito.Location = new System.Drawing.Point(6, 29);
            this.rbdCredito.Name = "rbdCredito";
            this.rbdCredito.Size = new System.Drawing.Size(58, 17);
            this.rbdCredito.TabIndex = 16;
            this.rbdCredito.TabStop = true;
            this.rbdCredito.Text = "Crédito";
            this.rbdCredito.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(314, 51);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(83, 20);
            this.dateTimePicker1.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(314, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Data Início";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(314, 15);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(83, 17);
            this.checkBox1.TabIndex = 18;
            this.checkBox1.Text = "Filtrar Data?";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(403, 51);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(79, 20);
            this.dateTimePicker2.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(403, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Data Fim";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(753, 113);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(49, 23);
            this.button2.TabIndex = 21;
            this.button2.Text = "&Limpar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmIndicadores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 795);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.gpTipo);
            this.Controls.Add(this.gpBandeira);
            this.Controls.Add(this.btnCSV);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "frmIndicadores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Indicadores";
            this.Load += new System.EventHandler(this.frmIndicadores_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.gpBandeira.ResumeLayout(false);
            this.gpBandeira.PerformLayout();
            this.gpTipo.ResumeLayout(false);
            this.gpTipo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnCSV;
        private System.Windows.Forms.GroupBox gpBandeira;
        private System.Windows.Forms.RadioButton rbdVisa;
        private System.Windows.Forms.RadioButton rbMaster;
        private System.Windows.Forms.GroupBox gpTipo;
        private System.Windows.Forms.RadioButton rbdDebito;
        private System.Windows.Forms.RadioButton rbdCredito;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
    }
}