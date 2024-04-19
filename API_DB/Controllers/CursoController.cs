using API_DB.Models.Data;
using API_DB.Models.InputModels;
using API_DB.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_DB.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly ILogger<CursoController> _logger;
        private readonly CursoDB _cursoDB;

        public CursoController(ILogger<CursoController> logger, CursoDB cursoDB)
        {
            _logger = logger;
            _cursoDB = cursoDB;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursoViewModel>>> getAlunos()
        {
            try
            {
                var dados = await _cursoDB.listar();
                if (dados != null)
                    return Ok(dados);
                else
                    return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("Não foi possível obter a lista de alunos!");
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<CursoViewModel>> getAlunoById([FromRoute] int Id)
        {
            try
            {
                var dados = await _cursoDB.obterPorId(Id);
                if (dados != null)
                    return Ok(dados);
                else
                    return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("Não foi possível encontrar o curso!");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CursoViewModel>> InsertAluno(
            [FromBody] CursoInputModel curso
        )
        {
            try
            {
                var dados = await _cursoDB.insert(curso);
                return Ok(dados);
            }
            catch (Exception)
            {
                return UnprocessableEntity("Não foi possível inserir o curso!");
            }
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<CursoViewModel>> Update(
            [FromRoute] int Id, [FromBody] CursoInputModel curso
        )
        {
            try
            {
                var dados = await _cursoDB.update(Id, curso);
                return Ok(dados);
            }
            catch (Exception e)
            {
                return UnprocessableEntity("Não foi possível atualizar o curso! -> " + e);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<CursoViewModel>> Delete([FromRoute] int Id)
        {
            try
            {
                var dados = await _cursoDB.delete(Id);
                return Ok(dados);
            }
            catch (Exception e)
            {
                return UnprocessableEntity("Não foi possível excluir o curso pois há matricula! ");
            }
        }

    }
}