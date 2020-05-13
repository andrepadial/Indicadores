using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicadores.Business.Master.Credito
{
    public class Movimento
    {

        public string Servico { set; get; }
        public DateTime Data { set; get; }
        public string Transacao { set; get; }
        public string ProcCode { set; get; }
        public int Quantidade { set; get; }
        public double ReconAmount { set; get; }
        public string TipoReconAmount { set; get; }
        public string ReconCurrCode { set; get; }
        public double Fee { set; get; }
        public string TipoFee { set; get; }
        public string FeeCurrCode { set; get; }

        public string ServiceID { set; get; }

        public string Arquivo { set; get; }
        

        public Movimento()
        {

        }

        public Movimento(   TipoServico tpServ, DateTime dt, TipoTransacao tpTrans, string prCode, int qtd, double rcAmt,
                            string rcTpAmt, string rcCurr, double vlrFee, string tpFee, string feeCur, string svcID,
                            string nmArq
                        )
        {
            this.Servico = tpServ.Tipo;
            this.Data = dt;
            this.Transacao = tpTrans.Tipo;
            this.ProcCode = prCode;
            this.Quantidade = qtd;
            this.ReconAmount = rcAmt;
            this.TipoReconAmount = rcTpAmt;
            this.ReconCurrCode = rcCurr;
            this.Fee = vlrFee;
            this.TipoFee = tpFee;
            this.FeeCurrCode = feeCur;
            this.ServiceID = svcID;
            this.Arquivo = nmArq;
        }


        public static List<Movimento> getMovimentos(string path)
        {
            string[] linhas = File.ReadAllLines(path);
            Business.TipoServico serv = null;
            Business.TipoTransacao tt = null;
            DateTime data = new DateTime();
            string serviceID = String.Empty;
            List<Business.Master.Credito.Movimento> movtos = new List<Business.Master.Credito.Movimento>();
            int contador = 1;

            foreach (string linha in linhas)
            {                         

                if (linha.ToUpper().Contains("BUSINESS SERVICE LEVEL:"))
                {
                    serv = new Business.TipoServico(linha.Substring(25, 20).TrimEnd().TrimStart());
                    data = new DateTime(Convert.ToInt32(linha.Substring(62, 4)), Convert.ToInt32(linha.Substring(67, 2)), Convert.ToInt32(linha.Substring(70, 2)));
                }
                if (linha.ToUpper().Contains("BUSINESS SERVICE ID:"))
                {
                    serviceID = linha.Substring(22, 20).TrimEnd().TrimStart();
                }
                if (linha.Contains("------------ --------------- --- -------- -------------------------- ------- ----------------------- -------"))
                {
                    continue;
                }
                if (linha.Substring(14, 11).TrimEnd().TrimStart() == "PURCHASE" || linha.Substring(14, 11).TrimEnd().TrimStart() == "CREDIT")
                {
                    if (contador == 1)
                    {
                        switch (linha.Substring(1, 12).TrimEnd().TrimStart())
                        {
                            case "FIRST PRES.":
                                {
                                    tt = new Business.TipoTransacao("FIRST PRES.");
                                    break;
                                }

                            case "FIRST C/B -F":
                                {
                                    tt = new Business.TipoTransacao("FIRST C/B –F");
                                    break;
                                }
                            case "FEE COLL –MBG":
                                {
                                    tt = new Business.TipoTransacao("FEE COLL –MBG");
                                    break;
                                }

                            case "SEC. PRES.-F":
                                {
                                    tt = new Business.TipoTransacao("SEC. PRES.-F");
                                    break;
                                }
                        }
                    }


                    movtos.Add(new Business.Master.Credito.Movimento
                            (
                                serv,
                                data,
                                tt,
                                linha.Substring(14, 15).TrimEnd().TrimStart(),
                                Convert.ToInt32(linha.Substring(36, 6).TrimEnd().TrimStart()),
                                Math.Round((Convert.ToDouble(linha.Substring(43, 23).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                linha.Substring(67, 2),
                                linha.Substring(70, 7),
                                Math.Round((Convert.ToDouble(linha.Substring(78, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                linha.Substring(99, 2),
                                linha.Substring(102, 7),
                                serviceID,
                                path
                            )
                        );

                    contador += 1;                   
                }

                if (linha.Contains("FIRST PRES.  TOTAL") || linha.Contains("FEE COLL –MBG  TOTAL") ||
                       linha.Contains("SEC. PRES.-F TOTAL") || linha.Contains("FIRST C/B –F TOTAL")
                       )
                {
                    contador = 1;
                    //serv = new TipoServico();
                    //tt = new TipoTransacao();                    
                }

            }

            movtos.Where(x => x.TipoReconAmount == "CR").ToList<Movimento>().ForEach(s => s.TipoReconAmount = "+");
            movtos.Where(x => x.TipoReconAmount == "DR").ToList<Movimento>().ForEach(s => s.TipoReconAmount = "-");
            movtos.Where(x => x.TipoFee == "CR").ToList<Movimento>().ForEach(s => s.TipoFee = "+");
            movtos.Where(x => x.TipoFee == "DR").ToList<Movimento>().ForEach(s => s.TipoFee = "-");
           

            return movtos.OrderBy(x => x.Arquivo).ThenBy(x => x.Servico).ThenBy(x => x.Transacao).ThenBy(x => x.ServiceID).ToList<Movimento>();
        }

    }
}
