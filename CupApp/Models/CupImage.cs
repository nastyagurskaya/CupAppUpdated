using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CupApp.Models
{
    [Table("CupImage")]
    public class CupImage
    {
        [Key]
        [ForeignKey("Cup")]
        public int CupImageID { get; set; }
        public byte[] Image { get; set; }
        public virtual Cup Cup { get; set; }

        //[DisplayName("Image")]
        //public string ImagePath { get; set; }

        //public CupImage()
        //{
        //    ImagePath = "~/AppFiles/Images/default.png";
        //}
    }
}