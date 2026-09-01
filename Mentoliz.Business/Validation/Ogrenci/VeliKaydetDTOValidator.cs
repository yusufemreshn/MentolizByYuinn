using FluentValidation;
using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Business.Validation.Ogrenci;

public class VeliKaydetDTOValidator : AbstractValidator<VeliKaydetDTO>
{
    // telefon alanına serbest metin girilmesin diye rakam, boşluk, parantez, artı ve tire dışında karakter kabul etmiyoruz
    private const string TelefonDeseni = @"^\+?[0-9\s()-]{7,20}$";

    public VeliKaydetDTOValidator()
    {
        RuleFor(v => v.Ad).NotEmpty().WithMessage("Velinin adı boş bırakılamaz.").MaximumLength(100);

        RuleFor(v => v.Soyad).NotEmpty().WithMessage("Velinin soyadı boş bırakılamaz.").MaximumLength(100);

        RuleFor(v => v.Telefon)
            .NotEmpty().WithMessage("Veli telefon numarası boş bırakılamaz.")
            .Matches(TelefonDeseni).WithMessage("Telefon numarası sadece rakam, boşluk, parantez ve tire içerebilir.");

        RuleFor(v => v.Eposta)
            .EmailAddress().WithMessage("Eposta adresi geçerli bir formatta olmalı.")
            .When(v => !string.IsNullOrWhiteSpace(v.Eposta));

        RuleFor(v => v.Meslek).MaximumLength(100);
    }
}
