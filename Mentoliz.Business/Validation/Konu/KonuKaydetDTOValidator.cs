using FluentValidation;
using Mentoliz.Business.Dto.Konu;

namespace Mentoliz.Business.Validation.Konu;

public class KonuKaydetDTOValidator : AbstractValidator<KonuKaydetDTO>
{
    public KonuKaydetDTOValidator()
    {
        RuleFor(k => k.Ad).NotEmpty().WithMessage("Konu adı boş bırakılamaz.").MaximumLength(200);
    }
}
