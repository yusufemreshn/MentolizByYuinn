using FluentValidation;
using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Business.Validation.Ogrenci;

public class OgrenciKaydetDTOValidator : AbstractValidator<OgrenciKaydetDTO>
{
    private const string TelefonDeseni = @"^\+?[0-9\s()-]{7,20}$";

    public OgrenciKaydetDTOValidator()
    {
        RuleFor(o => o.Ad).NotEmpty().WithMessage("Öğrencinin adı boş bırakılamaz.").MaximumLength(100);

        RuleFor(o => o.Soyad).NotEmpty().WithMessage("Öğrencinin soyadı boş bırakılamaz.").MaximumLength(100);

        // telefon zorunlu değil ama doluysa formatı kontrol ediliyor
        RuleFor(o => o.Telefon)
            .Matches(TelefonDeseni).WithMessage("Telefon numarası sadece rakam, boşluk, parantez ve tire içerebilir.")
            .When(o => !string.IsNullOrWhiteSpace(o.Telefon));

        RuleFor(o => o.OgrenciNo).MaximumLength(30);

        RuleFor(o => o.OkulAdi).MaximumLength(200);

        RuleFor(o => o.GenelNotlar).MaximumLength(4000);

        RuleFor(o => o.KayitTarihi).NotEmpty().WithMessage("Kayıt tarihi boş bırakılamaz.");

        // doğum tarihi bugünden ileride olamaz
        RuleFor(o => o.DogumTarihi)
            .LessThan(DateTime.Today).WithMessage("Doğum tarihi bugünden ileride olamaz.")
            .When(o => o.DogumTarihi.HasValue);

        RuleForEach(o => o.Veliler).SetValidator(new VeliKaydetDTOValidator());
    }
}
