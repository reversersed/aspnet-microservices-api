using FluentValidation;
using IdentityApi.src.Queries;

namespace IdentityApi.src.Validators;

public class RefreshQueryValidation : AbstractValidator<RefreshQuery>
{
    public RefreshQueryValidation()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Требуется refresh token");
        RuleFor(x => x.Token).NotEmpty().WithMessage("Требуется указать токен");
    }
}
