using API_DB.Models.InputModels;
using API_DB.Models.ViewModels;
using System.Data.SqlClient;

namespace API_DB.Models.Data
{
    public class TipoAlunoDB
    {
        private readonly IConexao _conexao;

        public TipoAlunoDB(IConexao conexao)
        {
            _conexao = conexao;
        }

        /* Implementar métodos para banco de dados */

        public async Task<List<TipoAlunoViewModel>> listar()
        {
            List<TipoAlunoViewModel> tipoalunos = new List<TipoAlunoViewModel>();

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                /*-----------------------------------------------------------------*/
                string query = "Select * from tbTipoAluno ";
                SqlCommand command = new SqlCommand(query, conn);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Usando um objeto DataReader para ler os dados do banco
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    // Enquanto conseguir ler algum dado do banco...
                    while (reader.Read())
                    {
                        // Obter os dados e criar um objeto  TipoAluno
                        TipoAlunoViewModel tipoaluno = new TipoAlunoViewModel();
                        tipoaluno.IdTipoAluno = Int32.Parse(reader["IdTipo"].ToString());
                        tipoaluno.TipoAluno = reader["Tipo"].ToString();


                        // Adicionando o novo TipoAluno na lista
                        tipoalunos.Add(tipoaluno);
                    }
                }

                // Fechando a conexão com o banco
                conn.Close();
            }

            return tipoalunos;
        }

        public async Task<TipoAlunoViewModel> obterPorId(int Id)
        {
            TipoAlunoViewModel tipoalunos = null;

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                /*-----------------------------------------------------------------
                Se o tipoalunos não criou a view vWTipoAlunos, utilize esse select

                Select A.*, T.Tipo From tbTipoAlunos A Inner Join tbTipoTipoAluno T
                    On T.IdTipo = A.IdTipoTipoAluno Where IdTipoAluno = @Id              
                ------------------------------------------------------------------*/
                string query = "Select * from tbTipoAluno Where IdTipo = @Id ";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id", Id);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Usando um objeto DataReader para ler os dados do banco
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    // Enquanto conseguir ler algum dado do banco...
                    while (reader.Read())
                    {
                        tipoalunos = new TipoAlunoViewModel();

                        // Obter os dados e criar um objeto TipoAluno
                        tipoalunos.IdTipoAluno = Int32.Parse(reader["IdTipo"].ToString());
                        tipoalunos.TipoAluno = reader["Tipo"].ToString();

                    }
                }

                // Fechando a conexão com o banco
                conn.Close();
            }

            return tipoalunos;
        }

        public async Task<TipoAlunoViewModel> obterTipoAluno(TipoAlunoInputModel tipoalunos)
        {
            TipoAlunoViewModel tipoalunosAux = null;

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Select Top 1 * from tbTipoAluno " +
                               " Where Tipo = @Tipo " +
                               "Order By IdTipo Desc ";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Tipo", tipoalunos.TipoAluno);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Usando um objeto DataReader para ler os dados do banco
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    // Enquanto conseguir ler algum dado do banco...
                    while (reader.Read())
                    {
                        tipoalunosAux = new TipoAlunoViewModel();

                        // Obter os dados e criar um objeto TipoAluno
                        tipoalunosAux.IdTipoAluno = Int32.Parse(reader["IdTipo"].ToString());
                        tipoalunosAux.TipoAluno = reader["Tipo"].ToString();


                    }
                }

                // Fechando a conexão com o banco
                conn.Close();
            }

            return tipoalunosAux;
        }

        public async Task<TipoAlunoViewModel> insert(TipoAlunoInputModel tipoaluno)
        {
            TipoAlunoViewModel tipoalunosAux = null;

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Insert Into tbTipoAluno (IdTipo, Tipo) " +
                               "Values (" +
                                  "(Select Max(IdTipo)+1 From tbTipoAluno), @Tipo " +
                               ")";
                SqlCommand command = new SqlCommand(query, conn);

                command.Parameters.AddWithValue("@Tipo", tipoaluno.TipoAluno);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Executando o comando
                await command.ExecuteNonQueryAsync();

                // Fechando a conexão com o banco
                conn.Close();
            }

            tipoalunosAux = await obterTipoAluno(tipoaluno);
            return tipoalunosAux;
        }

        public async Task<TipoAlunoViewModel> update(int Id, TipoAlunoInputModel tipoaluno)
        {
            TipoAlunoViewModel tipoalunoAux = null;

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Update tbTipoAluno Set Tipo=@Tipo " +
                                  "Where IdTipo=@Id ";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Tipo", tipoaluno.TipoAluno);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Executando o comando
                await command.ExecuteNonQueryAsync();

                // Fechando a conexão com o banco
                conn.Close();
            }

            tipoalunoAux = await obterTipoAluno(tipoaluno);
            return tipoalunoAux;
        }

        public async Task<TipoAlunoViewModel> delete(int Id)
        {

            TipoAlunoViewModel tipoalunoAux = await obterPorId(Id);

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Delete From tbTipoAluno Where IdTipo=@Id ";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id", Id);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Executando o comando
                await command.ExecuteNonQueryAsync();

                // Fechando a conexão com o banco
                conn.Close();
            }

            return tipoalunoAux;
        }

    }
}