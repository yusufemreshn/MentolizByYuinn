using FluentValidation;
using Mentoliz.Business.Dto.Odev;

namespace Mentoliz.Business.Validation.Odev;

public class OdevKaydetDTOValidator : AbstractValidator<OdevKaydetDTO>
{
    public OdevKaydetDTOValidator()
    {
        RuleFor(o => o.KonuBasligi).NotEmpty().WithMessage("Konu başlığı boş bırakılamaz.").MaximumLength(200);

        RuleFor(o => o.KaynakAdi).MaximumLength(200);

        RuleFor(o => o.Aciklama).MaximumLength(1000);

        RuleFor(o => o.SonTeslimTarihi)
            .GreaterThanOrEqualTo(o => o.VerilisTarihi)
            .WithMessage("Son teslim tarihi veriliş tarihinden önce olamaz.");

        RuleFor(o => o.ToplamSoruSayisi).GreaterThan(0).When(o => o.ToplamSoruSayisi.HasValue)
            .WithMessage("Toplam soru sayısı sıfırdan büyük olmalı.");

        // tamamlanan soru sayısı toplamı geçemez, geçerse yanlış bilgi kaydedilmiş olur
        RuleFor(o => o.TamamlananSoruSayisi)
            .LessThanOrEqualTo(o => o.ToplamSoruSayisi!.Value)
            .WithMessage("Tamamlanan soru sayısı toplam soru sayısını geçemez.")
            .When(o => o.TamamlananSoruSayisi.HasValue && o.ToplamSoruSayisi.HasValue);
    }
}
