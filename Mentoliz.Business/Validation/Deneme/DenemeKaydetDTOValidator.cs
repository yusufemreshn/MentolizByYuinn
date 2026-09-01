using FluentValidation;
using Mentoliz.Business.Dto.Deneme;

namespace Mentoliz.Business.Validation.Deneme;

public class DenemeKaydetDTOValidator : AbstractValidator<DenemeKaydetDTO>
{
    public DenemeKaydetDTOValidator()
    {
        RuleFor(d => d.Ad).NotEmpty().WithMessage("Deneme adı boş bırakılamaz.").MaximumLength(200);

        RuleFor(d => d.YayinAdi).MaximumLength(100);

        RuleFor(d => d.Aciklama).MaximumLength(1000);

        RuleFor(d => d.Tarih).NotEmpty().WithMessage("Deneme tarihi boş bırakılamaz.");
    }
}
