using EuroBankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EuroBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork _uw;

        public AccountsController(IUnitOfWork uw)
        {
            _uw = uw;
        }

        [HttpPost]
        public 
    }
}
