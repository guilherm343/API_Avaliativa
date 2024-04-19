using API_DB.Models.Data;
using API_DB.Models.InputModels;
using API_DB.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_DB.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MatriculaController : ControllerBase
    {
        private readonly ILogger<MatriculaController> _logger;
        private readonly MatriculaDB _matriculaDB;

        public MatriculaController(ILogger<MatriculaController> logger, MatriculaDB matriculaDB)
        {
            _logger = logger;
            _matriculaDB = matriculaDB;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatriculaViewModel>>> getMatriculas()
        {
            try
            {
                var dados = await _matriculaDB.listar();
                if (dados != null)
                    return Ok(dados);
                else
                    return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest("Não foi possível obter a lista de matriculas! -> " + e);
            }
        }

        [HttpPost]
        public async Task<ActionResult<MatriculaViewModel>> InsertMatricula(
            [FromBody] MatriculaInputModel matricula
        )
        {
            try
            {
                var dados = await _matriculaDB.insert(matricula);
                return Ok(dados);
            }
            catch (Exception e)
            {
                return UnprocessableEntity("Não foi possível inserir o matricula! ->" + e);
            }
        }



        [HttpDelete("{IdCurso}/{IdAluno}")]
        public async Task<ActionResult<MatriculaViewModel>> Delete([FromRoute] int IdCurso, [FromRoute] int IdAluno)
        {
            try
            {
                var dados = await _matriculaDB.delete(IdCurso, IdAluno);
                return Ok(dados);
            }
            catch (Exception e)
            {
                return UnprocessableEntity("Não foi possível excluir o matricula! ->" + e);
            }
        }

    }
}