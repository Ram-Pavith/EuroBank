using AutoMapper;
using Azure;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using EuroBankAPI.Service.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace EuroBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly ILogger<CustomerController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public CustomerController(IUnitOfWork context, ILogger<CustomerController> logger, IMapper mapper,IAuthService authService)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult<UserAuthResponseDTO>> CustomerLogin(CustomerLoginDTO customerLogin)
        {
            UserAuthResponseDTO response;
            try
            {
                var request = _mapper.Map<UserAuthLoginDTO>(customerLogin);
                response = await _authService.Login(request);
                if (response.Success)
                    return Ok(response);
                else 
                    return BadRequest(response.Message);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getCustomerAccounts")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AccountBalanceDTO>>> getCustomerAccounts(string CustomerId)
        {
            try
            {
                //Checking if the customer exist
                var CustomerExists = await _context.Customers.GetAsync(x => x.CustomerId == CustomerId);
                if (CustomerExists == null)
                {
                    return NotFound();
                }
                else
                {
                    IEnumerable<Account> customerAccounts = await _context.Accounts.GetAllAsync(x => x.CustomerId == CustomerId);
                    //IEnumerable<AccountBalanceDTO> customerAccountsDTO = _mapper.Map<IEnumerable<AccountBalanceDTO>>(customerAccounts); --error
                    List<AccountBalanceDTO> customerAccountsDTO = _mapper.Map<List<AccountBalanceDTO>>(customerAccounts);
                    return customerAccountsDTO;
                }
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getAccount")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AccountBalanceDTO>> getAccount(Guid AccountId)
        {
            try
            {
                //Checking is account exist
                Account targetAccount = await _context.Accounts.GetAsync(x => x.AccountId == AccountId);
                if (targetAccount == null)
                {
                    return NotFound();
                }
                else
                {
                    AccountBalanceDTO targetAccountDTO = _mapper.Map<AccountBalanceDTO>(targetAccount);
                    return targetAccountDTO;
                }
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getAccountStatement")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StatementDTO>>> getAccountStatement(Guid AccountId, DateTime? from_date, DateTime? to_date)
        {
            try
            {
                //Checking is account exist
                Account targetAccount = await _context.Accounts.GetAsync(x => x.AccountId == AccountId);
                if (targetAccount == null)
                {
                    return NotFound();
                }
                else
                {
                    if (from_date != null && to_date != null)
                    {
                        IEnumerable<Statement> stmt = await _context.Statements.GetAllAsync(x => x.AccountId == AccountId &&
                                                                            x.Date >= from_date && x.Date <= to_date);
                        List<StatementDTO> AccountStatement = _mapper.Map<List<StatementDTO>>(stmt);
                        return AccountStatement;
                    }
                    else
                    {
                        IEnumerable<Statement> stmt = await _context.Statements.GetAllAsync(x => x.AccountId == AccountId &&
                                                                            x.Date.Month == DateTime.Now.Month);
                        List<StatementDTO> AccountStatement = _mapper.Map<List<StatementDTO>>(stmt);
                        return AccountStatement;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
