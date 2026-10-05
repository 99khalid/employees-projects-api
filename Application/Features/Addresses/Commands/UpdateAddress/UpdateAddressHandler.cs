
namespace Application.Features.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressHandler : IRequestHandler<UpdateAddressRequest, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAddressHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateAddressRequest request, CancellationToken cancellationToken)
        {
            var Address = await _unitOfWork.Addresses.GetByIdAsync(request.AddressID);

            if (Address == null)
            {
                throw new KeyNotFoundException($"Address with ID {request.AddressID} not found.");
            }

            // التحقق من أن TownID يشير إلى مدينة موجودة
            if (request.TownID.HasValue)
            {
                var townExists = await _unitOfWork.Towns.GetByIdAsync(request.TownID.Value);
                if (townExists == null)
                {
                    throw new KeyNotFoundException($"Town with ID {request.TownID.Value} not found.");
                }
            }

            // تحديث الخصائص المطلوبة
            
             Address.AddressText=request.AddressText;
             Address.TownID=request.TownID;

            _unitOfWork.Addresses.Update(Address);
            await _unitOfWork.CommitAsync(cancellationToken); // استخدم DbContext لحفظ التغييرات

            return Unit.Value; // إرجاع وحدة القيمة لتشير إلى النجاح
        }

    }
}
