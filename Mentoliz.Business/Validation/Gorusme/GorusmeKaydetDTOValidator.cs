using FluentValidation;
using Mentoliz.Business.Dto.Gorusme;

namespace Mentoliz.Business.Validation.Gorusme;

public class GorusmeKaydetDTOValidator : AbstractValidator<GorusmeKaydetDTO>
{
    public GorusmeKaydetDTOValidator()
    {
        RuleFor(g => g.Konu).NotEmpty().WithMessage("Görüşme konusu boş bırakılamaz.").MaximumLength(200);

        RuleFor(g => g.Notlar).NotEmpty().WithMessage("Görüşme notu boş bırakılamaz.").MaximumLength(4000);

        RuleFor(g => g.AlinanKarar).MaximumLength(1000);

        RuleFor(g => g.Tarih).NotEmpty().WithMessage("Görüşme tarihi boş bırakılamaz.");

        // dakika olarak makul bir üst sınır koyduk, yanlışlıkla saat girilmesin diye
        RuleFor(g => g.Sure)
            .InclusiveBetween(1, 480).WithMessage("Görüşme süresi bir ile dört yüz seksen dakika arasında olmalı.")
            .When(g => g.Sure.HasValue);
    }
}
