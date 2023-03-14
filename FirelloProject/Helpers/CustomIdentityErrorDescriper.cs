using Microsoft.AspNetCore.Identity;

namespace FirelloProject.Helpers
{
    public class CustomIdentityErrorDescriper : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $" `{userName}` artiq istifadeci movcuddur"

            };

        }
    }
}
