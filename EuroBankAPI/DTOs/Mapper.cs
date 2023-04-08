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
            CreateMap<EmployeeRegisterDTO, EmployeeDTO>().ReverseMap();
            CreateMap<EmployeeDTO, EmployeeDetailsDTO>().ReverseMap();

            //Customer Mapper
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<CustomerCreationStatus, CustomerCreationStatusDTO>().ReverseMap();
            CreateMap<CustomerRegisterDTO, CustomerDTO>();
            CreateMap<CustomerDTO, CustomerDetailsDTO>().ReverseMap();
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
            CreateMap<Account, AccountBalanceDTO>()
                .ForMember(dest => dest.AccountId, map => map.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.Balance, map => map.MapFrom(src => src.Balance));

            //EmployeeLoginDTO and UserAuthDTO
            CreateMap<EmployeeLoginDTO, UserAuthLoginDTO>()
                .ForMember(dest => dest.Role, map=> map.MapFrom(src => "Employee"/*src.EmployeeId is string ?"Employee":"Employee"*/  ))
                .ForMember(dest => dest.Username, map => map.MapFrom(src => src.EmailId));
            //CustomerLoginDTO and UserAuthDTO
            CreateMap<CustomerLoginDTO, UserAuthLoginDTO>()
                .ForMember(dest => dest.Role, map => map.MapFrom(src => "Customer"))
                .ForMember(dest => dest.Username, map=>map.MapFrom(src => src.EmailId));

        }
    }
}
