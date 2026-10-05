namespace Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectRequest : IRequest
    {
        public Guid ProjectID { get; set; }
    }
}
