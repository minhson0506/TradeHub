using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
	public class Status
	{
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}

