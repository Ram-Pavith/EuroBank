using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Service.BusinessService
{
    public class BusinessService : IBusinessService
    {
        private readonly int savingsAccountTypeId = 1;
        private readonly int currentAccountTypeId = 2;
        private readonly double savingsAccountMinBalance = 2000;
        private readonly double currentAccountMinBalance = 500;
        private readonly double accountMinBalance = 1000;
        public double EvaluateMinBalance(int AccountTypeId, double Balance)
        {
            if(AccountTypeId == savingsAccountTypeId)
            {
                return savingsAccountMinBalance;
            }
            if(AccountTypeId == currentAccountTypeId)
            {
                return currentAccountMinBalance;
            }
            return accountMinBalance;

        }
    }
}
