
namespace Application.Features.Towns.Commands.UpdateTown
{
    public class UpdateTownHandler : IRequestHandler<UpdateTownRequest, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateTownRequest request, CancellationToken cancellationToken)
        {
            var town = await _unitOfWork.Towns.GetByIdAsync(request.TownID);

            if (town == null)
            {
                throw new KeyNotFoundException($"Town with ID {request.TownID} not found.");
            }

            // تحديث الخصائص المطلوبة
            town.Name = request.Name; // افترض أن لديك خاصية اسمية

            _unitOfWork.Towns.Update(town);
            await _unitOfWork.CommitAsync(cancellationToken); // استخدم DbContext لحفظ التغييرات

            return Unit.Value; // إرجاع وحدة القيمة لتشير إلى النجاح
        }
    }
}
