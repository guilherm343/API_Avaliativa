using API_DB.Models.InputModels;
using API_DB.Models.ViewModels;
using System.Data.SqlClient;

namespace API_DB.Models.Data
{
    public class MatriculaDB
    {
        private readonly IConexao _conexao;

        public MatriculaDB(IConexao conexao)
        {
            _conexao = conexao;
        }

        /* Implementar métodos para banco de dados */

        public async Task<List<MatriculaViewModel>> listar()
        {
            List<MatriculaViewModel> matriculas = new List<MatriculaViewModel>();

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                /*-----------------------------------------------------------------
                Se o  matricula não criou a view vW Matriculas, utilize esse select

                Select A.*, T.Tipo From tb Matriculas A Inner Join tbTipo Matricula T
                    On T.IdTipo = A.IdTipo Matricula                  
                ------------------------------------------------------------------*/
                string query = "Select * from tbMatricula ";
                SqlCommand command = new SqlCommand(query, conn);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Usando um objeto DataReader para ler os dados do banco
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    // Enquanto conseguir ler algum dado do banco...
                    while (reader.Read())
                    {
                        // Obter os dados e criar um objeto  Matricula
                        MatriculaViewModel matricula = new MatriculaViewModel();
                        matricula.IdCurso = Int32.Parse(reader["IdCurso"].ToString());
                        matricula.IdAluno = Int32.Parse(reader["IdCurso"].ToString());
                        matricula.DataMatricula = DateOnly.Parse(
                                reader["DataMatricula"].ToString().Substring(0, 10)
                            );


                        // Adicionando o novo Matricula na lista
                        matriculas.Add(matricula);
                    }
                }

                // Fechando a conexão com o banco
                conn.Close();
            }

            return matriculas;
        }

        public async Task<MatriculaViewModel> obterPorId(int IdCurso, int IdAluno)
        {
            MatriculaViewModel matriculaAux = null;

            using (SqlConnection conn = _conexao.getConexao())
            {

                string query = "Select Top 1 * from tbMatricula " +
                    " Where IdCurso = @Curso And IdAluno = @Aluno " +
                    "Order By IdAluno Desc";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Curso", IdCurso);
                command.Parameters.AddWithValue("@Aluno", IdAluno);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Usando um objeto DataReader para ler os dados do banco
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    // Enquanto conseguir ler algum dado do banco...
                    while (reader.Read())
                    {
                        matriculaAux = new MatriculaViewModel();

                        // Obter os dados e criar um objeto Matricula
                        matriculaAux.IdCurso = Int32.Parse(reader["IdCurso"].ToString());
                        matriculaAux.IdAluno = Int32.Parse(reader["IdAluno"].ToString());
                        matriculaAux.DataMatricula = DateOnly.Parse(
                                reader["DataMatricula"].ToString().Substring(0, 10)
                            );

                    }
                }

                // Fechando a conexão com o banco
                conn.Close();
            }

            return matriculaAux;
        }

        public async Task<MatriculaViewModel> obterMatricula(MatriculaInputModel matricula)
        {
            MatriculaViewModel matriculaAux = null;

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Select Top 1 * from tbMatricula " +
                               " Where IdCurso = @Curso And IdAluno = @Aluno " +
                               "Order By IdAluno Desc";

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Aluno", matricula.IdAluno);
                command.Parameters.AddWithValue("@Curso", matricula.IdCurso);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Usando um objeto DataReader para ler os dados do banco
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    // Enquanto conseguir ler algum dado do banco...
                    while (reader.Read())
                    {
                        matriculaAux = new MatriculaViewModel();

                        // Obter os dados e criar um objeto Matricula

                        matriculaAux.IdAluno = Int32.Parse(reader["IdAluno"].ToString());
                        matriculaAux.IdCurso = Int32.Parse(reader["IdCurso"].ToString());
                        matriculaAux.DataMatricula = DateOnly.Parse(
                               reader["DataMatricula"].ToString().Substring(0, 10)
                           );

                    }
                }

                // Fechando a conexão com o banco
                conn.Close();
            }

            return matriculaAux;
        }

        public async Task<MatriculaViewModel> insert(MatriculaInputModel matricula)
        {
            MatriculaViewModel matriculaAux = null;

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Insert Into tbMatricula (IdAluno, IdCurso, DataMatricula) " +
                               "Values (" +
                                  "@Aluno, @Curso,@DataM " +
                               ")";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Aluno", matricula.IdAluno);
                command.Parameters.AddWithValue("@Curso", matricula.IdCurso);
                command.Parameters.AddWithValue("@DataM", matricula.DataMatricula.ToString());

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Executando o comando
                await command.ExecuteNonQueryAsync();

                // Fechando a conexão com o banco
                conn.Close();
            }

            matriculaAux = await obterMatricula(matricula);
            return matriculaAux;
        }

        public async Task<MatriculaViewModel> delete(int IdCurso, int IdAluno)
        {

            MatriculaViewModel matriculaAux = await obterPorId(IdCurso, IdAluno);

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Delete From tbMatricula Where IdCurso=@IdCurso and IdAluno=@IdAluno ";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@IdCurso", IdCurso);
                command.Parameters.AddWithValue("@IdAluno", IdAluno);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Executando o comando
                await command.ExecuteNonQueryAsync();

                // Fechando a conexão com o banco
                conn.Close();
            }

            return matriculaAux;
        }

    }
}