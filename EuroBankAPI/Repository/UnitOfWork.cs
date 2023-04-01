using EuroBankAPI.Data;

namespace EuroBankAPI.Repository
{
    public class UnitOfWork
    {
        private readonly EuroBankContext _db;
        public UnitOfWork(EuroBankContext db)
        {
            _db = db;

        }

        public void Save()
        {
            _db.SaveChanges();
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
