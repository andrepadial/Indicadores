namespace Indicadores
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnProcessar = new System.Windows.Forms.Button();
            this.gpIndicador = new System.Windows.Forms.GroupBox();
            this.rbdAll = new System.Windows.Forms.RadioButton();
            this.rbdChargeBack = new System.Windows.Forms.RadioButton();
            this.rbRefunds = new System.Windows.Forms.RadioButton();
            this.rbChargesFee = new System.Windows.Forms.RadioButton();
            this.lblInfo = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnCSV = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.gpIndicador.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnProcessar
            // 
            this.btnProcessar.Location = new System.Drawing.Point(426, 56);
            this.btnProcessar.Name = "btnProcessar";
            this.btnProcessar.Size = new System.Drawing.Size(75, 23);
            this.btnProcessar.TabIndex = 1;
            this.btnProcessar.Text = "&Processar";
            this.btnProcessar.UseVisualStyleBackColor = true;
            this.btnProcessar.Click += new System.EventHandler(this.btnProcessar_Click);
            // 
            // gpIndicador
            // 
            this.gpIndicador.Controls.Add(this.rbdAll);
            this.gpIndicador.Controls.Add(this.rbdChargeBack);
            this.gpIndicador.Controls.Add(this.rbRefunds);
            this.gpIndicador.Controls.Add(this.rbChargesFee);
            this.gpIndicador.Location = new System.Drawing.Point(12, 12);
            this.gpIndicador.Name = "gpIndicador";
            this.gpIndicador.Size = new System.Drawing.Size(408, 67);
            this.gpIndicador.TabIndex = 2;
            this.gpIndicador.TabStop = false;
            this.gpIndicador.Text = "Tipo Indicador";
            // 
            // rbdAll
            // 
            this.rbdAll.AutoSize = true;
            this.rbdAll.Location = new System.Drawing.Point(343, 27);
            this.rbdAll.Name = "rbdAll";
            this.rbdAll.Size = new System.Drawing.Size(55, 17);
            this.rbdAll.TabIndex = 3;
            this.rbdAll.TabStop = true;
            this.rbdAll.Text = "Todos";
            this.rbdAll.UseVisualStyleBackColor = true;
            // 
            // rbdChargeBack
            // 
            this.rbdChargeBack.AutoSize = true;
            this.rbdChargeBack.Location = new System.Drawing.Point(254, 27);
            this.rbdChargeBack.Name = "rbdChargeBack";
            this.rbdChargeBack.Size = new System.Drawing.Size(83, 17);
            this.rbdChargeBack.TabIndex = 2;
            this.rbdChargeBack.TabStop = true;
            this.rbdChargeBack.Text = "Chargeback";
            this.rbdChargeBack.UseVisualStyleBackColor = true;
            // 
            // rbRefunds
            // 
            this.rbRefunds.AutoSize = true;
            this.rbRefunds.Location = new System.Drawing.Point(111, 27);
            this.rbRefunds.Name = "rbRefunds";
            this.rbRefunds.Size = new System.Drawing.Size(137, 17);
            this.rbRefunds.TabIndex = 1;
            this.rbRefunds.TabStop = true;
            this.rbRefunds.Text = "Transactions / Refunds";
            this.rbRefunds.UseVisualStyleBackColor = true;
            // 
            // rbChargesFee
            // 
            this.rbChargesFee.AutoSize = true;
            this.rbChargesFee.Location = new System.Drawing.Point(6, 27);
            this.rbChargesFee.Name = "rbChargesFee";
            this.rbChargesFee.Size = new System.Drawing.Size(99, 17);
            this.rbChargesFee.TabIndex = 0;
            this.rbChargesFee.TabStop = true;
            this.rbChargesFee.Text = "Charges a Fees";
            this.rbChargesFee.UseVisualStyleBackColor = true;
            this.rbChargesFee.CheckedChanged += new System.EventHandler(this.rbChargesFee_CheckedChanged);
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(100, 23);
            this.lblInfo.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 85);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(924, 426);
            this.dataGridView1.TabIndex = 10;
            // 
            // btnCSV
            // 
            this.btnCSV.Image = ((System.Drawing.Image)(resources.GetObject("btnCSV.Image")));
            this.btnCSV.Location = new System.Drawing.Point(507, 56);
            this.btnCSV.Name = "btnCSV";
            this.btnCSV.Size = new System.Drawing.Size(42, 23);
            this.btnCSV.TabIndex = 12;
            this.btnCSV.UseVisualStyleBackColor = true;
            this.btnCSV.Click += new System.EventHandler(this.btnCSV_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 523);
            this.Controls.Add(this.btnCSV);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.gpIndicador);
            this.Controls.Add(this.btnProcessar);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Indicadores";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gpIndicador.ResumeLayout(false);
            this.gpIndicador.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnProcessar;
        private System.Windows.Forms.GroupBox gpIndicador;
        private System.Windows.Forms.RadioButton rbChargesFee;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.RadioButton rbRefunds;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnCSV;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.RadioButton rbdChargeBack;
        private System.Windows.Forms.RadioButton rbdAll;
    }
}

