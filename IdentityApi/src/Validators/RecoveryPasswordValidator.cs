using FluentValidation;
using IdentityApi.src.Queries;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityApi.src.Validators;

public class RecoveryPasswordValidator : AbstractValidator<RecoveryPasswordQuery>
{
    public RecoveryPasswordValidator()
    {
        RuleFor(x => x.Email).EmailAddress().WithMessage("Укажите валидный почтовый адрес");
        RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Требуется ввести пароль")
                                .MinimumLength(8).WithMessage("Длина пароля должна быть не меньше 8 символов")
                                .Matches("[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
                                .Matches("[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву")
                                .Matches("[0-9]").WithMessage("Пароль должен включать в себя хотя бы одну цифру")
                                .Matches("[^a-zA-Z0-9]").WithMessage("Пароль должен включать в себя специальный символ");
    }
}