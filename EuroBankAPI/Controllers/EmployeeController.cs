using AutoMapper;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using EuroBankAPI.Service.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EuroBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public EmployeeController(IUnitOfWork context,ILogger<EmployeeController> logger ,IMapper mapper,IAuthService authService) { 
            _context= context;
            _logger= logger;
            _mapper= mapper;
            _authService= authService;
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<Employee> Register(EmployeeRegisterDTO employeeRegisterDTO)
        {
            var employeeDTO = _mapper.Map<EmployeeDTO>(employeeRegisterDTO);
            _authService.CreatePasswordHash(employeeRegisterDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
            employeeDTO.PasswordHash = Convert.ToString(passwordHash);
            employeeDTO.PasswordSalt = Convert.ToString(passwordSalt);
            Employee employee = _mapper.Map<Employee>(employeeDTO);
            await _context.Employees.CreateAsync(employee);
            return employee;
        }


    }
}
