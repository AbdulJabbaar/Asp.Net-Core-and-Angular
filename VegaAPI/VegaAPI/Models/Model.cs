using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VegaAPI.Models
{
    [Table("Models")]
    public class Model
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [ForeignKey("MakeId")]
        public int MakeId { get; set; }
        public Make Make { get; set; }
    }
}
