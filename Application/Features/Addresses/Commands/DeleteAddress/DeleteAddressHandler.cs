namespace Application.Features.Addresses.Commands.DeleteAddress
{
    public class DeleteAddressHandler : IRequestHandler<DeleteAddressRequest, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAddressHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteAddressRequest request, CancellationToken cancellationToken)
        {
            var address = await _unitOfWork.Addresses.GetByIdAsync(request.AddressID);
            if (address == null)
            {
                throw new KeyNotFoundException($"Address with ID {request.AddressID} not found.");
            }

            _unitOfWork.Addresses.Delete(address);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
