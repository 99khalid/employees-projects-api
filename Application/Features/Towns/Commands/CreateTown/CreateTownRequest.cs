
namespace Application.Features.Towns.Commands.CreateTown
{
    public class CreateTownRequest : IRequest<Guid>
    {
        public string Name { get; set; }
    }
}


