

namespace Application.Features.Towns.Queries.GetTownById
{
    public class GetTownByIdHandler : IRequestHandler<GetTownByIdRequest, Town?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTownByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Town?> Handle(GetTownByIdRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Towns.GetByIdAsync(request.TownID);
        }
    }

}
