using FluentValidation;
using Mentoliz.Business.Dto.Hedef;

namespace Mentoliz.Business.Validation.Hedef;

public class HedefKaydetDTOValidator : AbstractValidator<HedefKaydetDTO>
{
    public HedefKaydetDTOValidator()
    {
        RuleFor(h => h.HedefUniversite).NotEmpty().WithMessage("Hedef üniversite boş bırakılamaz.").MaximumLength(200);

        RuleFor(h => h.HedefBolum).NotEmpty().WithMessage("Hedef bölüm boş bırakılamaz.").MaximumLength(200);

        RuleFor(h => h.HedefSiralama).GreaterThan(0).When(h => h.HedefSiralama.HasValue)
            .WithMessage("Hedef sıralama sıfırdan büyük olmalı.");

        RuleFor(h => h.HedefToplamNet).GreaterThanOrEqualTo(0).When(h => h.HedefToplamNet.HasValue)
            .WithMessage("Hedef toplam net negatif olamaz.");

        RuleForEach(h => h.DersNetleri).ChildRules(ders =>
        {
            ders.RuleFor(d => d.HedefNet).GreaterThanOrEqualTo(0).WithMessage("Ders hedef neti negatif olamaz.");
        });
    }
}
