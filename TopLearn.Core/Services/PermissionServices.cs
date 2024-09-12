using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Permission;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services
{
    public class PermissionServices : IPermissionServices
    {
        private TopLearnContext _context;

        public PermissionServices(TopLearnContext context)
        {
            _context = context;
        }

        #region Admin Panel

        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }
        public void AddRolesForUser(List<int> roleIds, int userId)
        {
            foreach (var roleId in roleIds)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId = roleId,
                    UserId = userId,
                });
            }

            _context.SaveChanges();
        }

        public void EditRolesForUser(List<int> roleIds, int userId)
        {
            // Delete Users Last Roles
            _context.UserRoles.Where(ur => ur.UserId == userId).ToList()
                .ForEach(r => _context.UserRoles.Remove(r));

            // Add New Roles For User
            AddRolesForUser(roleIds, userId);
        }

        public int AddRole(Role role)
        {
            _context.Add(role);
            _context.SaveChanges();
            return role.RoleId;
        }

        public Role GetRoleById(int id)
        {
            return _context.Roles.Find(id);
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        public void DeleteRole(int id)
        {
            _context.Roles.Single(r => r.RoleId == id).IsDelete = true;
            _context.SaveChanges();
        }
        #endregion

        #region Permission

        public List<Permission> GetAllPermissions()
        {
            return _context.Permission.ToList();
        }

        public void AddPermissionToRole(int roleId, List<int> permissions)
        {
            foreach (var permission in permissions)
            {
                _context.RolePermission.Add(new RolePermission()
                {
                    PermissionId = permission,
                    RoleId = roleId
                });
            }
            _context.SaveChanges();
        }

        public List<int> GetPermissionsRole(int roleId)
        {
            return _context.RolePermission
                .Where(pr => pr.RoleId == roleId)
                .Select(pr => pr.PermissionId).ToList();
        }

        public void UpdateRolePermissions(int roleId, List<int> permission)
        {
            // Delete Role Permissions
            _context.RolePermission.Where(rp => rp.RoleId == roleId)
                .ToList().ForEach(rp => _context.RolePermission.Remove(rp));

            // Add New Role Permissions
            AddPermissionToRole(roleId, permission);
        }

        public bool CheckPermission(int permissionId, string userName)
        {
            int UserId = _context.Users.SingleOrDefault(u => u.UserName == userName).UserId;

            var userRoles = GetUserRolesByUserId(UserId);
            if (!userRoles.Any())
                return false;

            var rolePermissions = GetRolePermissionsByPermissionId(permissionId);

            return rolePermissions.Any(rp => userRoles.Contains(rp));
        }

        public List<int> GetUserRolesByUserId(int userId)
        {
            return _context.UserRoles.Where(u => u.UserId == userId)
                .Select(u => u.RoleId).ToList();
        }

        public List<int> GetRolePermissionsByPermissionId(int permissionId)
        {
            return _context.RolePermission
                .Where(p => p.PermissionId == permissionId)
                .Select(p => p.RoleId).ToList();
        }

        public List<Role> GetDeletedRoles()
        {
            return _context.Roles.IgnoreQueryFilters().Where(r => r.IsDelete).ToList();
        }

        public void RecoveryRole(int id)
        {
            _context.Roles.IgnoreQueryFilters().Single(r => r.RoleId == id).IsDelete = false;
            _context.SaveChanges();
        }

        public Role GetDeletedRoleById(int id)
        {
            return GetDeletedRoles().Single(r => r.RoleId == id);
        }

        #endregion
    }
}
