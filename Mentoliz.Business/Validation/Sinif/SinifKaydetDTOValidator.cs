using FluentValidation;
using Mentoliz.Business.Dto.Sinif;

namespace Mentoliz.Business.Validation.Sinif;

public class SinifKaydetDTOValidator : AbstractValidator<SinifKaydetDTO>
{
    public SinifKaydetDTOValidator()
    {
        RuleFor(s => s.Ad).NotEmpty().WithMessage("Sınıf adı boş bırakılamaz.").MaximumLength(50);

        RuleFor(s => s.Kontenjan).GreaterThan(0).WithMessage("Kontenjan sıfırdan büyük olmalı.");
    }
}
