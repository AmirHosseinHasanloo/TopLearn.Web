using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;

namespace TopLearn.Core.Security
{
    public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private int _permissionId;
        private TopLearnContext _context;

        public PermissionCheckerAttribute(int permissionId)
        {
            _permissionId = permissionId;
        }


        public bool CheckPermission(int permissionId, string userName)
        {
            int UserId = _context.Users.SingleOrDefault(u => u.UserName == userName).UserId;

            var userRoles = _context.UserRoles.Where(u => u.UserId == UserId)
                .Select(r => r.RoleId).ToList();

            if (!userRoles.Any())
                return false;

            var rolePermissions = _context.RolePermission.Where(p => p.PermissionId == permissionId)
                .Select(p => p.RoleId).ToList();

            return rolePermissions.Any(rp => userRoles.Contains(rp));
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                _context =
                    (TopLearnContext)context.HttpContext.RequestServices.GetService(typeof(TopLearnContext));

                string userName = context.HttpContext.User.Identity.Name;

                if (!CheckPermission(_permissionId, userName))
                {
                    context.Result = new RedirectResult("/Login");
                }
            }
            else
            {
                context.Result = new RedirectResult("/Login");
            }
        }
    }
}
