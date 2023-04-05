using EuroBankAPI.Data;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Repository
{
    public class StatementRepository:GenericRepository<Statement>,IStatementRepository
    {
        private EuroBankContext _db;
        public StatementRepository(EuroBankContext db):base(db)
        {
            _db = db;
        }

        public async Task<Statement> UpdateAsync(Statement statement)
        {
            _db.Statements.Update(statement);
            await _db.AddRangeAsync();
            return statement;

        }
    }
}
