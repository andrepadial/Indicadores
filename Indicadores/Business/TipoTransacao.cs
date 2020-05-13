using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicadores.Business
{ 
    public class TipoTransacao
    {

        public string Tipo { set; get; }


        public TipoTransacao()
        {

        }

        public TipoTransacao(string tp)
        {
            this.Tipo = tp;
        }

    }
}
