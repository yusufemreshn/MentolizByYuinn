using FluentValidation;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Entities;

namespace Mentoliz.Business.Validation.Deneme;

public class DenemeSonucDetayKaydetDTOValidator : AbstractValidator<DenemeSonucDetayKaydetDTO>
{
    public DenemeSonucDetayKaydetDTOValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(d => d.Dogru).GreaterThanOrEqualTo(0).WithMessage("Doğru sayısı negatif olamaz.");

        RuleFor(d => d.Yanlis).GreaterThanOrEqualTo(0).WithMessage("Yanlış sayısı negatif olamaz.");

        RuleFor(d => d.Bos).GreaterThanOrEqualTo(0).WithMessage("Boş sayısı negatif olamaz.");

        // doğru yanlış boş toplamı testin soru sayısını geçerse yanlış veri girilmiş demektir
        RuleFor(d => d)
            .MustAsync(async (detay, iptalTokeni) =>
            {
                var testDepo = unitOfWork.RepositoryGetir<SinavTuruTest>();
                var test = await testDepo.TekGetirAsync(detay.SinavTuruTestId);
                return test is not null && detay.Dogru + detay.Yanlis + detay.Bos <= test.SoruSayisi;
            })
            .WithMessage("Doğru, yanlış ve boş sayılarının toplamı testin soru sayısını geçemez.");
    }
}
