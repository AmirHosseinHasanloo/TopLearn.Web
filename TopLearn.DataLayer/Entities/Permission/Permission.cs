using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TopLearn.DataLayer.Entities.Permission
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }
        public string PermissionTitle { get; set; }
        public int? ParentId { get; set; }

        #region Relations

        [ForeignKey("ParentId")]
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }

        #endregion

    }
}
