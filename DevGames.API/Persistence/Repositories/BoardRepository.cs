using DevGames.API.Entities;

namespace DevGames.API.Persistence.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly DevGamesContext _context;

        public BoardRepository(DevGamesContext context)
        {
            _context = context;
        }
        public void Add(Board board)
        {
            _context.Boards.Add(board);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var board = _context.Boards.SingleOrDefault(x => x.Id == id);
            _context.Boards.Remove(board);
            _context.SaveChanges();
        }

        public IEnumerable<Board> GetAll()
        {
            return _context.Boards.ToList();
        }

        public Board GetById(int id)
        {
           return _context.Boards.SingleOrDefault(x => x.Id == id);
        }

        public void Update(Board board)
        {
            _context.Boards.Update(board);
            _context.SaveChanges();
        }
    }
}
