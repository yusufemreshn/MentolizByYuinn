using FluentValidation;
using Mentoliz.Business.Dto.Gorev;

namespace Mentoliz.Business.Validation.Gorev;

public class GorevKaydetDTOValidator : AbstractValidator<GorevKaydetDTO>
{
    public GorevKaydetDTOValidator()
    {
        RuleFor(g => g.Baslik).NotEmpty().WithMessage("Görev başlığı boş bırakılamaz.").MaximumLength(200);

        RuleFor(g => g.Aciklama).MaximumLength(1000);

        RuleFor(g => g.Tarih).NotEmpty().WithMessage("Görev tarihi boş bırakılamaz.");
    }
}
