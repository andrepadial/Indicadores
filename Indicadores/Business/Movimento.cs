using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicadores.Business
{
    public class Movimento
    {

        #region ********** ATRIBUTOS **********
        public string Tipo { set; get; }

        public string Classe { set; get; }

        public string Status { set; get; }

        public string Categoria { set; get; }

        

        #endregion


        public Movimento()
        {
        }

        public Movimento(string tp, string cl, string st, string cat)
        {
            this.Tipo = tp;
            this.Classe = cl;
            this.Status = st;
            this.Categoria = cat;            
        }

    }
}
