using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
	public class Tag
	{
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(256)]
        public string TagName { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }
}

