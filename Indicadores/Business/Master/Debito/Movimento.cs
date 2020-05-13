using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicadores.Business.Master.Debito
{
    public class Movimento
    {
       
        public DateTime Data { set; get; }
        public string Transacao { set; get; }
        
        public int QuantidadeAprovada { set; get; }

        public int QuantidadeNegada { set; get; }
        public double ValorTransacao { set; get; }

        public string TipoValorTransacao { set; get; }

        public double ValorInterchange { set; get; }  

        public string Arquivo { set; get; }




        public Movimento()
        {

        }

        public Movimento(DateTime dt, TipoTransacao tpTrans, int qtdAprovada, int qtdNegada,
                            double vlrTrans, double vlrInter, string nmArq, string tpValTrans
                        )
        {
            
            this.Data = dt;
            this.Transacao = tpTrans.Tipo;            
            this.QuantidadeAprovada = qtdAprovada;
            this.QuantidadeNegada = qtdNegada;
            this.ValorTransacao = vlrTrans;
            this.ValorInterchange = vlrInter;            
            this.Arquivo = nmArq;
            this.TipoValorTransacao = tpValTrans;
            
        }


        public static List<Movimento> getMovimentos(string path)
        {
            string[] linhas = File.ReadAllLines(path);                        
            DateTime data = new DateTime();
            string serviceID = String.Empty;
            List<Business.Master.Debito.Movimento> movtos = new List<Business.Master.Debito.Movimento>();
            int contador = 1;

            foreach (string linha in linhas)
            {

                if (linha.ToUpper().Contains("DAILY CONTROL REPORT"))
                {                    
                    data = new DateTime((Convert.ToInt32(linha.Substring(130, 2)) + 2000), Convert.ToInt32(linha.Substring(124, 2)), Convert.ToInt32(linha.Substring(127, 2)));
                }               
                if (linha.Contains("GRAND TOTALS:"))
                {
                    contador += 1;
                    continue;
                }
                if (linha.Substring(4, 18).TrimEnd().TrimStart() == "PURCHASE" || linha.Substring(4, 18).TrimEnd().TrimStart() == "ECOM PURCHASE")
                {
                    if (contador > 1)
                    {
                        movtos.Add(new Business.Master.Debito.Movimento
                            (
                                data,
                                new TipoTransacao(linha.Substring(4, 18).TrimEnd().TrimStart()),
                                Convert.ToInt32(linha.Substring(23, 8)),
                                Convert.ToInt32(linha.Substring(33, 8)),
                                Math.Round((Convert.ToDouble(linha.Substring(42, 18).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                Math.Round((Convert.ToDouble(linha.Substring(104, 18).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                path,
                                linha.Substring(60, 2)
                            )
                        );

                        contador += 1;
                    }

                    
                }

                if (linha.Contains("--------------------------------------------------------------------------------------------------------------------------------"))
                {
                    contador = 1;                                       
                }

            }

            movtos.Where(x => x.TipoValorTransacao == "CR").ToList<Movimento>().ForEach(s => s.TipoValorTransacao = "+");
            movtos.Where(x => x.TipoValorTransacao == "DR").ToList<Movimento>().ForEach(s => s.TipoValorTransacao = "-");
            

            return movtos.OrderBy(x => x.Arquivo).ThenBy(x => x.Transacao).ToList<Movimento>();
        }

    }

}
