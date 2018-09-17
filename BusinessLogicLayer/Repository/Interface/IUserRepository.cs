using Entities;

namespace Repository
{
    public interface IUserRepository
    {
        ServiceRes IsUserValid(User objUser);
        ServiceRes RegisterUser(Registration objRegister);
        ServiceRes UnlockUserAccount(User objUser);
        ServiceRes ChangePassword(User changePassword);
        ServiceRes UserProfile();
    }
}
