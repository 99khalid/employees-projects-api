
namespace Application.Features.Addresses.Commands.CreateAddress
{
    public class CreateAddressRequest : IRequest<Guid>
    {
        public string AddressText { get; set; }
        public Guid? TownID { get; set; }    
    }
}


