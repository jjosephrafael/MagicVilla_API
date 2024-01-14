using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_VillaAPI.Models
{
    public class VillaNumber
    {
        // Villa Number is a primary key but not an identity key
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }
        // this is an id to the villa table
        [ForeignKey("Villa")]
        public int VillaID { get; set; }
        public Villa Villa { get; set; }
        // Foreign Key End

        public string SpecialDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
