namespace EuroBankAPI.Repository.IRepository
{
    public interface IServiceRepository:IGenericRepository<Models.Service>
    {
        Task <Models.Service> UpdateAsync(Models.Service service);
    }
}
