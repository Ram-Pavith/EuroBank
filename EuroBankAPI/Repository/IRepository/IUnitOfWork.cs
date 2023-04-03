namespace EuroBankAPI.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public IEmployeeRepository Employees { get; }
        void Save();
        void Dispose();
    }
}
