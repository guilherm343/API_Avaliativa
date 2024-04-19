

using API_DB.Models.InputModels;
using API_DB.Models.ViewModels;
using System.Data.SqlClient;

namespace API_DB.Models.Data
{
    public class CursoDB
    {
        private readonly IConexao _conexao;

        public CursoDB(IConexao conexao)
        {
            _conexao = conexao;
        }

        /* Implementar métodos para banco de dados */

        public async Task<List<CursoViewModel>> listar()
        {
            List<CursoViewModel> cursos = new List<CursoViewModel>();

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                /*-----------------------------------------------------------------
                Se o curso não criou a view vWCursos, utilize esse select

                Select A.*, T.Tipo From tbcursos A Inner Join tbTipoCurso T
                    On T.IdTipo = A.IdTipoCurso                  
                ------------------------------------------------------------------*/
                string query = "Select * from tbCursos ";
                SqlCommand command = new SqlCommand(query, conn);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Usando um objeto DataReader para ler os dados do banco
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    // Enquanto conseguir ler algum dado do banco...
                    while (reader.Read())
                    {
                        // Obter os dados e criar um objeto Curso
                        CursoViewModel curso = new CursoViewModel();
                        curso.IdCurso = Int32.Parse(reader["IdCurso"].ToString());
                        curso.Curso = reader["Curso"].ToString();
                        curso.CargaHoraria = Int32.Parse(reader["CargaHoraria"].ToString()); ;
                        curso.Sigla = reader["Sigla"].ToString();

                        // Adicionando o novo Curso na lista
                        cursos.Add(curso);
                    }
                }

                // Fechando a conexão com o banco
                conn.Close();
            }

            return cursos;
        }

        public async Task<CursoViewModel> obterPorId(int Id)
        {
            CursoViewModel curso = null;

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                /*-----------------------------------------------------------------
                Se o curso não criou a view vWCursos, utilize esse select

                Select A.*, T.Tipo From tbCursos A Inner Join tbTipoCurso T
                    On T.IdTipo = A.IdTipoCurso Where IdCurso = @Id              
                ------------------------------------------------------------------*/
                string query = "Select * from tbCursos Where IdCurso = @Id";
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
                        curso = new CursoViewModel();

                        // Obter os dados e criar um objeto Curso
                        curso.IdCurso = Int32.Parse(reader["IdCurso"].ToString());
                        curso.Curso = reader["Curso"].ToString();
                        curso.Sigla = reader["Sigla"].ToString();
                        curso.CargaHoraria = Int32.Parse(reader["CargaHoraria"].ToString());
                    }
                }

                // Fechando a conexão com o banco
                conn.Close();
            }

            return curso;
        }

        public async Task<CursoViewModel> obterCurso(CursoInputModel curso)
        {
            CursoViewModel cursoAux = null;

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Select Top 1 * from tbCursos " +
                               "Where Curso = @Curso And Sigla = @Sigla " +
                               "Order By IdCurso Desc";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Curso", curso.Curso);
                command.Parameters.AddWithValue("@Sigla", curso.Sigla);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Usando um objeto DataReader para ler os dados do banco
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    // Enquanto conseguir ler algum dado do banco...
                    while (reader.Read())
                    {
                        cursoAux = new CursoViewModel();

                        // Obter os dados e criar um objeto Curso
                        cursoAux.IdCurso = Int32.Parse(reader["IdCurso"].ToString());
                        cursoAux.Curso = reader["Curso"].ToString();
                        cursoAux.Sigla = reader["Sigla"].ToString();
                        cursoAux.CargaHoraria = Int32.Parse(reader["CargaHoraria"].ToString());
                    }
                }

                // Fechando a conexão com o banco
                conn.Close();
            }

            return cursoAux;
        }

        public async Task<CursoViewModel> insert(CursoInputModel curso)
        {
            CursoViewModel cursoAux = null;

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Insert Into tbCursos (IdCurso, Curso, Sigla, CargaHoraria) " +
                               "Values (" +
                                  "(Select Max(IdCurso)+1 From tbCursos), @Curso, @Sigla, @CargaHoraria" +
                               ")";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Curso", curso.Curso);
                command.Parameters.AddWithValue("@Sigla", curso.Sigla);
                command.Parameters.AddWithValue("@CargaHoraria", curso.CargaHoraria);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Executando o comando
                await command.ExecuteNonQueryAsync();

                // Fechando a conexão com o banco
                conn.Close();
            }

            cursoAux = await obterCurso(curso);
            return cursoAux;
        }

        public async Task<CursoViewModel> update(int Id, CursoInputModel curso)
        {
            CursoViewModel cursoAux = null;

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Update tbCursos Set Curso=@Curso, Sigla=@Sigla, " +
                                "CargaHoraria=@CargaHoraria " +
                                  "Where IdCurso = @Id";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Curso", curso.Curso);
                command.Parameters.AddWithValue("@Sigla", curso.Sigla);
                command.Parameters.AddWithValue("@CargaHoraria", curso.CargaHoraria);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Executando o comando
                await command.ExecuteNonQueryAsync();

                // Fechando a conexão com o banco
                conn.Close();
            }

            cursoAux = await obterCurso(curso);
            return cursoAux;
        }

        public async Task<CursoViewModel> delete(int Id)
        {

            CursoViewModel cursoAux = await obterPorId(Id);

            using (SqlConnection conn = _conexao.getConexao())
            {
                // Criando a instrução SQL e o objeto command para executá-la
                string query = "Delete From tbCursos Where IdCurso=@Id ";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id", Id);

                // Abrindo a conexão com o banco de dados
                conn.Open();

                // Executando o comando
                await command.ExecuteNonQueryAsync();

                // Fechando a conexão com o banco
                conn.Close();
            }

            return cursoAux;

        }

    }
}