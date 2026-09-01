using FluentValidation;
using Mentoliz.Business.Dto.Not;

namespace Mentoliz.Business.Validation.Not;

public class NotKaydetDTOValidator : AbstractValidator<NotKaydetDTO>
{
    public NotKaydetDTOValidator()
    {
        RuleFor(n => n.Baslik).NotEmpty().WithMessage("Not başlığı boş bırakılamaz.").MaximumLength(200);

        RuleFor(n => n.Icerik).NotEmpty().WithMessage("Not içeriği boş bırakılamaz.").MaximumLength(4000);
    }
}
