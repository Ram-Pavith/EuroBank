namespace EuroBankAPI.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IUserAuthRepository UserAuths { get; }
        void Save();
        void Dispose();
    }
}
