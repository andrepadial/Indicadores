using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CriptorEngine;
using System.Configuration;

namespace Indicadores
{
    public partial class frmTeste : Form
    {
        public frmTeste()
        {
            InitializeComponent();
        }

        private void frmTeste_Load(object sender, EventArgs e)
        {            
            string teste = CriptorEngine.CriptorEngine.Encrypt(ConfigurationManager.AppSettings["conexaoBD"], true);
            MessageBox.Show(teste);

            string teste2 = CriptorEngine.CriptorEngine.Decrypt(teste, true);
            MessageBox.Show(teste2);

        }
    }
}
