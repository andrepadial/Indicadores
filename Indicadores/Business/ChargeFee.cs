using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicadores.Business
{
    public class ChargeFee : Movimento
    {

        public double ValorAmountGR { set; get;  }

        public ChargeFee()
        {

        }

        public static List<ChargeFee> getTransactionsChargeFee(DataTable dt)
        {
            List<Business.ChargeFee> cf = new List<Business.ChargeFee>();
            

            foreach (DataRow dr in dt.Rows)
            {
                double valor = Convert.ToDouble(dr["local amount gr"].ToString().Replace(".", "").Replace(",", "")) / 100;
                ChargeFee c = new ChargeFee();

                c.Tipo = dr["Type"].ToString();
                c.Classe = dr["Class"].ToString();
                c.Status = dr["Status"].ToString();
                c.Categoria = dr["Category"].ToString();
                c.ValorAmountGR = Math.Round(valor, 2);                
                cf.Add(c);                          
            }


            var tipos = new List<string> { "Merchant Comm. Instalments", "Merchant Comm. Dom. CR", "Merchant Comm. Int. CR " };

            var resultado = (from l in cf
                             let value = l.Tipo
                             where tipos.Contains(value)
                             where l.Classe.ToUpper() == "CLEARING TRANSACTIONS"
                             where l.Status.ToUpper() == "PAID"
                             where l.Categoria.ToUpper() == "CHARGES & FEES"
                             select l).ToList<Business.ChargeFee>();


            

            resultado = resultado.OrderBy(x => x.Tipo).ThenBy(x => x.ValorAmountGR).ToList<Business.ChargeFee>();
            return resultado;
            
        }

    }
}
