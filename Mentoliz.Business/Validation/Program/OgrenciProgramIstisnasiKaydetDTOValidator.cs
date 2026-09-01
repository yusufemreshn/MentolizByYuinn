using FluentValidation;
using Mentoliz.Business.Dto.Program;

namespace Mentoliz.Business.Validation.Program;

public class OgrenciProgramIstisnasiKaydetDTOValidator : AbstractValidator<OgrenciProgramIstisnasiKaydetDTO>
{
    public OgrenciProgramIstisnasiKaydetDTOValidator()
    {
        RuleFor(o => o.BitisSaati)
            .GreaterThan(o => o.BaslangicSaati)
            .WithMessage("Bitiş saati başlangıç saatinden sonra olmalı.");

        RuleFor(o => o.OgretmenAdi).MaximumLength(100);

        RuleFor(o => o.Aciklama).MaximumLength(1000);

        RuleFor(o => o.BitisTarihi)
            .GreaterThanOrEqualTo(o => o.BaslangicTarihi!.Value)
            .WithMessage("Bitiş tarihi başlangıç tarihinden önce olamaz.")
            .When(o => o.BaslangicTarihi.HasValue && o.BitisTarihi.HasValue);
    }
}
