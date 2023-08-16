using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace DataAccess.Models
{
    public class Post
    {

        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Range(0.01, 10000)]
        public double Price { get; set; }
        [Required]
        public DateTime CreatedDateTime { get; set; }
        [Required]
        public DateTime ModifiedDateTime { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        public Guid StatusId { get; set; }

        /// <summary>
        /// These are the two other objects I'm relating, linking the PK to the FK
        /// </summary>
        [ForeignKey("AuthorId")]
        public ApplicationUser? PostAuthor { get; set; }
        [ForeignKey("StatusId")]
        public Status? Status { get; set; }

    }
}

