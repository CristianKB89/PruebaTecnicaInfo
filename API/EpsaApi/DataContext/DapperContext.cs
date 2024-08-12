using System.Data;
using System.Data.SqlClient;

namespace EpsaApi.DataContext
{
    public class DapperContext
    {
        public IDbConnection CreateConnection()
        {
            return new SqlConnection("Data Source=localhost;Initial Catalog=epsa;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
