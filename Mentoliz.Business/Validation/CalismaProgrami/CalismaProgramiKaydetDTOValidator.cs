using FluentValidation;
using Mentoliz.Business.Dto.CalismaProgrami;

namespace Mentoliz.Business.Validation.CalismaProgrami;

public class CalismaProgramiKaydetDTOValidator : AbstractValidator<CalismaProgramiKaydetDTO>
{
    public CalismaProgramiKaydetDTOValidator()
    {
        RuleFor(c => c.Aciklama).MaximumLength(500);

        RuleForEach(c => c.Satirlar).ChildRules(satir =>
        {
            satir.RuleFor(s => s.Aciklama).MaximumLength(500);

            satir.RuleFor(s => s.BitisSaati)
                .GreaterThan(s => s.BaslangicSaati)
                .WithMessage("Bitiş saati başlangıç saatinden sonra olmalı.");
        });
    }
}
