using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicadores.Business.Visa.Credito
{
    public class Movimento
    {
        public string Servico { set; get; }
        public string Transacao { set; get; }

        public DateTime ProcessingDate { set; get; }

        public int Quantidade { set; get; }
        public double TransactionAmount { set; get; }
        public string TipoTransactionAmount { set; get; }
        public double InterchangeFee { set; get; }
        public string TipoInterchangeFee { set; get; }
        public double ProcessingCharge { set; get; }
        public double SettlmentAmount { set; get; }
        public string TipoSettlmentAmount { set; get; }

        public string Arquivo { set; get; }

        public DateTime DataArquivo { set; get; }


        public Movimento()
        {

        }

        public Movimento(TipoServico serv, DateTime dt, TipoTransacao trans, int qtd, double vlr, string tpVlr, double vlrInter,
            string tpInter, double vlrProsCharge, double vlrSett, string tpVlrSett, string nmArq, DateTime dtArq)
        {
            this.Servico = serv.Tipo;
            this.ProcessingDate = dt;
            this.Transacao = trans.Tipo;
            this.Quantidade = qtd;
            this.TransactionAmount = vlr;
            this.TipoTransactionAmount = tpVlr;
            this.InterchangeFee = vlrInter;
            this.TipoInterchangeFee = tpInter;
            this.ProcessingCharge = vlrProsCharge;
            this.SettlmentAmount = vlrSett;
            this.TipoSettlmentAmount = tpVlrSett;
            this.Arquivo = nmArq;
            this.DataArquivo = dtArq;
        }

        public Movimento(TipoServico serv, DateTime dt, TipoTransacao trans, int qtd, double vlr, string tpVlr, double vlrInter,
            string tpInter, double vlrProsCharge, double vlrSett, string tpVlrSett, string nmArq)
        {
            this.Servico = serv.Tipo;
            this.ProcessingDate = dt;
            this.Transacao = trans.Tipo;
            this.Quantidade = qtd;
            this.TransactionAmount = vlr;
            this.TipoTransactionAmount = tpVlr;
            this.InterchangeFee = vlrInter;
            this.TipoInterchangeFee = tpInter;
            this.ProcessingCharge = vlrProsCharge;
            this.SettlmentAmount = vlrSett;
            this.TipoSettlmentAmount = tpVlrSett;
            this.Arquivo = nmArq;
            //this.DataArquivo = dtArq;
        }

        public static List<Movimento> getMovimentos(string path)
        {
            List<Movimento> movtos = new List<Movimento>();
            List<Movimento> movimentos = new List<Movimento>();

            movtos.AddRange(getTransactionsD27Nacional(path));
            movtos.AddRange(getTransactionsReversalNacional(path));
            movtos.AddRange(getTransactionsChargeBackNacional(path));
            movtos.AddRange(getTransactionsFeeNacional(path));
            
            foreach(Movimento m in movtos)
            {
                var mov = (from m1 in movimentos
                           where m1.Arquivo == m.Arquivo
                           where m1.ProcessingDate == m.ProcessingDate
                           where m1.InterchangeFee == m.InterchangeFee
                           where m1.ProcessingCharge == m.ProcessingCharge
                           where m1.Quantidade == m.Quantidade
                           where m1.Servico == m.Servico
                           where m1.SettlmentAmount == m.SettlmentAmount
                           where m1.TipoInterchangeFee == m.TipoInterchangeFee
                           where m1.TipoSettlmentAmount == m.TipoSettlmentAmount
                           where m1.TipoTransactionAmount == m.TipoTransactionAmount
                           where m1.Transacao == m.Transacao
                           where m1.TransactionAmount == m.TransactionAmount
                           select m).ToList<Movimento>();

                if (mov.Count == 0)
                    movimentos.Add(m);
                else
                    continue;
                
            }

            movimentos.Where(x => x.TipoTransactionAmount == "CR").ToList<Movimento>().ForEach(s => s.TipoTransactionAmount = "+");
            movimentos.Where(x => x.TipoTransactionAmount == "DB").ToList<Movimento>().ForEach(s => s.TipoTransactionAmount = "-");
            movimentos.Where(x => x.TipoInterchangeFee == "CR").ToList<Movimento>().ForEach(s => s.TipoInterchangeFee = "+");
            movimentos.Where(x => x.TipoInterchangeFee == "DB").ToList<Movimento>().ForEach(s => s.TipoInterchangeFee = "-");
            movimentos.Where(x => x.TipoSettlmentAmount == "CR").ToList<Movimento>().ForEach(s => s.TipoSettlmentAmount = "+");
            movimentos.Where(x => x.TipoSettlmentAmount == "DB").ToList<Movimento>().ForEach(s => s.TipoSettlmentAmount = "-");

            return movimentos.OrderBy(x => x.ProcessingDate).ThenBy(x => x.Transacao).ThenBy(x => x.Quantidade).ToList<Movimento>();
            //return movimentos;
        }

        public static List<Movimento> getTransactionsChargeBackNacional(string path)
        {
            string[] linhas = File.ReadAllLines(path);
            Business.TipoServico serv = null;
            Business.TipoTransacao tt = null;
            bool validacaoReportRendimento = false, validacaoReportID = false, validaDisputFin = false;
            List<Movimento> movtos = new List<Movimento>();
            int contador = 1, contadorLinhas = 1, contadorReportID = 0;
            DateTime data = new DateTime();


            foreach (string linha in linhas)
            {
                contadorLinhas += 1;

                if (linha.Contains("REPORTING FOR:      9000410201 BANCO RENDIMENT"))
                {
                    validacaoReportRendimento = true;
                    data = getData(linha.Substring(123, 10).TrimEnd().TrimStart());
                }
                if (linha.Contains(" REPORTING FOR:      9000410202"))
                {
                    validacaoReportRendimento = false;
                }

                if (linha.Substring(0, 12) == " DISPUTE FIN" && validacaoReportRendimento)
                {
                    contador += 1;
                }

                if (linha.Contains("REPORT ID:  VSS-600"))
                {
                    validacaoReportID = true;
                    contadorReportID += 1;                    
                }                

                if(linha.Substring(0,12) == " DISPUTE FIN")
                {
                    validaDisputFin = true;
                }

                if (linha.Substring(0, 28).ToUpper() == " TOTAL ACQUIRER TRANSACTIONS")
                {
                    contador = 1;
                    contadorReportID = 1;
                    validacaoReportRendimento = false;
                    validacaoReportID = false;
                    validaDisputFin = false;                    
                }

                if (validacaoReportRendimento && contador > 1 && validacaoReportID && validacaoReportID && validaDisputFin &&
                        (linha.Substring(0, 16) == "     DISPUTE FIN")
                    )
                {
                    serv = new TipoServico("VISA CREDITO NACIONAL");
                    tt = new TipoTransacao(String.Concat("CHARGEBACK - ", linha.Substring(0, 18).TrimEnd().TrimStart()));



                    movtos.Add(new Movimento(serv,
                                                getData(linha.Substring(23, 7)),
                                                tt,
                                                Convert.ToInt32(linha.Substring(30, 12).Replace(",", "").Replace(".", "").TrimEnd().TrimStart()),
                                                Math.Round((Convert.ToDouble(linha.Substring(42, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(62, 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(64, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(84, 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(86, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(106, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(128, 2),
                                                path

                                            )
                        );


                    contador += 1;
                }
                
            }

            return movtos.OrderBy(x => x.ProcessingDate).ThenBy(x => x.Transacao).ThenBy(x => x.Quantidade).ToList<Movimento>();
        }

        public static List<Movimento> getTransactionsReversalNacional(string path)
        {
            string[] linhas = File.ReadAllLines(path);
            Business.TipoServico serv = null;
            Business.TipoTransacao tt = null;
            bool validacaoReportRendimento = false, validacaoReportID = false;
            List<Movimento> movtos = new List<Movimento>();
            int contador = 1;
            DateTime data = new DateTime();


            foreach (string linha in linhas)
            {

                if (linha.Contains("REPORTING FOR:      9000410201 BANCO RENDIMENT"))
                {
                    validacaoReportRendimento = true;
                    data = getData(linha.Substring(123, 10).TrimEnd().TrimStart());
                }
                if (linha.Contains(" REPORTING FOR:      9000410202"))
                {
                    validacaoReportRendimento = false;                    
                }

                if (linha.Contains("REPORT ID:  VSS-600"))
                {
                    validacaoReportID = true;
                }

                if (linha.Substring(0, 22) == " ACQUIRER TRANSACTIONS")
                {
                    contador += 1;
                }

                if (linha.Substring(0, 19).ToUpper() == " TOTAL NO DEFERMENT")
                {
                    contador = 1;
                    validacaoReportRendimento = false;
                    validacaoReportID = false;
                }

                if (validacaoReportRendimento && contador > 1 && validacaoReportID &&
                        (linha.Substring(0, 13) == "     REVERSAL")
                    )
                {
                    serv = new TipoServico("VISA CREDITO NACIONAL");
                    tt = new TipoTransacao(linha.Substring(0, 18).TrimEnd().TrimStart());

                    movtos.Add(new Movimento(serv,
                                                getData(linha.Substring(23, 7)),
                                                tt,
                                                Convert.ToInt32(linha.Substring(30, 12).Replace(",", "").Replace(".", "").TrimEnd().TrimStart()),
                                                Math.Round((Convert.ToDouble(linha.Substring(42, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(62, 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(64, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(84, 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(86, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(106, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(128, 2),
                                                path

                                            )
                        );


                    contador += 1;
                }

            }

            return movtos.OrderBy(x => x.ProcessingDate).ThenBy(x => x.Transacao).ThenBy(x => x.Quantidade).ToList<Movimento>();
        }

        public static List<Movimento> getTransactionsD27Nacional(string path)
        {
            string[] linhas = File.ReadAllLines(path);
            string d27 = String.Empty;
            Business.TipoServico serv = null;
            Business.TipoTransacao tt = null;
            bool validacaoReportRendimento = false, validacaoReportID = false;
            List<Movimento> movtos = new List<Movimento>();
            int contador = 1;
            DateTime data = new DateTime();


            foreach (string linha in linhas)
            {

                if (linha.Contains("REPORTING FOR:      9000410201 BANCO RENDIMENT"))
                {
                    validacaoReportRendimento = true;
                    data = getData(linha.Substring(123, 10).TrimEnd().TrimStart());
                }
                if (linha.Contains(" REPORTING FOR:      9000410202"))
                {
                    validacaoReportRendimento = false;                    
                }

                if (linha.Contains("REPORT ID:  VSS-600"))
                {
                    validacaoReportID = true;
                    continue;
                }


                if (linha.Substring(0, 13) == " 27-DAY DEFER")
                {
                    contador += 1;
                }

                if (linha.Substring(0, 19).ToUpper() == " TOTAL 27-DAY DEFER")
                {
                    contador = 1;
                    validacaoReportRendimento = false;
                    validacaoReportID = false;
                    data = new DateTime();
                }

                if (validacaoReportRendimento && contador > 1 && validacaoReportID &&
                        (linha.Substring(0, 18) == "     ORIGINAL SALE" || linha.Substring(0, 18) == "     ORIGINAL     "
                        )
                    )
                {

                    switch(linha.Substring(0, 18))
                    {
                        case "     ORIGINAL SALE":
                            {
                                d27 = "PURCHASE";
                                break;
                            }

                        case "     ORIGINAL     ":
                            {
                                d27 = "MERCHANDISE CREDIT";
                                break;
                            }
                    }

                    serv = new TipoServico("VISA CREDITO NACIONAL");
                    tt = new TipoTransacao( String.Concat("D27 - ", d27));
                                        

                    movtos.Add(new Movimento(serv,
                                                getData(linha.Substring(23, 7))
                                                ,
                                                tt,
                                                Convert.ToInt32(linha.Substring(30, 12).Replace(",", "").Replace(".", "").TrimEnd().TrimStart()),
                                                Math.Round((Convert.ToDouble(linha.Substring(42, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(62, 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(64, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(84, 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(86, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(106, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(128, 2),
                                                path

                                            )
                        );


                    contador += 1;
                }

            }

            return movtos.OrderBy(x => x.ProcessingDate).ThenBy(x => x.Transacao).ThenBy(x => x.Quantidade).ToList<Movimento>();
        }

        

        public static List<Movimento> getTransactionsFeeNacional(string path)
        {
            string[] linhas = File.ReadAllLines(path);
            Business.TipoServico serv = null;
            Business.TipoTransacao tt = null;
            bool validacaoReportRendimento = false, validacaoReportID = false;
            List<Movimento> movtos = new List<Movimento>();
            int contador = 1;
            DateTime data = new DateTime();


            foreach (string linha in linhas)
            {

                if (linha.Contains("REPORTING FOR:      9000410201 BANCO RENDIMENT"))
                {
                    validacaoReportRendimento = true;
                    data = getData(linha.Substring(123, 10).TrimEnd().TrimStart());
                }

                if(linha.Contains("REPORT ID:  VSS-600"))
                {
                    validacaoReportID = true;
                }

                if (linha.Contains(" REPORTING FOR:      9000410202"))
                {
                    validacaoReportRendimento = false;
                }

                if (linha.Substring(0, 13) == " 02-DAY DEFER")
                {
                    contador += 1;
                }

                if (linha.Substring(0, 19).ToUpper() == " TOTAL 02-DAY DEFER")
                {
                    contador = 1;
                    validacaoReportRendimento = false;
                    validacaoReportID = false;
                }

                if (validacaoReportRendimento && contador > 1 && validacaoReportID &&
                        (linha.Substring(0, 21) == "   TOTAL 02-DAY DEFER"
                        )
                    )
                {
                    serv = new TipoServico("VISA FEE COLLECTION");
                    tt = new TipoTransacao(String.Concat("FEE - ", linha.Substring(0, 21).TrimEnd().TrimStart()));

                    movtos.Add(new Movimento(serv,
                                                getData(linha.Substring(23, 7)),
                                                tt,
                                                Convert.ToInt32(linha.Substring(30, 12).Replace(",", "").Replace(".", "").TrimEnd().TrimStart()),
                                                Math.Round((Convert.ToDouble(linha.Substring(42, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(62, 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(64, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(84, 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(86, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(106, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(128, 2),
                                                path

                                            )
                        );


                    contador += 1;
                }

            }

            return movtos.OrderBy(x => x.ProcessingDate).ThenBy(x => x.Transacao).ThenBy(x => x.Quantidade).ToList<Movimento>();
        }


        public static List<Movimento> getTransactionsInternacional(string path)
        {
            string[] linhas = File.ReadAllLines(path);
            Business.TipoServico serv = null;
            Business.TipoTransacao tt = null;
            bool validacaoReportRendimento = false, validacaoReportID = false;
            List<Movimento> movtos = new List<Movimento>();
            List<Movimento> movtosAux = new List<Movimento>();
            int contador = 1;
            DateTime data = new DateTime();

            #region ###### VSS-115 #########

            foreach (string linha in linhas)
            {

                if (linha.Contains("REPORTING FOR:      9000410201 BANCO RENDIMENT"))
                {
                    validacaoReportRendimento = true;
                    data = getData(linha.Substring(123, 10).TrimEnd().TrimStart());
                }
                if (linha.Contains(" REPORTING FOR:      9000410202"))
                {
                    validacaoReportRendimento = false;
                }

                if (linha.Contains("REPORT ID:  VSS-115"))
                {
                    validacaoReportID = true;
                }


                if (linha.Substring(0, 22) == " ACQUIRER TRANSACTIONS")
                {
                    contador += 1;
                }

                if (linha.Substring(0, 28).ToUpper() == " FINAL SETTLEMENT NET AMOUNT")
                {
                    contador = 1;
                    validacaoReportRendimento = false;
                    validacaoReportID = false;
                }

                if (validacaoReportRendimento && contador > 1 && validacaoReportID &&
                        (linha.Substring(0, 18) == " TOTAL INTERCHANGE" || linha.Substring(0, 6) == " TOTAL"
                        )
                    )
                {
                    serv = new TipoServico("VISA CREDITO INTERNACIONAL");
                    tt = new TipoTransacao(String.Concat(linha.Substring(0, 18).TrimEnd().TrimStart()));


                    movtosAux.Add(new Movimento(serv,
                                                getData(linha.Substring(23, 7))
                                                ,
                                                tt,
                                                Convert.ToInt32(linha.Substring(30, 12).Replace(",", "").Replace(".", "").TrimEnd().TrimStart()),
                                                Math.Round((Convert.ToDouble(linha.Substring(42, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(62, 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(64, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(84, 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(86, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(106, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(128, 2),
                                                path

                                            )
                        );


                    contador += 1;
                }

            }
            #endregion

            #region ######### VSS-120 ##########



            #endregion

            return movtos.OrderBy(x => x.ProcessingDate).ThenBy(x => x.Transacao).ThenBy(x => x.Quantidade).ToList<Movimento>();
        }



        public static DateTime getData(string dataStr)
        {
            var meses = (CultureInfo.GetCultureInfo("EN-US").DateTimeFormat.AbbreviatedMonthNames).Select(x => x.ToUpper()).ToArray();            
            int mes = (Array.IndexOf(meses, dataStr.Substring(2, 3))) + 1;           
            int ano = Convert.ToInt32(dataStr.Substring(5,2)) + 2000;
            int dia = Convert.ToInt32(dataStr.Substring(0, 2));

            return new DateTime(ano, mes, dia);
        }
    }
}
