using EuroBankAPI.Models;

namespace EuroBankAPI.Repository.IRepository
{
    public interface IStatementRepository:IGenericRepository<Statement>
    {
        Task<Statement> UpdateAsync(Statement statement);

    }
}
