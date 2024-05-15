using FluentValidation;
using IdentityApi.src.Queries;

namespace IdentityApi.src.Validators;

public class LoginQueryValidation : AbstractValidator<LoginQuery>
{
    public LoginQueryValidation()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Требуется ввести имя пользователя")
                            .MinimumLength(4).WithMessage("Минимальная длина имени пользователя - 4");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Требуется ввести пароль");
    }
}
