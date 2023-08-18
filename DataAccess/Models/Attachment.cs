using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
	public class Attachment
	{
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string MediaLink { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [ForeignKey("Post")]
        public Guid PostId { get; set; }
    }
}

