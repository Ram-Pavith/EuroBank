﻿namespace EuroBankAPI.Service.BusinessService
{
    public interface IBusinessService
    {
        public double EvaluateMinBalance(int AccountTypeId);
        public double ServiceCharges(int ServiceCharges);

    }
}
