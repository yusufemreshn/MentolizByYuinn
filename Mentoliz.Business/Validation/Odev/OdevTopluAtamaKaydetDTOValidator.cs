using FluentValidation;
using Mentoliz.Business.Dto.Odev;

namespace Mentoliz.Business.Validation.Odev;

public class OdevTopluAtamaKaydetDTOValidator : AbstractValidator<OdevTopluAtamaKaydetDTO>
{
    public OdevTopluAtamaKaydetDTOValidator()
    {
        RuleFor(o => o.Baslik).NotEmpty().WithMessage("Başlık boş bırakılamaz.").MaximumLength(200);

        RuleFor(o => o.DersId).GreaterThan(0).WithMessage("Ders seçilmelidir.");

        RuleFor(o => o.SonTeslimTarihi)
            .GreaterThanOrEqualTo(o => o.VerilisTarihi)
            .WithMessage("Son teslim tarihi veriliş tarihinden önce olamaz.");
    }
}
