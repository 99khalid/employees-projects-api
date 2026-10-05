namespace Application.Features.Addresses.Queries.GetAddressById
{
    public class GetAddressByIdRequest:IRequest<Address?>
    {
        public Guid AddressID {get;set;}
    }
}