using FluentValidation;
using Mentoliz.Business.Dto.Konu;

namespace Mentoliz.Business.Validation.Konu;

public class OgrenciKonuTakipKaydetDTOValidator : AbstractValidator<OgrenciKonuTakipKaydetDTO>
{
    public OgrenciKonuTakipKaydetDTOValidator()
    {
        RuleFor(o => o.Notlar).MaximumLength(1000);
    }
}
