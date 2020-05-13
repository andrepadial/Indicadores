using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicadores.Business
{
    public class Refund: Movimento
    {

        public double ValorAmountGR { set; get; }
        public double ValorAmountINW { set; get; }
        public double ValorInterchange { set; get; }

        public Refund()
        {

        }

        
        public static List<Refund> getTransactionRefunds(DataTable dt)
        {
            List<Business.Refund> cf = new List<Business.Refund>();
            

            foreach (DataRow dr in dt.Rows)
            {
                double valor = Convert.ToDouble(dr["local amount gr"].ToString().Replace(".", "").Replace(",", "")) / 100;
                double valorINW = Convert.ToDouble(dr["local amount inw chg"].ToString().Replace(".", "").Replace(",", "")) / 100;
                double valorInter = Convert.ToDouble(dr["local amount out chg"].ToString().Replace(".", "").Replace(",", "")) / 100;

                Business.Refund re = new Business.Refund();
                re.Tipo = dr["Type"].ToString();
                re.Classe = dr["Class"].ToString();
                re.Status = dr["Status"].ToString();
                re.Categoria = dr["Category"].ToString();                
                re.ValorAmountGR = Math.Round(valor, 2);
                re.ValorAmountINW = Math.Round(valorINW, 2);
                re.ValorInterchange = Math.Round(valorInter, 2);
                cf.Add(re);
            }


            var tipos = new List<string> { "Purch with Install 7 +", "Purch with Install 4 to 6", "Purchase", "Purchase with Install 2 to 3", "Refund (Credit)" };

            var resultado = (from l in cf
                             let value = l.Tipo
                             where tipos.Contains(value)
                             where l.Classe.ToUpper() == "CLEARING TRANSACTIONS"
                             where l.Status.ToUpper() == "CLEARED"
                             where l.Categoria.ToUpper() == "PRESENTMENTS"
                             select l).ToList<Business.Refund>();
            

            resultado = resultado.OrderBy(x => x.Tipo).ToList<Business.Refund>();

            return resultado;            
        }

    }
}
