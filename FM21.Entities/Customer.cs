using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{

    [Table("Customer")]
    public class Customer
    {
        public Customer()
        {
            FormulaMaster = new HashSet<FormulaMaster>();
            ProjectMaster = new HashSet<ProjectMaster>();
        }

        [Key]
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerAbbreviation1 { get; set; }
        public string CustomerAbbreviation2 { get; set; }
        public bool?  IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<FormulaMaster> FormulaMaster { get; set; }
        public virtual ICollection<ProjectMaster> ProjectMaster { get; set; }
    }
}
