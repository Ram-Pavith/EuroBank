namespace EuroBankAPI.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public IEmployeeRepository Employees { get; }
        IUserAuthRepository UserAuths { get; }
        void Save();
        void Dispose();
    }
}
