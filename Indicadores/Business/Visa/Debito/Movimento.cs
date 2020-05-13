using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicadores.Business.Visa.Debito
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
        public double PaymentAmount { set; get; }
        public string TipoPaymentAmount { set; get; }

        public string Arquivo { set; get; }

        public DateTime DataArquivo { set; get; }


        public Movimento()
        {

        }

        public Movimento(TipoServico serv, DateTime dt, TipoTransacao trans, int qtd, double vlr, string tpVlr, double vlrInter,
            string tpInter, double vlrProsCharge, double vlrSett, string tpVlrSett, string nmArq, DateTime dtArquivo)
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
            this.PaymentAmount = vlrSett;
            this.TipoPaymentAmount = tpVlrSett;
            this.Arquivo = nmArq;
            this.DataArquivo = dtArquivo;
        }


        public static List<Movimento> getTransactions(string path)
        {
            string[] linhas = File.ReadAllLines(path);
            Business.TipoServico serv = null;
            Business.TipoTransacao tt = null;
            DateTime data = new DateTime(1900, 1, 1);
            bool validacaoReportRendimento = false, validacaoReportID = false;
            List<Movimento> movtos = new List<Movimento>();
            List<Movimento> movimentos = new List<Movimento>();
            int contador = 1, contadorLinhas = 1;


            foreach (string linha in linhas)
            {
                contadorLinhas += 1;

                if (linha.Contains("REPORTING FOR:      9000410201 BANCO RENDIMENT"))
                {
                    validacaoReportRendimento = true;

                    if (data == new DateTime(1900, 1, 1))
                        data = getData(linha.Substring(123, 10).TrimEnd().TrimStart());
                    else
                        continue;
                }
                if (linha.Contains(" REPORTING FOR:      9000410202"))
                {
                    validacaoReportRendimento = false;
                }

                if (linha.Contains("REPORT ID:  VSS-910"))
                {
                    validacaoReportID = true;
                }


                if (linha.Substring(0, 15) == "   NO DEFERMENT")
                {
                    contador += 1;
                }

                if (linha.Substring(0, 21).ToUpper() == "   TOTAL NO DEFERMENT")
                {
                    contador = 1;
                    validacaoReportRendimento = false;
                    validacaoReportID = false;
                }

                if (validacaoReportRendimento && contador > 1 && validacaoReportID &&
                        (linha.Substring(0, 20) == "       ORIGINAL SALE" || linha.Substring(0, 15) == "       ORIGINAL")
                    )
                {
                    serv = new TipoServico("VISA DEBITO NACIONAL");
                    tt = new TipoTransacao(String.Concat(linha.Substring(0, 20).TrimEnd().TrimStart()));


                    movtos.Add(new Movimento(serv,
                                                data
                                                ,
                                                tt,
                                                Convert.ToInt32(linha.Substring(58, 6).Replace(",", "").Replace(".", "").TrimEnd().TrimStart()),
                                                Math.Round((Convert.ToDouble(linha.Substring(64, 19).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(83, 2),
                                                Math.Round((Convert.ToDouble(linha.Substring(85, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(105, 2),
                                                0,
                                                Math.Round((Convert.ToDouble(linha.Substring(107, 20).TrimEnd().TrimStart().Replace(",", "").Replace(".", "")) / 100), 2),
                                                linha.Substring(127, 2),
                                                path,
                                                data

                                            )
                        );


                    contador += 1;
                }

            }

            foreach (Movimento m in movtos)
            {
                var mov = (from m1 in movimentos
                           where m1.Arquivo == m.Arquivo
                           where m1.ProcessingDate == m.ProcessingDate
                           where m1.InterchangeFee == m.InterchangeFee
                           where m1.ProcessingCharge == m.ProcessingCharge
                           where m1.Quantidade == m.Quantidade
                           where m1.Servico == m.Servico
                           where m1.PaymentAmount == m.PaymentAmount
                           where m1.TipoInterchangeFee == m.TipoInterchangeFee
                           where m1.TipoPaymentAmount == m.TipoPaymentAmount
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
            movimentos.Where(x => x.TipoPaymentAmount == "CR").ToList<Movimento>().ForEach(s => s.TipoPaymentAmount = "+");
            movimentos.Where(x => x.TipoPaymentAmount == "DB").ToList<Movimento>().ForEach(s => s.TipoPaymentAmount = "-");


            return movimentos.OrderBy(x => x.ProcessingDate).ThenBy(x => x.Transacao).ThenBy(x => x.Quantidade).ToList<Movimento>();
        }

        public static DateTime getData(string dataStr)
        {
            var meses = (CultureInfo.GetCultureInfo("EN-US").DateTimeFormat.AbbreviatedMonthNames).Select(x => x.ToUpper()).ToArray();
            int mes = (Array.IndexOf(meses, dataStr.Substring(2, 3))) + 1;
            int ano = Convert.ToInt32(dataStr.Substring(5, 2)) + 2000;
            int dia = Convert.ToInt32(dataStr.Substring(0, 2));

            return new DateTime(ano, mes, dia);
        }

    }
}
