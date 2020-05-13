using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicadores.Business
{
    public class ChargeBack : Movimento
    {

        public double ValorAmountGR { set; get; }

        public double ValorAmountINW { set; get; }


        public ChargeBack()
        {

        }


        public static List<ChargeBack> getTransactionsChargeBack(DataTable dt)
        {
            List<Business.ChargeBack> cf = new List<Business.ChargeBack>();
                        

            foreach (DataRow dr in dt.Rows)
            {
                double valor = Convert.ToDouble(dr["local amount gr"].ToString().Replace(".", "").Replace(",", "")) / 100;
                double valorINW = Convert.ToDouble(dr["local amount inw chg"].ToString().Replace(".", "").Replace(",", "")) / 100;
                

                if (dr["Type"].ToString() == "Purchase" && dr["Class"].ToString() == "Clearing transactions"
                    && dr["Category"].ToString() == "Chargebacks")
                {
                    if(dr["Status"].ToString() == "Posted" || dr["Status"].ToString() == "Reprocessed")
                    {
                        ChargeBack c = new ChargeBack(); 
                        c.Tipo = dr["Type"].ToString();
                        c.Classe = dr["Class"].ToString();
                        c.Status = dr["Status"].ToString();
                        c.Categoria = dr["Category"].ToString();
                        c.ValorAmountGR = Math.Round(valor, 2);
                        c.ValorAmountINW = Math.Round(valorINW);
                        cf.Add(c);
                    }
                }               
                
            }                               
            

            return cf.OrderBy(x => x.Status).ToList<Business.ChargeBack>();

        }

    }
}
