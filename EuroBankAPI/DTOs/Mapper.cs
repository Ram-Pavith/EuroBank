using AutoMapper;
using EuroBankAPI.Models;

namespace EuroBankAPI.DTOs
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            //Employee Mapper
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            //Customer Mapper
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<CustomerCreationStatus, CustomerCreationStatusDTO>().ReverseMap();
            //Transaction Mapper
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
            CreateMap<RefTransactionStatus,RefTransactionStatusDTO>().ReverseMap();
            CreateMap<RefTransactionType,RefTransactionTypeDTO>().ReverseMap();
            CreateMap<Models.Service, ServiceDTO>().ReverseMap();
            CreateMap<RefPaymentMethod, RefPaymentMethodDTO>().ReverseMap();
            CreateMap<CounterParty, CounterPartyDTO>().ReverseMap();
            //Accounts Mapper
            CreateMap<Account,AccountDTO>().ReverseMap();
            CreateMap<AccountType,AccountTypeDTO>().ReverseMap();   
            CreateMap<TransactionStatus,TransactionStatusDTO>().ReverseMap();
            CreateMap<AccountCreationStatus, AccountCreationStatusDTO>().ReverseMap();
            CreateMap<Statement,StatementDTO>().ReverseMap();
        }
    }
}
