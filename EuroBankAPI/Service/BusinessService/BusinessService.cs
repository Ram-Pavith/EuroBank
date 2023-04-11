using EuroBankAPI.Repository.IRepository;

namespace EuroBankAPI.Service.BusinessService
{
    public class BusinessService : IBusinessService
    {
        private readonly int savingsAccountTypeId = 1;
        private readonly int currentAccountTypeId = 2;
        private readonly double savingsAccountMinBalance = 10000;
        private readonly double currentAccountMinBalance = 500;
        private readonly double accountMinBalance = 1000;
        private readonly double serviceCharges = 10;
        public double EvaluateMinBalance(int AccountTypeId)
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
        public double ServiceCharges(int ServiceCharges)
        {
            return serviceCharges;
        }
    }
}
