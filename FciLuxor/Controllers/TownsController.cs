namespace FciLuxor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TownsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TownsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTown([FromBody] CreateTownRequest request)
        {
            var id = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetTownById), new { id }, new { id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTownById(Guid id)
        {
            var result = await _mediator.Send(new GetTownByIdRequest { TownID = id });
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTowns()
        {
            var result = await _mediator.Send(new GetAllTownsRequest());
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTown([FromBody] UpdateTownRequest request)
        {
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTown(Guid id)
        {
            await _mediator.Send(new DeleteTownRequest { TownID = id });
            return NoContent();
        }
    }
}
