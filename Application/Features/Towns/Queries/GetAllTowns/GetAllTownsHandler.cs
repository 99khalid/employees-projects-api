namespace Application.Features.Towns.Queries.GetAllTowns
{
    public class GetAllTownsHandler : IRequestHandler<GetAllTownsRequest, IEnumerable<Town>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllTownsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Town>> Handle(GetAllTownsRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Towns.GetAllAsync();
        }
    }
}
