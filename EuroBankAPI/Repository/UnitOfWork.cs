using EuroBankAPI.Data;
using EuroBankAPI.Repository.IRepository;
using EuroBankAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EuroBankAPI.Repository
{
    public class UnitOfWork:IUnitOfWork
    {

        private readonly EuroBankContext _db;
        public UnitOfWork(EuroBankContext db)
        {
            _db = db;
            Employees = new EmployeeRepository(db);
            UserAuths = new UserAuthRepository(db);
            Transactions=new TransactionRepository(db);
            CounterParties=new CounterPartyRepository(db);
            RefTransactionTypes=new RefTransactionTypeRepository(db);
            RefTransactionStatuses=new RefTransactionStatusRepository(db);
            Services=new ServiceRepository(db);
            RefPaymentMethods=new RefPaymentMethodRepository(db);
            Accounts = new AccountRepository(db);
            AccountTypes = new AccountTypeRepository(db);
            AccountCreationStatuses = new AccountCreationStatusRepository(db);
            Statements = new StatementRepository(db);
            TransactionStatuses = new TransactionStatusRepository(db);
            Customers = new CustomerRepository(db);
            CustomerCreationStatuses = new CustomerCreationStatusRepository(db);
        }
        public IEmployeeRepository Employees { get; }
        public IUserAuthRepository UserAuths { get; }
        public ITransactionRepository Transactions { get; }
        public ICounterPartyRepository CounterParties { get; }
        public IRefTransactionTypeRepository RefTransactionTypes { get; }
        public IRefTransactionStatusRepository RefTransactionStatuses { get; }
        public IServiceRepository Services { get; }
        public IRefPaymentMethodRepository RefPaymentMethods { get; }
        public ICustomerRepository Customers { get; }
        public ICustomerCreationStatusRepository CustomerCreationStatuses { get; }
        public IAccountRepository Accounts { get; }
        public IAccountCreationStatusRepository AccountCreationStatuses { get; }
        public IStatementRepository Statements { get; }
        public IAccountTypeRepository AccountTypes { get; }
        public ITransactionStatusRepository TransactionStatuses { get; }

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
