namespace FciLuxor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
        {
            var id = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetProjectById), new { id }, new { id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var result = await _mediator.Send(new GetProjectByIdRequest { ProjectID = id });
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var result = await _mediator.Send(new GetAllProjectsRequest());
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectRequest request)
        {
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            await _mediator.Send(new DeleteProjectRequest { ProjectID = id });
            return NoContent();
        }
    }
}
