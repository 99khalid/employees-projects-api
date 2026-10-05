

namespace Application.Features.Towns.Queries.GetTownById
{
    public class GetTownByIdRequest : IRequest<Town?>
    {
        public Guid TownID { get; set; }
    }
}
