namespace Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectRequest, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProjectHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectID);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {request.ProjectID} not found.");
            }

            // A project that still has employees assigned can't be deleted.
            var employeesProjects = await _unitOfWork.EmployeeProjects.GetAllAsync(ep => ep.ProjectID == request.ProjectID);
            if (employeesProjects.Any())
            {
                throw new InvalidOperationException($"Cannot delete project with ID {request.ProjectID} because it has employees assigned.");
            }

            _unitOfWork.Projects.Delete(project);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
