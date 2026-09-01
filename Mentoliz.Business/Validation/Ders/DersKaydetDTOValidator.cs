using FluentValidation;
using Mentoliz.Business.Dto.Ders;

namespace Mentoliz.Business.Validation.Ders;

public class DersKaydetDTOValidator : AbstractValidator<DersKaydetDTO>
{
    public DersKaydetDTOValidator()
    {
        RuleFor(d => d.Ad).NotEmpty().WithMessage("Ders adı boş bırakılamaz.").MaximumLength(100);

        RuleFor(d => d.KisaAd).NotEmpty().WithMessage("Kısa ad boş bırakılamaz.").MaximumLength(20);
    }
}
