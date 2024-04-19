using System.Data.SqlClient;
namespace API_DB.Models.Data
{
    public interface IConexao
    {
        SqlConnection getConexao();
    }
}
