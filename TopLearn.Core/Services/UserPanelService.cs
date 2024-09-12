using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.DTOs;
using TopLearn.Core.Generators;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Services
{
    public class UserPanelService : IUserPanelService
    {
        #region Dependency Injection

        private TopLearnContext _context;
        private IUserService _userService;

        public UserPanelService(TopLearnContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        #endregion

        #region Get
        public EditProfileViewModel GetDataForUserProfileEdit(string username)
        {
            return _context.Users.Where(u => u.UserName == username).Select(u => new EditProfileViewModel()
            {
                UserName = u.UserName,
                Email = u.Email,
                AvatarName = u.UserAvatar
            }).Single();
        }

        public SideBarUserPanelViewModel GetSidebarUserPanel(string username)
        {
            User user = GetUserByName(username);

            SideBarUserPanelViewModel sideBar = new SideBarUserPanelViewModel();

            sideBar.UserName = user.UserName;
            sideBar.RegisterDate = user.RegisterDate;
            sideBar.UserAvatar = user.UserAvatar;

            return sideBar;
        }

        public User GetUserByName(string username)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == username);
        }

        public UserInformationViewModel GetUserInformation(string username)
        {
            User user = GetUserByName(username);

            UserInformationViewModel info = new UserInformationViewModel();


            info.UserName = user.UserName;
            info.Email = user.Email;
            info.RegisterDate = user.RegisterDate;
            info.Wallet = BalanceUserWallet(username);

            return info;
        }

        public int GetUserIdByUserName(string username)
        {
            return _context.Users.Single(u => u.UserName == username).UserId;
        }

        #endregion

        #region change Update & Edit
        public void ChangeUserPassword(string username, string newpassword)
        {
            var user = GetUserByName(username);

            if (user != null)
            {
                user.Password = PasswordHelper.EncodePasswordMD5(newpassword);
                _userService.UpdateUser(user);
            }
        }

        public void EditProfile(string username, EditProfileViewModel profile)
        {
            if (profile.Avatar != null)
            {
                string ImagePath;
                if (profile.AvatarName != "Defualt.png")
                {
                    ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);

                    if (File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                }

                profile.AvatarName = NameGenerator.GenerateName() + Path.GetExtension(profile.Avatar.FileName);
                ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    profile.Avatar.CopyTo(stream);
                }
            }

            var user = GetUserByName(username);
            user.UserName = profile.UserName;
            user.Email = profile.Email;
            user.UserAvatar = profile.AvatarName;

            // Update Current User
            _userService.UpdateUser(user);
        }

        public UserInformationViewModel GetUserInformation(int userId)
        {
            User user = _userService.GetUserByUserId(userId);

            UserInformationViewModel info = new UserInformationViewModel();


            info.UserName = user.UserName;
            info.Email = user.Email;
            info.RegisterDate = user.RegisterDate;
            info.Wallet = BalanceUserWallet(user.UserName);

            return info;
        }
        #endregion

        #region Compare
        public bool CompareOldPassword(string username, string oldpassword)
        {
            return _context.Users.Any(u =>
                u.UserName == username && u.Password == PasswordHelper.EncodePasswordMD5(oldpassword));
        }
        #endregion

        #region Wallet

        public int BalanceBannedUserWallet(string username)
        {
            int userId = GetBannedUserByName(username).UserId;

            var deposit = _context.Wallets.IgnoreQueryFilters()
                .Where(w => w.UserId == userId && w.TypeId == 1 && w.IsPayed)
                .Select(w => w.Amount);

            var harvest = _context.Wallets.IgnoreQueryFilters()
                .Where(w => w.UserId == userId && w.TypeId == 2)
                .Select(w => w.Amount);

            return (deposit.Sum() - harvest.Sum());
        }

        public int BalanceUserWallet(string username)
        {
            int UserId = GetUserIdByUserName(username);

            // Variz
            var Deposit = _context.Wallets.Where
                    (w => w.UserId == UserId && w.TypeId == 1
                    && w.IsPayed)
                   .Select(w => w.Amount);

            // Bardasht
            var Harvest = _context.Wallets.Where
                (w => w.UserId == UserId && w.TypeId == 2)
                .Select(w => w.Amount);

            return (Deposit.Sum() - Harvest.Sum());
        }

        public List<WalletViewModel> GetWalletBalanceHistoryOfUser(string username)
        {
            int UserId = GetUserIdByUserName(username);

            // return the list of User Balance History
            return _context.Wallets.Where(w => w.UserId == UserId && w.IsPayed).Select(w => new WalletViewModel()
            {
                Amount = w.Amount,
                CreateDate = w.CreateDate,
                Description = w.Description,
                Type = w.TypeId
            }).ToList();
        }

        public int ChargeWallet(string username, int amount, string description, bool IsPay = false)
        {
            int UserId = GetUserIdByUserName(username);
            Wallet wallet = new Wallet()
            {
                Amount = amount,
                IsPayed = IsPay,
                TypeId = 1,
                UserId = UserId,
                CreateDate = DateTime.Now,
                Description = description,
            };
            return AddWallet(wallet);
        }

        public int AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();

            return wallet.WalletId;
        }


        #endregion


        #region Admin Panel
        public UsersForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "", string userName = "")
        {
            IQueryable<User> result = _context.Users;

            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }

            if (!string.IsNullOrEmpty(userName))
            {
                result = result.Where(u => u.UserName.Contains(userName));
            }


            // Show In Page
            int take = 24;
            int skip = (pageId - 1) * take;

            UsersForAdminViewModel UserForAdmin = new UsersForAdminViewModel();

            UserForAdmin.CurrentPage = pageId;
            UserForAdmin.PageCount = result.Count() / take;
            UserForAdmin.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();


            return UserForAdmin;
        }

        public int AddUserFromAdminPanel(CreateUserViewModel create)
        {
            User AddUser = new User();
            AddUser.Email = create.Email;
            AddUser.UserName = create.UserName;
            AddUser.Password = PasswordHelper.EncodePasswordMD5(create.Password);
            AddUser.ActiveCode = NameGenerator.GenerateName();
            AddUser.IsActive = true;
            AddUser.RegisterDate = DateTime.Now;

            #region Save User Avatar

            if (create.UserAvatar != null)
            {
                AddUser.UserAvatar = AddUserImage(create.UserAvatar);
            }

            #endregion

            return _userService.AddUser(AddUser);
        }

        public EditUserViewModel GetUserForShowInEditMode(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId)
                .Select(u => new EditUserViewModel()
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                    AvatarName = u.UserAvatar,
                    UserRoles = u.UserRoles.Select(u => u.RoleId).ToList(),
                    Password = u.Password,
                }).Single();
        }

        public void EditUserFromAdminPanel(EditUserViewModel editUser)
        {
            User user = _userService.GetUserByUserId(editUser.UserId);

            user.Email = editUser.Email;

            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMD5(editUser.Password);
            }

            if (editUser.UserAvatar != null)
            {
                // Delete User Last Profile Avatar =>

                if (editUser.AvatarName != "Defualt.png" || editUser.AvatarName != null)
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editUser.AvatarName);

                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }
                }

                // Add New Profile Avatar

                user.UserAvatar = AddUserImage(editUser.UserAvatar);

            }

            _context.Update(user);
            _context.SaveChanges();
        }

        public string AddUserImage(IFormFile avatar)
        {
            string avatarName = AvatarNameGeneratorWithFilePath(avatar);
            string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", avatarName);

            using (var stream = new FileStream(ImagePath, FileMode.Create))
            {
                avatar.CopyTo(stream);
            }

            return avatarName;
        }

        public string AvatarNameGeneratorWithFilePath(IFormFile avatar)
        {
            return NameGenerator.GenerateName() + Path.GetExtension(avatar.FileName);
        }

        public UsersForAdminViewModel GetBannedUsers(int pageId = 1, string filterEmail = "", string userName = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(u => u.IsBanned);

            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }

            if (!string.IsNullOrEmpty(userName))
            {
                result = result.Where(u => u.UserName.Contains(userName));
            }

            // Show In Page
            int take = 24;
            int skip = (pageId - 1) * take;

            UsersForAdminViewModel UserForAdmin = new UsersForAdminViewModel();

            UserForAdmin.CurrentPage = pageId;
            UserForAdmin.PageCount = result.Count() / take;
            UserForAdmin.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();

            return UserForAdmin;
        }

        public void BanUser(int userId)
        {
            User user = _userService.GetUserByUserId(userId);
            user.IsBanned = true;
            _userService.UpdateUser(user);
        }

        public UserInformationViewModel GetBannedUserInformation(string username)
        {
            var user = GetBannedUserByName(username);

            UserInformationViewModel userInfo = new UserInformationViewModel();

            userInfo.Email = user.Email;
            userInfo.RegisterDate = user.RegisterDate;
            userInfo.UserName = user.UserName;
            userInfo.Wallet = BalanceBannedUserWallet(username);

            return userInfo;
        }

        public UserInformationViewModel GetBannedUserInformation(int userId)
        {
            var user = GetBannedUserById(userId);

            UserInformationViewModel userInfo = new UserInformationViewModel();

            userInfo.Email = user.Email;
            userInfo.RegisterDate = user.RegisterDate;
            userInfo.UserName = user.UserName;
            userInfo.Wallet = BalanceBannedUserWallet(user.UserName);

            return userInfo;
        }

        public void RecoveryUser(int userId)
        {
            _context.Users.IgnoreQueryFilters()
                .Single(u => u.UserId == userId).IsBanned = false;

            _context.SaveChanges();
        }

        public User GetBannedUserByName(string username)
        {
            return _context.Users.IgnoreQueryFilters().Single(u => u.UserName == username);
        }

        public User GetBannedUserById(int userId)
        {
            return _context.Users.IgnoreQueryFilters()
                .Single(u => u.UserId == userId);
        }

        #endregion
    }
}
