namespace EuroBankAPI.Repository.IRepository
{
    public interface IUnitOfWork
    {
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
        void Save();
        void Dispose();
    }
}
