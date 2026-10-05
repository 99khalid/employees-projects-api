
namespace Application.Features.Addresses.Commands.CreateAddress
{
    public class CreateAddressHandler : IRequestHandler<CreateAddressRequest, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAddressHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateAddressRequest request, CancellationToken cancellationToken)
        {
             // التحقق من أن TownID يشير إلى مدينة موجودة
            if (request.TownID.HasValue)
            {
                var townExists = await _unitOfWork.Towns.GetByIdAsync(request.TownID.Value);
                if (townExists == null)
                {
                    throw new KeyNotFoundException($"Town with ID {request.TownID.Value} not found.");
                }
            }
            var newAddress = new Address
            {
                AddressText = request.AddressText,
                TownID = request.TownID
            };

            await _unitOfWork.Addresses.AddAsync(newAddress);
            await _unitOfWork.CommitAsync(cancellationToken); //   لحفظ التغييرات

            return newAddress.AddressID;
        }

       
    }
}
