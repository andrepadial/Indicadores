using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicadores.DAO.Master
{
    public class DAOMovimento
    {


        public DAOMovimento()
        {

        }

        public static SqlConnection retornaConexao()
        {
            return new SqlConnection(CriptorEngine.CriptorEngine.Decrypt(ConfigurationManager.AppSettings["conexaoBD"], true));
        }

        public int Insert(string pathFile)
        {

            List<Business.Master.Debito.Movimento> movimentos = new List<Business.Master.Debito.Movimento>();

            int retorno = 0;

            return retorno;
        }



    }
}
