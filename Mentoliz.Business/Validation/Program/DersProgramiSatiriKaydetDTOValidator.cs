using FluentValidation;
using Mentoliz.Business.Dto.Program;

namespace Mentoliz.Business.Validation.Program;

public class DersProgramiSatiriKaydetDTOValidator : AbstractValidator<DersProgramiSatiriKaydetDTO>
{
    public DersProgramiSatiriKaydetDTOValidator()
    {
        RuleFor(d => d.BitisSaati)
            .GreaterThan(d => d.BaslangicSaati)
            .WithMessage("Bitiş saati başlangıç saatinden sonra olmalı.");

        RuleFor(d => d.DerslikAdi).MaximumLength(50);

        RuleFor(d => d.OgretmenAdi).MaximumLength(100);
    }
}
