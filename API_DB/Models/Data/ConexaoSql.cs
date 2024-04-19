using System.Data.SqlClient;
namespace API_DB.Models.Data
{
    public class ConexaoSql : IConexao
    {
        private readonly string _stringDeConexao;

        public ConexaoSql(string stringDeConexao) 
        { 
            _stringDeConexao = stringDeConexao;
        }

        public SqlConnection getConexao() 
        {
            return new SqlConnection(_stringDeConexao);
        }
    }
}
