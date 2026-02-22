using AtosCustomer.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using static AtosCustomer.Api.DTOs.CustomerModels;

namespace AtosCustomer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _repo;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerRepository repo, ILogger<CustomersController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Add([FromBody] CreateCustomerRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Firstname) || string.IsNullOrWhiteSpace(request.Surname))
            {
                return BadRequest();
            }

            var created = _repo.Add(request.Firstname.Trim(), request.Surname.Trim());

            if (created is null)
                return Conflict(new { error = "Customer already exists." });

            _logger.LogInformation("Customer created: {CustomerId}", created.Id);

            return StatusCode(StatusCodes.Status201Created, created);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Remove(int id)
        {
            if (id <= 0)
                return BadRequest();

            var removed = _repo.Remove(id);

            if (!removed)
                return NotFound();

            _logger.LogInformation("Removed Customer with Id: {CustomerId}", id);

            return NoContent();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var customers = _repo.GetAll();

            _logger.LogInformation("Retrieved all {CustomerCount} customers", customers.Count);
            return Ok(customers);
        }
    }
}
