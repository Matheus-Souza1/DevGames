using AutoMapper;
using DevGames.API.Entities;
using DevGames.API.Models;
using DevGames.API.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DevGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardRepository _repository;
        private readonly IMapper _mapper;

        public BoardsController(IBoardRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retornando Todos os Boards
        /// </summary>
        /// <returns>Objeto de detalhes dos boards</returns>
        /// <response code ="404">Boards não encontrados</response>
        /// <response code ="200">Boards encontrados</response>
        [HttpGet]
        public IActionResult GetAll()
        {
            var boards = _repository.GetAll();

            Log.Information($"{boards.Count()} boards retrivied");
            return Ok(boards);
        }

        /// <summary>
        /// Detalhes de um Board
        /// </summary>
        /// <param name="id"> Identificador do Board</param>
        /// <returns>Objeto de detalhes do board</returns>
        /// <response code ="404">Board não encontrado</response>
        /// <response code ="200">Board encontrado</response>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var board = _repository.GetById(id);

            if(board == null)
            {
                return NotFound();
            }

            return Ok(board);
        }

        /// <summary>
        /// Cadastrando um Post
        /// </summary>
        /// <remarks>
        /// Requisição de exemplo:
        /// {
        /// "gameTitle": "Valorant",
        /// "description": "FPS",
        /// "rules": "sem cheater"
        /// }
        /// </remarks>
        /// <param name="model"> Board Data</param>
        /// <returns>Objeto Criado</returns>
        /// <response code="201">Board Criado</response>
        /// <response code="400">Dados invalidos</response>
        [HttpPost]
        public IActionResult Post(AddBoardInputModel model)
        {
            var board = _mapper.Map<Board>(model);

            _repository.Add(board);

            return CreatedAtAction("GetById", new { id = board.Id }, model);
        }


        /// <summary>
        /// Atualizando um Board
        /// </summary>
        /// <remarks>
        /// Requisição de exemplo:
        /// {
        /// "description": "FPS",
        /// "rules": "sem cheater"
        /// }
        /// </remarks>
        /// <param name="id">Identificador do Board</param>
        /// <param name="model">Board Data</param>
        /// <returns>Atualizando o Board</returns>
        /// <response code="204">Board Atualizado</response>
        /// <response code="400">Dados invalidos</response>
        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateBoardInputModel model)
        {
            var board = _repository.GetById(id);

            if (board == null)
            {
                return NotFound();
            }

            board.Update(model.Description,model.Rules);
           
            _repository.Update(board);

            return NoContent();
        }

        /// <summary>
        /// Excluindo um Board
        /// </summary>
        /// <param name="id">Identificador do Board</param>
        /// <returns>Board deletado</returns>
        /// <response code="204">Board deletado</response>
        /// <response code="400">Dados invalidos</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var board = _repository.GetById(id);

            if (board == null)
            {
                return NotFound();
            }

          _repository.Delete(id);

            return NoContent();
        }
    }
}
