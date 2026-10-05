

namespace Application.Features.Projects.Queries.GetProjectById
{
    public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdRequest, Project?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProjectByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Project?> Handle(GetProjectByIdRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Projects.GetByIdAsync(request.ProjectID);
        }
    }

}
