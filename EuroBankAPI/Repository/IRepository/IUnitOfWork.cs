namespace EuroBankAPI.Repository.IRepository
{
    public interface IUnitOfWork
    {
        void Save();
        void Dispose();
    }
}
