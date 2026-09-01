using FluentValidation;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Deneme;

namespace Mentoliz.Business.Validation.Deneme;

public class DenemeSonucKaydetDTOValidator : AbstractValidator<DenemeSonucKaydetDTO>
{
    public DenemeSonucKaydetDTOValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(d => d.Notlar).MaximumLength(1000);

        RuleForEach(d => d.Detaylar).SetValidator(new DenemeSonucDetayKaydetDTOValidator(unitOfWork));
    }
}
