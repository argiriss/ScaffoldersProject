using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models
{
    public class Settings
    {
        [Key]
        public int SettingsId { get; set; }
        public decimal AdminFee { get; set; }
        public decimal MemberFee { get; set; }
    }
}
