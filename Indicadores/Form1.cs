using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Indicadores
{
    public partial class Form1 : Form
    {

        public List<Business.Movimento> Movimentos = null;

        public List<Business.Movimento> MovimentosResumo = null;

        public Form1()
        {
            InitializeComponent();
            rbChargesFee.Checked = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            System.Windows.Forms.ToolTip ToolTip2 = new System.Windows.Forms.ToolTip();
            ToolTip2.SetToolTip(this.btnCSV, "Exportar p/ CSV");
        }

        private void importarCSV(string path)
        {
            Rendimento.Layout.CSV teste = new Rendimento.Layout.CSV();
            DataTable dt = teste.converterCSVToDatatable(path, Convert.ToChar(ConfigurationManager.AppSettings["delimitador"].ToString()));

            if (rbChargesFee.Checked)
                getChargeFees(dt);
            else if (rbRefunds.Checked)
            {
                getTransactionRefunds(dt);
            }
            else if (rbdChargeBack.Checked)
            {
                getTransactionsChargeBack(dt);                
            }
            else
            {                
            }
            
        }

        
        private void getChargeFees(DataTable dt)
        {
            List<Business.ChargeFee> cf = new List<Business.ChargeFee>();
            cf = Business.ChargeFee.getTransactionsChargeFee(dt);
            dataGridView1.DataSource = cf;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

              
        private void getTransactionRefunds(DataTable dt)
        {

            List<Business.Refund> cf = new List<Business.Refund>();
            cf = Business.Refund.getTransactionRefunds(dt);
            dataGridView1.DataSource = cf;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void getTransactionsChargeBack(DataTable dt)
        {
            List<Business.ChargeBack> cf = new List<Business.ChargeBack>();
            cf = Business.ChargeBack.getTransactionsChargeBack(dt);
            dataGridView1.DataSource = cf;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
       

        private void btnProcessar_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            this.Movimentos = new List<Business.Movimento>();
            this.MovimentosResumo = new List<Business.Movimento>();

            if (openFileDialog1.FileName.ToString().Length == 0 || openFileDialog1.FileName.ToString().Contains("openFileDialog"))
            {
                MessageBox.Show("Selecione arquivo para processamento", "Processamento de arquivo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                importarCSV(openFileDialog1.FileName);
            }
        }

        private void rbChargesFee_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void btnCSV_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            bool valida = false;
            
            if (this.Movimentos.Count == 0 || this.MovimentosResumo.Count == 0)
            {
                MessageBox.Show("Não há movimentos a serem gerados.", "Movimentos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                valida = exportCSV();

                if (valida)
                    MessageBox.Show("Arquivos gerados com sucesso.", "Geração de arquivo.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Arquivos não foram gerados.", "Geração de arquivo.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }           

            
            Cursor = Cursors.Default;
        }

        private bool exportCSV()
        {
            bool valida = false, validaResumo = false, validaRet = false;
            string fileName = String.Empty;
            string fileNameResumo = String.Empty;


            if (rbChargesFee.Checked)
            {
                fileName = openFileDialog1.FileName.Replace(".csv", "") + "_changeFeesAnalitico_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv";
                fileNameResumo = openFileDialog1.FileName.Replace(".csv", "") + "_changeFeesSint_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv";
            }
            else if (rbRefunds.Checked)
            {
                fileName = openFileDialog1.FileName.Replace(".csv", "") + "_refundsAnalitico_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv";
                fileNameResumo = openFileDialog1.FileName.Replace(".csv", "") + "_refundsSint_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv";
            }
            else if(rbdChargeBack.Checked)
            {
                fileName = openFileDialog1.FileName.Replace(".csv", "") + "_chargeBackAnalitico_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv";
                fileNameResumo = openFileDialog1.FileName.Replace(".csv", "") + "_chargeBacksSint_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv";
            }
            else
            {
                fileName = openFileDialog1.FileName.Replace(".csv", "") + "_Analitico_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv";
                fileNameResumo = openFileDialog1.FileName.Replace(".csv", "") + "_Sintetico_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv";
            }
                        

            valida = Rendimento.Layout.CSV.exportObjectToCSV<Business.Movimento>(this.Movimentos, ConfigurationManager.AppSettings["delimitadorExport"].ToString(), fileName);
            validaResumo = Rendimento.Layout.CSV.exportObjectToCSV<Business.Movimento>(this.MovimentosResumo, ConfigurationManager.AppSettings["delimitadorExport"].ToString(), fileNameResumo);

            if (valida && validaResumo)
                validaRet = true;

            return validaRet;
        }
        
    }
}
