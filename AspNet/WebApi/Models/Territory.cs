namespace WebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Territory
    {
        public Territory()
        {
            Employees = new HashSet<Employee>();
        }

        [Key, Required]
        [StringLength(20)]
        public string TerritoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string TerritoryDescription { get; set; }

        public int RegionID { get; set; }

        public Region Region { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
