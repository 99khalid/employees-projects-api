namespace Application.Features.Projects.Queries.GetProjectById
{
    public class GetProjectByIdRequest : IRequest<Project?>
    {
        public Guid ProjectID { get; set; }
    }
}