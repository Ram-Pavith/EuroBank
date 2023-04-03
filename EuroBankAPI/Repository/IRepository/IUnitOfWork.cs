namespace EuroBankAPI.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public IEmployeeRepository Employees { get; }
        public ICustomerRepository Customers { get; }
        public ICustomerCreationStatusRepository CustomerCreationStatuses { get; }
        public IAccountRepository Accounts { get; }
        public IAccountCreationStatusRepository AccountCreationStatuses { get; }
        public IStatementRepository Statements { get; }
        public IAccountTypeRepository AccountTypes { get; }
        public ITransactionStatusRepository TransactionStatuses { get; }
        IUserAuthRepository UserAuths { get; }
        void Save();
        void Dispose();
    }
}
