using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TopLearn.DataLayer.Entities.Permission;

namespace TopLearn.DataLayer.Entities.User
{
    public class Role
    {
        public Role()
        {
            
        }

        [Key]
        public int RoleId { get; set; }
        [Display(Name = "نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        public string RoleTitle { get; set; }

        public bool IsDelete { get; set; }
        #region Relations

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }

        #endregion

    }
}
