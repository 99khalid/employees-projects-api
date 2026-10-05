

namespace Application.Features.Addresses.Queries.GetAddressById
{
    public class GetAddressByIdHandler : IRequestHandler<GetAddressByIdRequest, Address?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAddressByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Address?> Handle(GetAddressByIdRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Addresses.GetByIdAsync(request.AddressID);
        }
    }
}