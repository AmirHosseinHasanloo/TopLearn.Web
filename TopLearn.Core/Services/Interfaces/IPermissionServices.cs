using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Permission;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IPermissionServices
    {
        #region Roles
        List<Role> GetRoles();
        List<Role> GetDeletedRoles();
        int AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(int id);
        void RecoveryRole(int id);
        Role GetRoleById(int id);
        Role GetDeletedRoleById(int id);
        void AddRolesForUser(List<int> roleIds, int userId);
        void EditRolesForUser(List<int> roleIds, int userId);
        #endregion

        #region Permissions

        List<Permission> GetAllPermissions();

        void AddPermissionToRole(int roleId, List<int> permissions);

        List<int> GetPermissionsRole(int roleId);
        void UpdateRolePermissions(int roleId, List<int> permission);
        bool CheckPermission(int permissionId, string userName);
        List<int> GetUserRolesByUserId(int userId);
        List<int> GetRolePermissionsByPermissionId(int permissionId);

        #endregion
    }
}
