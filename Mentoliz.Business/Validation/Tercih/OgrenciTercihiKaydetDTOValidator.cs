using FluentValidation;
using Mentoliz.Business.Dto.Tercih;

namespace Mentoliz.Business.Validation.Tercih;

public class OgrenciTercihiKaydetDTOValidator : AbstractValidator<OgrenciTercihiKaydetDTO>
{
    public OgrenciTercihiKaydetDTOValidator()
    {
        RuleFor(t => t.Sira).GreaterThan(0).WithMessage("Tercih sırası birden küçük olamaz.");

        RuleFor(t => t.UniversiteAdi).NotEmpty().WithMessage("Üniversite adı boş bırakılamaz.").MaximumLength(200);

        RuleFor(t => t.BolumAdi).NotEmpty().WithMessage("Bölüm adı boş bırakılamaz.").MaximumLength(200);

        RuleFor(t => t.TabanPuan).GreaterThan(0).WithMessage("Taban puan sıfırdan büyük olmalı.").When(t => t.TabanPuan.HasValue);

        RuleFor(t => t.Notlar).MaximumLength(500);
    }
}
