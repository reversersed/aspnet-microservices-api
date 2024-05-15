using FluentValidation;
using IdentityApi.src.Queries;
using MediatR;

namespace IdentityApi.src.Validators;

public class ChangeUsernameValidator : AbstractValidator<ChangeUsernameQuery>
{
    public ChangeUsernameValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Требуется ввести имя пользователя")
            .MinimumLength(4).WithMessage("Минимальная длина имени пользователя - 4");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Требуется ввести пароль");
    }
}
