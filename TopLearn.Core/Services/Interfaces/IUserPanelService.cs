using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using TopLearn.Core.DTOs;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IUserPanelService
    {
        #region Get
        UserInformationViewModel GetUserInformation(string username);
        UserInformationViewModel GetBannedUserInformation(string username);
        UserInformationViewModel GetBannedUserInformation(int userId);
        UserInformationViewModel GetUserInformation(int userId);
        SideBarUserPanelViewModel GetSidebarUserPanel(string username);
        User GetUserByName(string username);
        User GetBannedUserByName(string username);
        EditProfileViewModel GetDataForUserProfileEdit(string username);
        int GetUserIdByUserName(string username);
        User GetBannedUserById(int  userId);

        #endregion

        #region Compare
        bool CompareOldPassword(string username, string oldpassword);

        #endregion

        #region Change or Update & Edit
        void EditProfile(string username, EditProfileViewModel profile);
        void ChangeUserPassword(string username, string newpassword);
        #endregion

        #region Wallet

        int BalanceUserWallet(string username);
        List<WalletViewModel> GetWalletBalanceHistoryOfUser(string username);
        int ChargeWallet(string username, int amount, string description, bool IsPay = false);
        int AddWallet(Wallet wallet);
        int BalanceBannedUserWallet(string username);

        #endregion

        #region Admin Panel
        UsersForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "", string userName = "");
        UsersForAdminViewModel GetBannedUsers(int pageId = 1, string filterEmail = "", string userName = "");
        int AddUserFromAdminPanel(CreateUserViewModel create);
        EditUserViewModel GetUserForShowInEditMode(int userId);
        void EditUserFromAdminPanel(EditUserViewModel editUser);
        string AddUserImage(IFormFile avatar);
        string AvatarNameGeneratorWithFilePath(IFormFile avatar);
        void BanUser(int userId);
        void RecoveryUser(int userId);

        #endregion
    }
}
