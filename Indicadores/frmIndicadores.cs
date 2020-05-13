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
    public partial class frmIndicadores : Form
    {
        public frmIndicadores()
        {
            InitializeComponent();
        }

        private void frmIndicadores_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "All files (*.*)|*.*";
            System.Windows.Forms.ToolTip ToolTip2 = new System.Windows.Forms.ToolTip();
            ToolTip2.SetToolTip(this.btnCSV, "Exportar p/ CSV");
            rbMaster.Checked = true;
            rbdCredito.Checked = true;
            checkBox1.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            if (validacoesForm())
            {
                if (rbMaster.Checked)
                {
                    if (rbdCredito.Checked)
                        processarMovimentoCreditoMaster();
                    else
                        processarMovimentoDebitoMaster();
                }
                else
                {
                    if (rbdCredito.Checked)
                        processarMovimentoCreditoVisa();
                    else
                        processarMovimentosDebitoVisa();
                }
            }
        }

        private void btnCSV_Click(object sender, EventArgs e)
        {            
            if (rbMaster.Checked)
            {
                if (rbdCredito.Checked)
                    gerarArquivosCreditoMaster();
                else
                    gerarArquivosDebitoMaster();
            }
            else
            {
                if (rbdCredito.Checked)
                    gerarArquivosCreditoVisa();
                else
                    gerarArquivosDebitoVisa();
            }

        }


        #region ########## MASTERCARD CREDITO ##########

        private void processarMovimentoCreditoMaster()
        {
            List<Business.Master.Credito.Movimento> movtos = new List<Business.Master.Credito.Movimento>();
            string arquivos = String.Empty;

            if (openFileDialog1.FileName.ToUpper().Contains("OPENFILEDIALOG")
                || openFileDialog1.FileName.Length == 0)
            {
                MessageBox.Show("Selecione arquivo para processamento.", "Leitura de arquivo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                foreach (string path in openFileDialog1.FileNames)
                {
                    //movtos = Business.Master.Credito.Movimento.getMovimentos(openFileDialog1.FileName);

                    if (validaArquivoMaster(path))
                    {
                        movtos.AddRange(Business.Master.Credito.Movimento.getMovimentos(path));
                        arquivos = String.Concat(arquivos, path, Environment.NewLine);
                    }
                    else
                    {
                        MessageBox.Show("O arquivo: " + path + " não é bandeira Mastercard.", "Processamento de arquivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }

                DateTime data = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
                DateTime dataFim = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);

                //dataGridView1.DataSource = !checkBox1.Checked ? movtos : movtos.Where(x => x.Data == data).ToList<Business.Master.Credito.Movimento>();
                dataGridView1.DataSource = !checkBox1.Checked ? movtos : movtos.Where(x => x.Data >= data).Where(x => x.Data <= dataFim).ToList<Business.Master.Credito.Movimento>();
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                richTextBox1.Text = arquivos;                
            }
        }

        private void gerarArquivosCreditoMaster()
        {
            Cursor = Cursors.WaitCursor;
            List<Business.Master.Credito.Movimento> movimentos = new List<Business.Master.Credito.Movimento>();
            DateTime data = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
            DateTime dataFim = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);

            //dataGridView1.DataSource = !checkBox1.Checked ? movtos : movtos.Where(x => x.Data >= data).Where(x => x.Data <= dataFim).ToList<Business.Master.Credito.Movimento>();

            try
            {
                foreach (string path in openFileDialog1.FileNames)
                {

                    if (validaArquivoMaster(path))
                    {
                        movimentos = Business.Master.Credito.Movimento.getMovimentos(path);
                        string arquivo = openFileDialog1.FileNames[0].Substring(0, openFileDialog1.FileNames[0].LastIndexOf("\\")) + @"\\MasterCredito" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv";

                        if (movimentos.Count > 0)
                        {
                            if (!checkBox1.Checked)
                                Rendimento.Layout.CSV.exportObjectToCSV<Business.Master.Credito.Movimento>(movimentos, ConfigurationManager.AppSettings["delimitadorExport"], path + "Credito_");
                            else
                            {
                                List<Business.Master.Credito.Movimento> movtos = new List<Business.Master.Credito.Movimento>();
                                //movtos = movimentos.Where(x => x.Data == data).ToList<Business.Master.Credito.Movimento>();
                                movtos = movimentos.Where(x => x.Data >= data).Where(x => x.Data <= dataFim).ToList<Business.Master.Credito.Movimento>();
                                Rendimento.Layout.CSV.exportObjectToCSV<Business.Master.Credito.Movimento>(movtos, ConfigurationManager.AppSettings["delimitadorExport"], path + "Credito_");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("O arquivo: " + path + " não é bandeira Mastercard.", "Processamento de arquivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }

                gerarArquivoGeralCreditoMaster();
                MessageBox.Show("Arquivos gerados com sucesso.", "Geração de arquivos.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar arquivos para CSV: " + ex.Message.ToString(), "Exportação de arquivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            Cursor = Cursors.Default;
        }

        private void gerarArquivoGeralCreditoMaster()
        {
            Cursor = Cursors.WaitCursor;
            List<Business.Master.Credito.Movimento> movimentos = new List<Business.Master.Credito.Movimento>();

            string arquivo = openFileDialog1.FileNames[0].Substring(0, openFileDialog1.FileNames[0].LastIndexOf("\\")) + @"\\geral_MasterCredito" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv";
            DateTime data = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
            DateTime dataFim = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);
            //string arquivo = caminho.Substring(caminho.LastIndexOf("\\") + 1);

            try
            {
                foreach (string path in openFileDialog1.FileNames)
                {
                    if (validaArquivoMaster(path))
                        movimentos.AddRange(Business.Master.Credito.Movimento.getMovimentos(path));
                    else
                    {
                        MessageBox.Show("O arquivo: " + path + " não é bandeira Mastercard.", "Processamento de arquivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }

                if (movimentos.Count > 0)
                {
                    
                    if (!checkBox1.Checked)
                        Rendimento.Layout.CSV.exportObjectToCSV<Business.Master.Credito.Movimento>(movimentos, ConfigurationManager.AppSettings["delimitadorExport"], arquivo);                    
                    else
                    {
                        List<Business.Master.Credito.Movimento> movtos = new List<Business.Master.Credito.Movimento>();
                        //movtos = movimentos.Where(x => x.Data == data).ToList<Business.Master.Credito.Movimento>();
                        movtos = movimentos.Where(x => x.Data >= data).Where(x => x.Data <= dataFim).ToList<Business.Master.Credito.Movimento>();
                        Rendimento.Layout.CSV.exportObjectToCSV<Business.Master.Credito.Movimento>(movtos, ConfigurationManager.AppSettings["delimitadorExport"], arquivo);
                    }
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar arquivos para CSV: " + ex.Message.ToString(), "Exportação de arquivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            Cursor = Cursors.Default;
        }


        #endregion

        #region ########## MASTERCARD DEBITO ##########


        private void processarMovimentoDebitoMaster()
        {
            List<Business.Master.Debito.Movimento> movtos = new List<Business.Master.Debito.Movimento>();
            string arquivos = String.Empty;

            if (openFileDialog1.FileName.ToUpper().Contains("OPENFILEDIALOG")
                || openFileDialog1.FileName.Length == 0)
            {
                MessageBox.Show("Selecione arquivo para processamento.", "Leitura de arquivo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                foreach (string path in openFileDialog1.FileNames)
                {
                    //movtos = Business.Master.Credito.Movimento.getMovimentos(openFileDialog1.FileName);

                    if (validaArquivoMaster(path))
                    {
                        movtos.AddRange(Business.Master.Debito.Movimento.getMovimentos(path));
                        arquivos = String.Concat(arquivos, path, Environment.NewLine);
                    }
                    else
                    {
                        MessageBox.Show("O arquivo: " + path + " não é bandeira Mastercard.", "Processamento de arquivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }

                DateTime data = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
                DateTime dataFim = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);

                //dataGridView1.DataSource = !checkBox1.Checked ? movtos : movtos.Where(x => x.Data == data).ToList<Business.Master.Debito.Movimento>(); 
                dataGridView1.DataSource = !checkBox1.Checked ? movtos : movtos.Where(x => x.Data >= data).Where(x => x.Data <= dataFim).ToList<Business.Master.Debito.Movimento>();
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                richTextBox1.Text = arquivos;                
            }
        }

        private void gerarArquivosDebitoMaster()
        {
            Cursor = Cursors.WaitCursor;
            List<Business.Master.Debito.Movimento> movimentos = new List<Business.Master.Debito.Movimento>();
            DateTime data = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
            DateTime dataFim = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);

            try
            {
                foreach (string path in openFileDialog1.FileNames)
                {

                    if (validaArquivoMaster(path))
                    {
                        movimentos = Business.Master.Debito.Movimento.getMovimentos(path);

                        if (movimentos.Count > 0)
                        {
                            if (!checkBox1.Checked)
                                Rendimento.Layout.CSV.exportObjectToCSV<Business.Master.Debito.Movimento>(movimentos, ConfigurationManager.AppSettings["delimitadorExport"], path + "Debito_");
                            else
                            {
                                List<Business.Master.Debito.Movimento> movtos = new List<Business.Master.Debito.Movimento>();
                                movtos = movimentos.Where(x => x.Data == data).ToList<Business.Master.Debito.Movimento>();
                                Rendimento.Layout.CSV.exportObjectToCSV<Business.Master.Debito.Movimento>(movtos, ConfigurationManager.AppSettings["delimitadorExport"], path + "Debito_");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("O arquivo: " + path + " não é bandeira Mastercard.", "Processamento de arquivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }

                gerarArquivoGeralDebitoMaster();
                MessageBox.Show("Arquivos gerados com sucesso.", "Geração de arquivos.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar arquivos para CSV: " + ex.Message.ToString(), "Exportação de arquivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            Cursor = Cursors.Default;
        }

        private void gerarArquivoGeralDebitoMaster()
        {
            Cursor = Cursors.WaitCursor;
            List<Business.Master.Debito.Movimento> movimentos = new List<Business.Master.Debito.Movimento>();

            string arquivo = openFileDialog1.FileNames[0].Substring(0, openFileDialog1.FileNames[0].LastIndexOf("\\")) + @"\\geral_MasterDebito" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv";
            DateTime data = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
            DateTime dataFim = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);
            

            try
            {
                foreach (string path in openFileDialog1.FileNames)
                {
                    if(validaArquivoMaster(path))
                        movimentos.AddRange(Business.Master.Debito.Movimento.getMovimentos(path));
                    else
                    {
                        MessageBox.Show("O arquivo: " + path + " não é bandeira Mastercard.", "Processamento de arquivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }

                if (movimentos.Count > 0)
                {
                    if(!checkBox1.Checked)
                        Rendimento.Layout.CSV.exportObjectToCSV<Business.Master.Debito.Movimento>(movimentos, ConfigurationManager.AppSettings["delimitadorExport"], arquivo);
                    else
                    {
                        List<Business.Master.Debito.Movimento> movtos = new List<Business.Master.Debito.Movimento>();
                        //movtos = movimentos.Where(x => x.Data == data).ToList<Business.Master.Debito.Movimento>();
                        movtos = movimentos.Where(x => x.Data >= data).Where(x => x.Data <= dataFim).ToList<Business.Master.Debito.Movimento>();
                        Rendimento.Layout.CSV.exportObjectToCSV<Business.Master.Debito.Movimento>(movtos, ConfigurationManager.AppSettings["delimitadorExport"], arquivo);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar arquivos para CSV: " + ex.Message.ToString(), "Exportação de arquivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            Cursor = Cursors.Default;
        }


        #endregion

        #region ########## VISA CREDITO ##########

        private void processarMovimentoCreditoVisa()
        {
            List<Business.Visa.Credito.Movimento> movtos = new List<Business.Visa.Credito.Movimento>();
            string arquivos = String.Empty;

            if (openFileDialog1.FileName.ToUpper().Contains("OPENFILEDIALOG")
                || openFileDialog1.FileName.Length == 0)
            {
                MessageBox.Show("Selecione arquivo para processamento.", "Leitura de arquivo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                foreach (string path in openFileDialog1.FileNames)
                {
                    //movtos = Business.Master.Credito.Movimento.getMovimentos(openFileDialog1.FileName);

                    if (validaArquivoVisa(path))
                    {
                        movtos.AddRange(Business.Visa.Credito.Movimento.getMovimentos(path));
                        arquivos = String.Concat(arquivos, path, Environment.NewLine);
                    }
                    else
                    {
                        MessageBox.Show("O arquivo: " + path + " não é bandeira Visa.", "Processamento de arquivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }

                DateTime data = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
                DateTime dataFim = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);

                //dataGridView1.DataSource = !checkBox1.Checked ? movtos : movtos.Where(x => x.ProcessingDate == data).ToList<Business.Visa.Credito.Movimento>();
                dataGridView1.DataSource = !checkBox1.Checked ? movtos : movtos.Where(x => x.ProcessingDate >= data).Where(x => x.ProcessingDate <= dataFim).ToList<Business.Visa.Credito.Movimento>();
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                richTextBox1.Text = arquivos;                
            }
        }

        private void gerarArquivosCreditoVisa()
        {
            Cursor = Cursors.WaitCursor;
            List<Business.Visa.Credito.Movimento> movimentos = new List<Business.Visa.Credito.Movimento>();
            DateTime data = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
            DateTime dataFim = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);

            try
            {
                foreach (string path in openFileDialog1.FileNames)
                {
                    if (validaArquivoVisa(path))
                    {
                        movimentos = Business.Visa.Credito.Movimento.getMovimentos(path);

                        if (movimentos.Count > 0)
                        {
                            if (!checkBox1.Checked)
                                Rendimento.Layout.CSV.exportObjectToCSV<Business.Visa.Credito.Movimento>(movimentos, ConfigurationManager.AppSettings["delimitadorExport"], path + "Credito_");
                            else
                            {
                                List<Business.Visa.Credito.Movimento> movtos = new List<Business.Visa.Credito.Movimento>();
                                //movtos = movimentos.Where(x => x.ProcessingDate == data).ToList<Business.Visa.Credito.Movimento>();
                                movtos = movimentos.Where(x => x.ProcessingDate >= data).Where(x => x.ProcessingDate <= dataFim).ToList<Business.Visa.Credito.Movimento>();
                                Rendimento.Layout.CSV.exportObjectToCSV<Business.Visa.Credito.Movimento>(movtos, ConfigurationManager.AppSettings["delimitadorExport"], path + "Credito_");
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("O arquivo: " + path + " não é bandeira Visa.", "Processamento de arquivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }

                gerarArquivoGeralCreditoVisa();
                MessageBox.Show("Arquivos gerados com sucesso.", "Geração de arquivos.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar arquivos para CSV: " + ex.Message.ToString(), "Exportação de arquivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            Cursor = Cursors.Default;
        }

        private void gerarArquivoGeralCreditoVisa()
        {
            Cursor = Cursors.WaitCursor;
            List<Business.Visa.Credito.Movimento> movimentos = new List<Business.Visa.Credito.Movimento>();
            DateTime data = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
            DateTime dataFim = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);
            string arquivo = openFileDialog1.FileNames[0].Substring(0, openFileDialog1.FileNames[0].LastIndexOf("\\")) + @"\\geral_MasterCredito" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv";
            //string arquivo = caminho.Substring(caminho.LastIndexOf("\\") + 1);

            try
            {
                foreach (string path in openFileDialog1.FileNames)
                {
                    if(validaArquivoVisa(path))
                        movimentos.AddRange(Business.Visa.Credito.Movimento.getMovimentos(path));
                    else
                    {
                        MessageBox.Show("O arquivo: " + path + " não é bandeira Visa.", "Processamento de arquivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }

                if (movimentos.Count > 0)
                {

                    if(!checkBox1.Checked)
                        Rendimento.Layout.CSV.exportObjectToCSV<Business.Visa.Credito.Movimento>(movimentos, ConfigurationManager.AppSettings["delimitadorExport"], arquivo);
                    else
                    {
                        List<Business.Visa.Credito.Movimento> movtos = new List<Business.Visa.Credito.Movimento>();
                        //movtos = movimentos.Where(x => x.ProcessingDate == data).ToList<Business.Visa.Credito.Movimento>();
                        movtos = movimentos.Where(x => x.ProcessingDate >= data).Where(x => x.ProcessingDate <= dataFim).ToList<Business.Visa.Credito.Movimento>();
                        Rendimento.Layout.CSV.exportObjectToCSV<Business.Visa.Credito.Movimento>(movtos, ConfigurationManager.AppSettings["delimitadorExport"], arquivo);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar arquivos para CSV: " + ex.Message.ToString(), "Exportação de arquivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            Cursor = Cursors.Default;
        }


        #endregion

        #region ######### VISA DEBITO ############

        private void processarMovimentosDebitoVisa()
        {
            List<Business.Visa.Debito.Movimento> movtos = new List<Business.Visa.Debito.Movimento>();
            string arquivos = String.Empty;

            if (openFileDialog1.FileName.ToUpper().Contains("OPENFILEDIALOG")
                || openFileDialog1.FileName.Length == 0)
            {
                MessageBox.Show("Selecione arquivo para processamento.", "Leitura de arquivo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                foreach (string path in openFileDialog1.FileNames)
                {
                    //movtos = Business.Master.Credito.Movimento.getMovimentos(openFileDialog1.FileName);

                    if (validaArquivoVisa(path))
                    {
                        movtos.AddRange(Business.Visa.Debito.Movimento.getTransactions(path));
                        arquivos = String.Concat(arquivos, path, Environment.NewLine);
                    }
                    else
                    {
                        MessageBox.Show("O arquivo: " + path + " não é bandeira Visa.", "Processamento de arquivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }

                DateTime data = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
                DateTime dataFim = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);

                //dataGridView1.DataSource = !checkBox1.Checked ? movtos : movtos.Where(x => x.ProcessingDate == data).ToList<Business.Visa.Debito.Movimento>();
                dataGridView1.DataSource = !checkBox1.Checked ? movtos : movtos.Where(x => x.ProcessingDate >= data).Where(x => x.ProcessingDate <= dataFim).ToList<Business.Visa.Debito.Movimento>();
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                richTextBox1.Text = arquivos;                
            }
        }

        private void gerarArquivosDebitoVisa()
        {
            Cursor = Cursors.WaitCursor;
            List<Business.Visa.Debito.Movimento> movimentos = new List<Business.Visa.Debito.Movimento>();
            DateTime data = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
            DateTime dataFim = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);

            try
            {
                foreach (string path in openFileDialog1.FileNames)
                {
                    if (validaArquivoVisa(path))
                    {
                        movimentos = Business.Visa.Debito.Movimento.getTransactions(path);

                        if (movimentos.Count > 0)
                        {
                            if (!checkBox1.Checked)
                                Rendimento.Layout.CSV.exportObjectToCSV<Business.Visa.Debito.Movimento>(movimentos, ConfigurationManager.AppSettings["delimitadorExport"], path + "Debito_");
                            else
                            {
                                List<Business.Visa.Debito.Movimento> movtos = new List<Business.Visa.Debito.Movimento>();
                                //movtos = movimentos.Where(x => x.ProcessingDate == data).ToList<Business.Visa.Debito.Movimento>();
                                movtos = movimentos.Where(x => x.ProcessingDate >= data).Where(x => x.ProcessingDate <= dataFim).ToList<Business.Visa.Debito.Movimento>();
                                Rendimento.Layout.CSV.exportObjectToCSV<Business.Visa.Debito.Movimento>(movtos, ConfigurationManager.AppSettings["delimitadorExport"], path + "Debito_");
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("O arquivo: " + path + " não é bandeira Visa.", "Processamento de arquivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }

                gerarArquivoGeralDebitoVisa();
                MessageBox.Show("Arquivos gerados com sucesso.", "Geração de arquivos.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar arquivos para CSV: " + ex.Message.ToString(), "Exportação de arquivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            Cursor = Cursors.Default;
        }

        private void gerarArquivoGeralDebitoVisa()
        {
            Cursor = Cursors.WaitCursor;
            List<Business.Visa.Debito.Movimento> movimentos = new List<Business.Visa.Debito.Movimento>();
            DateTime data = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
            DateTime dataFim = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);
            string arquivo = openFileDialog1.FileNames[0].Substring(0, openFileDialog1.FileNames[0].LastIndexOf("\\")) + @"\\geral_MasterDebito" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv";
            //string arquivo = caminho.Substring(caminho.LastIndexOf("\\") + 1);

            try
            {
                foreach (string path in openFileDialog1.FileNames)
                {
                    if (validaArquivoVisa(path))
                        movimentos.AddRange(Business.Visa.Debito.Movimento.getTransactions(path));
                    else
                    {
                        MessageBox.Show("O arquivo: " + path + " não é bandeira Visa.", "Processamento de arquivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }

                if (movimentos.Count > 0)
                {

                    if (!checkBox1.Checked)
                        Rendimento.Layout.CSV.exportObjectToCSV<Business.Visa.Debito.Movimento>(movimentos, ConfigurationManager.AppSettings["delimitadorExport"], arquivo);
                    else
                    {
                        List<Business.Visa.Debito.Movimento> movtos = new List<Business.Visa.Debito.Movimento>();
                        //movtos = movimentos.Where(x => x.ProcessingDate == data).ToList<Business.Visa.Debito.Movimento>();
                        movtos = movimentos.Where(x => x.ProcessingDate >= data).Where(x => x.ProcessingDate <= dataFim).ToList<Business.Visa.Debito.Movimento>();
                        Rendimento.Layout.CSV.exportObjectToCSV<Business.Visa.Debito.Movimento>(movtos, ConfigurationManager.AppSettings["delimitadorExport"], arquivo);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar arquivos para CSV: " + ex.Message.ToString(), "Exportação de arquivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            Cursor = Cursors.Default;
        }


        #endregion

        private bool validaArquivoVisa(string path)
        {
            bool valida = false;
            string[] linhas = File.ReadAllLines(path);

            var linha = (from l in linhas
                         where l.Contains("VISANET")
                         select l).ToList<string>();

            if (linha.Count > 0)
                valida = true;

            return valida;
        }

        private bool validaArquivoMaster(string path)
        {
            bool valida = false;
            string[] linhas = File.ReadAllLines(path);

            var linha = (from l in linhas
                         where l.Contains("MASTERCARD")
                         select l).ToList<string>();

            if (linha.Count > 0)
                valida = true;

            return valida;
        }

        private bool validacoesForm()
        {
            bool valida = true;

            DateTime data = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
            DateTime dataFim = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);

            if (checkBox1.Checked)
            {
                if (data > dataFim)
                {
                    valida = false;
                    MessageBox.Show("Data inicial maior que a data final.", "Filtro de datas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            return valida;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //this.button1.PerformClick();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            richTextBox1.Text = "";
        }
    }
}
