using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicadores.Business
{
    public class TipoServico
    {

        public string Tipo { set; get; }


        public TipoServico()
        {

        }

        public TipoServico (string tpServico)
        {
            this.Tipo = tpServico;
        }
    }
}
