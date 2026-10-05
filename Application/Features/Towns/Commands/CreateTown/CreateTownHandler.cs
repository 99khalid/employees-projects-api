
namespace Application.Features.Towns.Commands.CreateTown
{
    public class CreateTownHandler : IRequestHandler<CreateTownRequest, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateTownRequest request, CancellationToken cancellationToken)
        {
            var newTown = new Town
            {
                Name = request.Name // افترض أن لديك خاصية اسمية
                // إضافة أي خصائص أخرى لازمة
            };

            await _unitOfWork.Towns.AddAsync(newTown);
            await _unitOfWork.CommitAsync(cancellationToken); // استخدم DbContext لحفظ التغييرات

            return newTown.TownID;
        }
    }
}
