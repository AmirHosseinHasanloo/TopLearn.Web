using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.Core.DTOs;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IUserService
    {
        bool IsExistUserName(string username);
        bool IsExistEmail(string email);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        User GetUserByEmail(string email);
        User GetUserByUserId(int userId);
        User GetUserByActiveCode(string activecode);
        void UpdateUser(User user);
        bool ActiveAccount(string activecode);
        Wallet GetWalletByWalletId(int walletid);
        void UpdateWallet(Wallet wallet);
        bool IsUserBuyedThisCourse(int courseId,string userName);

        
    }
}
