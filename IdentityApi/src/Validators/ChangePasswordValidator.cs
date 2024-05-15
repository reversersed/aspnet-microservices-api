using FluentValidation;
using IdentityApi.src.Queries;

namespace IdentityApi.src.Validators;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordQuery>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Введите пароль");
        RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Требуется ввести пароль")
                                .MinimumLength(8).WithMessage("Длина пароля должна быть не меньше 8 символов")
                                .Matches("[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
                                .Matches("[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву")
                                .Matches("[0-9]").WithMessage("Пароль должен включать в себя хотя бы одну цифру")
                                .Matches("[^a-zA-Z0-9]").WithMessage("Пароль должен включать в себя специальный символ");
    }
}
