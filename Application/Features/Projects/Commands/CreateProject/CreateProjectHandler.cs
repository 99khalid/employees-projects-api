
namespace Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectRequest, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProjectHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
        {
            var newProject = new Project
            {
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            await _unitOfWork.Projects.AddAsync(newProject);
            await _unitOfWork.CommitAsync(cancellationToken); // استخدم DbContext لحفظ التغييرات

            return newProject.ProjectID;
        }

    }
}
