using API_DB.Models.Data;
using API_DB.Models.InputModels;
using API_DB.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_DB.Controllers
    {
        [Route("api/v1/[controller]")]
        [ApiController]
        public class TipoAlunoController : ControllerBase
        {
            private readonly ILogger<TipoAlunoController> _logger;
            private readonly TipoAlunoDB _tipoalunoDB;

            public TipoAlunoController(ILogger<TipoAlunoController> logger, TipoAlunoDB tipoalunoDB)
            {
                _logger = logger;
                _tipoalunoDB = tipoalunoDB;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<TipoAlunoViewModel>>> getTipoAlunos()
            {
                try
                {
                    var dados = await _tipoalunoDB.listar();
                    if (dados != null)
                        return Ok(dados);
                    else
                        return NoContent();
                }
                catch (Exception)
                {
                    return BadRequest("Não foi possível obter a lista de tipoalunos! ");
                }
            }

            [HttpGet("{Id}")]
            public async Task<ActionResult<TipoAlunoViewModel>> getTipoAlunoById([FromRoute] int Id)
            {
                try
                {
                    var dados = await _tipoalunoDB.obterPorId(Id);
                    if (dados != null)
                        return Ok(dados);
                    else
                        return NoContent();
                }
                catch (Exception)
                {
                    return BadRequest("Não foi possível encontrar o tipoaluno!");
                }
            }

            [HttpPost]
            public async Task<ActionResult<TipoAlunoViewModel>> InsertTipoAluno([FromBody] TipoAlunoInputModel tipoAluno)
            {
                try
                {
                    var dados = await _tipoalunoDB.insert(tipoAluno);
                    return Ok(dados);
                }
                catch (Exception)
                {
                    return UnprocessableEntity("Não foi possível inserir o tipoaluno!");
                }
            }

            [HttpPut("{Id}")]
            public async Task<ActionResult<TipoAlunoViewModel>> UpdateTipoAluno(
                [FromRoute] int Id, [FromBody] TipoAlunoInputModel aluno
            )
            {
                try
                {
                    var dados = await _tipoalunoDB.update(Id, aluno);
                    return Ok(dados);
                }
                catch (Exception )
                {
                    return UnprocessableEntity("Não foi possível atualizar o tipoaluno!" );
                }
            }

            [HttpDelete("{Id}")]
            public async Task<ActionResult<TipoAlunoViewModel>> Delete([FromRoute] int Id)
            {
                try
                {
                    var dados = await _tipoalunoDB.delete(Id);
                    return Ok(dados);
                }
                catch (Exception )
                {
                    return UnprocessableEntity("Não foi possível excluir o tipoaluno!");
                }
            }

        }
    }


