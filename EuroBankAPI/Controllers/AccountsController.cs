using AutoMapper;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EuroBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork _uw;
        private readonly IMapper _mapper;
        public AccountsController(IUnitOfWork uw,IMapper mapper)
        {
            _uw = uw;
            _mapper = mapper;
        }


        [HttpPost]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AccountCreationStatusDTO>> createAccount(string CustomerId)
        {
            //Checking if the customer exist
            var CustomerExists = await _uw.Customers.GetAsync(x => x.CustomerId == CustomerId);
            if (CustomerExists == null)
            {
                return NotFound();
            }
            else
            {
                //Checking if the customer already has an acocunt of Type "Current"
                var AccountExists = await _uw.Accounts.GetAsync(x => x.CustomerId == CustomerId && x.AccountTypeId == 2);
                if (AccountExists == null)
                {
                    return BadRequest();
                }
                else
                {
                    //Creating a new account 
                    Account newAcc = new Account();
                    newAcc.CustomerId = CustomerId;
                    newAcc.AccountTypeId = 2;   //current account

                    newAcc.AccountCreationStatusId = 1;     // success status
                    await _uw.Accounts.CreateAsync(newAcc);
                    AccountCreationStatus acs = await _uw.AccountCreationStatuses.GetAsync(x => x.AccountCreationStatusId == newAcc.AccountCreationStatusId);
                    AccountCreationStatusDTO accCreationStatusDTO = _mapper.Map<AccountCreationStatusDTO>(acs);
                    return accCreationStatusDTO;
                }                      
            }          
        }


        [HttpGet]
        [Authorize(Roles = "Employee, Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AccountBalanceDTO>>> getCustomerAccounts(string CustomerId)
        {
            //Checking if the customer exist
            var CustomerExists = await _uw.Customers.GetAsync(x => x.CustomerId == CustomerId);
            if (CustomerExists == null)
            {
                return NotFound();
            }
            else
            {
                IEnumerable<Account> customerAccounts = await _uw.Accounts.GetAllAsync(x => x.CustomerId == CustomerId);
                //IEnumerable<AccountBalanceDTO> customerAccountsDTO = _mapper.Map<IEnumerable<AccountBalanceDTO>>(customerAccounts); --error
                List<AccountBalanceDTO> customerAccountsDTO = _mapper.Map<List<AccountBalanceDTO>>(customerAccounts);
                return customerAccountsDTO;
            }
        }


    }
}
