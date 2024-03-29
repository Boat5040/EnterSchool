﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnterSchoolApp.Models
{
    [Table("Institution")]
    public class Institution: ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InstitutionId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }


        [Required]
        public string Subdomain { get; set; }

        [Column(TypeName = "image")]
        public byte[] IntitutionLogo { get; set; }


        [Required]
        public string Email { get; set; }

        [Column(TypeName = "xml")]
        public string SecurityPolicy { get; set; }


        public virtual ICollection<Branch> Branches { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

    }
}