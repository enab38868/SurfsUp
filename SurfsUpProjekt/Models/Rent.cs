using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SurfsUpProjekt.Models
{
    public class Rent
    {
        [Key]
        [ForeignKey("Board")]
        public int BoardId { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartRent { get; set; } = DateTime.Now;
        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndRent { get; set; } = DateTime.Now.AddDays(7);
       
        //public virtual Board Board { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
    }
}
