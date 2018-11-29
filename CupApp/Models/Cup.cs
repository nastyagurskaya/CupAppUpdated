using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace CupApp.Models
{
    public enum CupType
    {
        tea, coffee
    }
    [Table("Cup")]
    public class Cup
    {
        [Key]
        public int CupId { get; set; }
        [Required]
        [Range(0.1, 2.0)]
        public double Capacity { get; set; }
        public CupType? CupType { get; set; }
        public int CountryID { get; set; }
        public virtual CupImage CupImage { get; set; }
        public virtual Country Country { get; set; }
        //[NotMapped]
        //public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile ImageUpload { get; set; }
        public Cup()
        {
            string path = Directory.GetCurrentDirectory();
            //ImagePath = path+"/AppFiles/Images/default.png";
        }
        //public Cup(double capacity, CupType? cupType, int countryID, IFormFile imageUpload)
        //{
        //    Capacity = capacity;
        //    CupType = cupType;
        //    CountryID = countryID;
        //    ImageUpload = imageUpload;
        //}
    }
}