using DevGames.API.Entities;
using DevGames.API.Models;
using DevGames.API.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevGames.API.Controllers
{
    [Route("api/boards/{id}/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _repository;

        public PostsController(IPostRepository repository)
        {

            _repository = repository;
        }


        /// <summary>
        /// Retornando todos os Post
        /// </summary>
        /// <param name="id">Identificador do Board</param>
        /// <returns>Retornando os Posts</returns>
        /// <response code="404">Posts não encontrados</response>
        /// <response code="200">Posts encontrados</response>
        [HttpGet]
        public IActionResult GetAll(int id)
        {
            var posts = _repository.GetAll(id);

            return Ok(posts);
        }

        /// <summary>
        /// Buscando detalhes de um Post
        /// </summary>
        /// <param name="postId">Identificador do post</param>
        /// <returns>Detalhes do post</returns>
        /// <response code="404">Post não encontrado</response>
        /// <response code="200">Post encontrado</response>
        [HttpGet("{postId}")]
        public IActionResult GetById(int id, int postId)
        {

            var post = _repository.GetById(postId);

            if(post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        /// <summary>
        /// Criando um Post
        /// </summary>
        /// <remarks>
        /// Requisição de exemplo:
        /// {
        /// "title": "Valorant",
        /// "description": "FPS",
        /// "user": "Galo"
        /// }
        /// </remarks>
        /// <param name="id">identificador do Board</param>
        /// <param name="model">Post Data</param>
        /// <returns>Objeto do Post</returns>
        /// <response code="201">Post Criado</response>
        /// <response code="400">Dados invalido</response>
        [HttpPost]
        public IActionResult Post(int id, AddPostInputModel model)
        {
            var post = new Post(model.Title, model.Description, id);

            _repository.Add(post);

            return CreatedAtAction("GetById", new { id = post.Id, postId = post.Id }, model);
        }


        /// <summary>
        /// Criando um comentario para o Post
        /// </summary>
        /// <remarks>
        /// Requisição de exemplo:
        /// {
        /// "title": "Valorant",
        /// "description": "FPS",
        /// "user": "Galo"
        /// }
        /// </remarks>
        /// <param name="id"> Identificador do Board</param>
        /// <param name="postId">Identificador do Post</param>
        /// <param name="model">Objeto com dados de cadastro do commentario</param>
        /// <returns>Objeto criado</returns>
        /// <response code="204">Comentario Criado</response>
        /// <response code="400">Dados invalido</response>
        [HttpPost("{postId}/comments")]
        public IActionResult PostComment(int id, int postId, AddCommentInputModel model)
        {
     
            var postExists = _repository.PostExists(postId);

            if(!postExists)
            {
                return NotFound();
            }

            var comment = new Comment(model.Title, model.Description, model.User, postId);

            _repository.AddComment(comment);

            return NoContent();
        }

    }
}
