namespace FciLuxor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddressesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressRequest request)
        {
            var id = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetAddressById), new { id }, new { id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(Guid id)
        {
            var result = await _mediator.Send(new GetAddressByIdRequest { AddressID = id });
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            var result = await _mediator.Send(new GetAllAddressesRequest());
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressRequest request)
        {
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            await _mediator.Send(new DeleteAddressRequest { AddressID = id });
            return NoContent();
        }
    }
}
