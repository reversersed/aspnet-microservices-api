using FluentValidation;
using IdentityApi.src.Queries.Internal;

namespace IdentityApi.src.Validators.Internal;

public class FindUserQueryValidation : AbstractValidator<FindUserQuery>
{
    public FindUserQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty().When(x => string.IsNullOrWhiteSpace(x.UserName)).WithMessage("Требуется указать идентификатор или имя пользователя");
        RuleFor(x => x.UserName).NotEmpty().When(x => x.Id == 0).WithMessage("Требуется указать идентификатор или имя пользователя");
    }
}
