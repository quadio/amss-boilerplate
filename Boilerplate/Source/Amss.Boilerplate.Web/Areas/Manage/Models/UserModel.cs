namespace Amss.Boilerplate.Web.Areas.Manage.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using Amss.Boilerplate.Data;

    using Foolproof;

    public class UserModel
    {
        [HiddenInput]
        public long? UserId { get; set; }

        [Required]
        [StringLength(MetadataInfo.StringNormal)]
        public string Name { get; set; }

        [Required]
        [StringLength(MetadataInfo.StringNormal)]
        public string Login { get; set; }

        [RequiredIf("UserId", Operator.EqualTo, null, ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(MetadataInfo.StringNormal)]
        public string Email { get; set; }

        [Display(Name = "Role")]
        public long RoleId { get; set; }

        [Display(Name = "Partner")]
        public long? PartnerId { get; set; }

        public Dictionary<long, string> AvailablePartners { get; set; }

        public Dictionary<long, string> AvailableRoles { get; set; }
    }
}