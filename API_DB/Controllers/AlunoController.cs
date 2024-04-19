using API_DB.Models.Data;
using API_DB.Models.InputModels;
using API_DB.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_DB.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        private readonly ILogger<AlunoController> _logger;
        private readonly AlunoDB _alunoDB;

        public AlunoController(ILogger<AlunoController> logger, AlunoDB alunoDB)
        {
            _logger = logger;
            _alunoDB = alunoDB;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlunoViewModel>>> getAlunos()
        {
            try
            {
                var dados = await _alunoDB.listar();
                if (dados != null)
                    return Ok(dados);
                else
                    return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest("Não foi possível obter a lista de alunos! ->" + e);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<AlunoViewModel>> getAlunoById([FromRoute] int Id)
        {
            try
            {
                var dados = await _alunoDB.obterPorId(Id);
                if (dados != null)
                    return Ok(dados);
                else
                    return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest("Não foi possível encontrar o aluno!" + e);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AlunoViewModel>> InsertAluno(
            [FromBody] AlunoInputModel aluno
        )
        {
            try
            {
                var dados = await _alunoDB.insert(aluno);
                return Ok(dados);
            }
            catch (Exception e)
            {
                return UnprocessableEntity("Não foi possível inserir o aluno!" + e);
            }
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<AlunoViewModel>> UpdateAluno(
            [FromRoute] int Id, [FromBody] AlunoInputModel aluno
        )
        {
            try
            {
                var dados = await _alunoDB.update(Id, aluno);
                return Ok(dados);
            }
            catch (Exception)
            {
                return UnprocessableEntity("Não foi possível atualizar o aluno!");
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<AlunoViewModel>> Delete([FromRoute] int Id)
        {
            try
            {
                var dados = await _alunoDB.delete(Id);
                return Ok(dados);
            }
            catch (Exception e)
            {
                return UnprocessableEntity("Não foi possível excluir o aluno! ->" + e);
            }
        }

    }
}
