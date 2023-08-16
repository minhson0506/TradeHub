using System;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
	public class Comment
	{
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public Guid PostId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

    }
}

