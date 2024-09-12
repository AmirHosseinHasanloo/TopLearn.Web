using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.Convertors;
using TopLearn.DataLayer.Entities.User;
using TopLearn.Core.Generators;
using TopLearn.Core.Security;
using TopLearn.DataLayer.Entities.Wallet;
using TopLearn.Core.DTOs;

namespace TopLearn.Core.Services
{
    public class UserService : IUserService
    {
        private TopLearnContext _context;

        public UserService(TopLearnContext context)
        {
            _context = context;
        }

        public bool ActiveAccount(string activecode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activecode);

            if (user == null || user.IsActive)
            {
                return false;
            }

            user.IsActive = true;
            user.ActiveCode = NameGenerator.GenerateName();
            _context.SaveChanges();
            return true;
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public User GetUserByActiveCode(string activecode)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == activecode);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == FixedText.FixedEmail(email));
        }

        public User GetUserByUserId(int userId)
        {
            return _context.Users.Find(userId);
        }

        public Wallet GetWalletByWalletId(int walletid)
        {
            return _context.Wallets.Find(walletid);
        }

        public bool IsExistEmail(string email) =>
        _context.Users.Any(u => u.Email == FixedText.FixedEmail(email));


        public bool IsExistUserName(string username) =>
            _context.Users.Any(u => u.UserName == username);

        public bool IsUserBuyedThisCourse(int courseId, string userName)
        {
            int userId = _context.Users.Single(u => u.UserName == userName).UserId;

            return _context.UserCourse.Any(d => d.UserId == userId && d.CourseId == courseId);
        }

        public User LoginUser(LoginViewModel login)
        {
            string hashPassword = PasswordHelper.EncodePasswordMD5(login.Password);
            string email = FixedText.FixedEmail(login.Email);

            return _context.Users.SingleOrDefault(u => u.Email == email && u.Password == hashPassword);
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }

    }
}
