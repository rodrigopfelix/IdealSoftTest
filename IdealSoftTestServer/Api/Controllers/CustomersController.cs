using IdealSoftTestServer.Application.DTOs.Customers;
using IdealSoftTestServer.Application.DTOs.Phones;
using IdealSoftTestServer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdealSoftTestServer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;
        private readonly IPhoneService _phoneService;

        public CustomersController(ICustomerService service, IPhoneService phoneService)
        {
            _service = service;
            _phoneService = phoneService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var customers = await _service.GetAllCustomersAsync(page, pageSize);
            return Ok(customers);
        }

        #region CRUD Operations

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCustomerByIdAsync([FromRoute] Guid id)
        {
            var customer = await _service.GetCustomerByIdAsync(id)
                ?? throw new KeyNotFoundException("Customer not found.");
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerRequest request)
        {
            var customer = await _service.CreateCustomerAsync(request.FirstName, request.LastName);
            return CreatedAtAction(nameof(GetCustomerByIdAsync), new { id = customer.Id }, customer);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCustomerAsync([FromRoute] Guid id, [FromBody] CustomerRequest request)
        {
            var customer = await _service.UpdateCustomerAsync(id, request.FirstName, request.LastName)
                ?? throw new KeyNotFoundException("Customer not found.");
            return Ok(customer);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCustomerAsync([FromRoute] Guid id)
        {
            var customer = await _service.DeleteCustomerAsync(id)
                ?? throw new KeyNotFoundException("Customer not found.");
            return Ok(customer);
        }

        #endregion

        #region Phone Management

        [HttpGet]
        [Route("{customerId:guid}/phones")]
        public async Task<IActionResult> GetPhonesByCustomerIdAsync([FromRoute] Guid customerId)
        {
            var phones = await _phoneService.GetPhonesByCustomerIdAsync(customerId);
            if (phones == null)
                return NotFound();
            return Ok(phones);
        }

        [HttpGet]
        [Route("{customerId:guid}/phones/{phoneId:guid}")]
        public async Task<IActionResult> GetPhoneByIdAsync([FromRoute] Guid customerId, [FromRoute] Guid phoneId)
        {
            var phones = await _phoneService.GetPhonesByCustomerIdAsync(customerId);
            var phone = phones.Find(p => p.Id == phoneId)
                ?? throw new KeyNotFoundException("Phone not found.");
            return Ok(phone);
        }

        [HttpPost]
        [Route("{customerId:guid}/phones")]
        public async Task<IActionResult> AddPhoneToCustomerAsync([FromRoute] Guid customerId, [FromBody] PhoneRequest request)
        {
            var phone = await _phoneService.CreatePhoneForCustomerAsync(customerId, request.RegionCode, request.Number, request.Type);
            return CreatedAtAction(nameof(GetPhoneByIdAsync), new { customerId = customerId, phoneId = phone.Id }, phone);
        }

        [HttpPut]
        [Route("{customerId:guid}/phones/{phoneId:guid}")]
        public async Task<IActionResult> UpdatePhoneAsync([FromRoute] Guid customerId, [FromRoute] Guid phoneId, [FromBody] PhoneRequest request)
        {
            var phones = await _phoneService.GetPhonesByCustomerIdAsync(customerId);
            var phone = phones.Find(p => p.Id == phoneId)
                ?? throw new KeyNotFoundException("Phone not found.");

            phone = await _phoneService.UpdatePhoneAsync(phoneId, request.RegionCode, request.Number, request.Type);
            return Ok(phone);
        }

        [HttpDelete]
        [Route("{customerId:guid}/phones/{phoneId:guid}")]
        public async Task<IActionResult> DeletePhoneAsync([FromRoute] Guid customerId, [FromRoute] Guid phoneId)
        {
            var phones = await _phoneService.GetPhonesByCustomerIdAsync(customerId);
            var phone = phones.Find(p => p.Id == phoneId)
                ?? throw new KeyNotFoundException("Phone not found.");
            phone = await _phoneService.DeletePhoneAsync(phoneId);
            return Ok(phone);
        }

        #endregion
    }
}
