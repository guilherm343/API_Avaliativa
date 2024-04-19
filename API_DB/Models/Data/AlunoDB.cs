using API_DB.Models.InputModels;
using API_DB.Models.ViewModels;
using System.Data.SqlClient;

namespace API_DB.Models.Data
{
    public class AlunoDB
    {
        private readonly IConexao _conexao;

        public AlunoDB(IConexao conexao)
        {
            _conexao = conexao;
        }

        /* Implementar métodos para banco de dados */

        public async Task<List<AlunoViewModel>> listar()
        {
            List<AlunoViewModel> alunos = new List<AlunoViewModel>();

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                /*-----------------------------------------------------------------
                Se o aluno não criou a view vWAlunos, utilize esse select

                Select A.*, T.Tipo From tbAlunos A Inner Join tbTipoAluno T
                        On T.IdTipo = A.IdTipoAluno                  
                ------------------------------------------------------------------*/
                string query = "Select * from tbAlunos";
                SqlCommand command = new SqlCommand(query, conn);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Usando um objeto DataReader para ler os dados do banco
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    // Enquanto conseguir ler algum dado do banco...
                    while (reader.Read())
                    {
                        // Obter os dados e criar um objeto Aluno
                        AlunoViewModel aluno = new AlunoViewModel();
                        aluno.IdAluno = Int32.Parse(reader["IdAluno"].ToString());
                        aluno.Aluno = reader["Aluno"].ToString();
                        aluno.RM = reader["RM"].ToString();
                        aluno.DataNascimento = DateOnly.Parse(
                            reader["DataNascimento"].ToString().Substring(0, 10)
                        );
                        aluno.IdTipoAluno = Int32.Parse(reader["IdTipoAluno"].ToString()); ;


                        // Adicionando o novo Aluno na lista
                        alunos.Add(aluno);
                    }
                }

                // Fechando a conexão com o banco
                conn.Close();
            }

            return alunos;
        }

        public async Task<AlunoViewModel> obterPorId(int Id)
        {
            AlunoViewModel aluno = null;

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                /*-----------------------------------------------------------------
                Se o aluno não criou a view vWAlunos, utilize esse select

                Select A.*, T.Tipo From tbAlunos A Inner Join tbTipoAluno T
                        On T.IdTipo = A.IdTipoAluno Where IdAluno = @Id              
                ------------------------------------------------------------------*/
                string query = "Select * from tbAlunos Where IdAluno = @Id";
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
                        aluno = new AlunoViewModel();

                        // Obter os dados e criar um objeto Aluno
                        aluno.IdAluno = Int32.Parse(reader["IdAluno"].ToString());
                        aluno.Aluno = reader["Aluno"].ToString();
                        aluno.RM = reader["RM"].ToString();
                        aluno.DataNascimento = DateOnly.Parse(
                            reader["DataNascimento"].ToString().Substring(0, 10)
                        );
                        aluno.IdTipoAluno = Int32.Parse(reader["IdTipoAluno"].ToString());

                    }
                }

                // Fechando a conexão com o banco
                conn.Close();
            }

            return aluno;
        }

        public async Task<AlunoViewModel> obterAluno(AlunoInputModel aluno)
        {
            AlunoViewModel alunoAux = null;

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Select Top 1 * from tbAlunos " +
                               "Where Aluno = @Aluno And Rm = @Rm " +
                               "Order By IdAluno Desc";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Aluno", aluno.Aluno);
                command.Parameters.AddWithValue("@Rm", aluno.RM);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Usando um objeto DataReader para ler os dados do banco
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    // Enquanto conseguir ler algum dado do banco...
                    while (reader.Read())
                    {
                        alunoAux = new AlunoViewModel();

                        // Obter os dados e criar um objeto Aluno
                        alunoAux.IdAluno = Int32.Parse(reader["IdAluno"].ToString());
                        alunoAux.Aluno = reader["Aluno"].ToString();
                        alunoAux.RM = reader["RM"].ToString();
                        alunoAux.DataNascimento = DateOnly.Parse(
                            reader["DataNascimento"].ToString().Substring(0, 10)
                        );
                        alunoAux.IdTipoAluno = Int32.Parse(reader["IdTipoAluno"].ToString()); ;

                    }
                }

                // Fechando a conexão com o banco
                conn.Close();
            }

            return alunoAux;
        }

        public async Task<AlunoViewModel> insert(AlunoInputModel aluno)
        {
            AlunoViewModel alunoAux = null;

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Insert Into tbAlunos (IdAluno, Aluno, RM, DataNascimento, IdTipoAluno) " +
                               "Values (" +
                                  "(Select Max(IdAluno)+1 From tbAlunos), @Aluno, @Rm, @DataNasc, @IdTipo" +
                               ")";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Aluno", aluno.Aluno);
                command.Parameters.AddWithValue("@Rm", aluno.RM);
                command.Parameters.AddWithValue("@DataNasc", aluno.DataNascimento.ToString());
                command.Parameters.AddWithValue("@IdTipo", aluno.IdTipoAluno);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Executando o comando
                await command.ExecuteNonQueryAsync();

                // Fechando a conexão com o banco
                conn.Close();
            }

            alunoAux = await obterAluno(aluno);
            return alunoAux;
        }

        public async Task<AlunoViewModel> update(int Id, AlunoInputModel aluno)
        {
            AlunoViewModel alunoAux = null;

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Update tbAlunos Set Aluno= @Aluno, Rm= @Rm, " +
                                  "DataNascimento=@DataNasc, IdTipoAluno= @IdTipo " +
                                  "Where IdAluno=@Id ";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Aluno", aluno.Aluno);
                command.Parameters.AddWithValue("@Rm", aluno.RM);
                command.Parameters.AddWithValue("@DataNasc", aluno.DataNascimento.ToString());
                command.Parameters.AddWithValue("@IdTipo", aluno.IdTipoAluno);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Executando o comando
                await command.ExecuteNonQueryAsync();

                // Fechando a conexão com o banco
                conn.Close();
            }

            alunoAux = await obterAluno(aluno);
            return alunoAux;
        }

        public async Task<AlunoViewModel> delete(int Id)
        {

            AlunoViewModel alunoAux = await obterPorId(Id);

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Delete From tbAlunos Where IdAluno= @Id ";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id", Id);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Executando o comando
                await command.ExecuteNonQueryAsync();

                // Fechando a conexão com o banco
                conn.Close();
            }

            return alunoAux;
        }

    }
}