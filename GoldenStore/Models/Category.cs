using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenStore.Models
{
    public class Category
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Parent Category")]
        public int? ParentId { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("ParentId")]
        public virtual Category Parent { get; set; }

        public Category()
        {
            ParentId = 0;
        }
    }
}
