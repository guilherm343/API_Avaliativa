namespace API_DB.Models.InputModels
{
    public class AlunoInputModel
    {
        public string Aluno { get; set; }
        public string RM { get; set; }
        public DateOnly DataNascimento { get; set; }
        public int IdTipoAluno { get; set; }
    }
}
