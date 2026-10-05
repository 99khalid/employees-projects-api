namespace Application.Features.Towns.Commands.DeleteTown
{
    public class DeleteTownHandler : IRequestHandler<DeleteTownRequest, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteTownRequest request, CancellationToken cancellationToken)
        {
            var town = await _unitOfWork.Towns.GetByIdAsync(request.TownID);
            if (town == null)
            {
                throw new KeyNotFoundException($"Town with ID {request.TownID} not found.");
            }

            // A town that still has addresses can't be deleted.
            var addresses = await _unitOfWork.Addresses.GetAllAsync(a => a.TownID == request.TownID);
            if (addresses.Any())
            {
                throw new InvalidOperationException($"Cannot delete town with ID {request.TownID} because it has associated addresses.");
            }

            _unitOfWork.Towns.Delete(town);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
