using System.Security.Cryptography.X509Certificates;

namespace API_DB.Models.InputModels
{
    public class MatriculaInputModel
    {
        public int IdCurso { get; set; }
        public int IdAluno { get; set; }
        public DateOnly DataMatricula { get; set; }
    }
}
