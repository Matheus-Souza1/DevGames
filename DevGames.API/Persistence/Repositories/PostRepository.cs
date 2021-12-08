using DevGames.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevGames.API.Persistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DevGamesContext _context;
        public PostRepository(DevGamesContext context)
        {
            _context = context;
        }
        public void Add(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public Post GetById(int id)
        {
            var post = _context.Posts.Include(x => x.Comments).SingleOrDefault(x => x.Id == id);
            return post;
        }

        public IEnumerable<Post> GetAll(int boardId)
        {
            return _context.Posts.Where(x => x.BoardId == boardId);
        }

        public bool PostExists(int postId)
        {
            return _context.Posts.Any(x => x.Id == postId);
        }
    }
}
