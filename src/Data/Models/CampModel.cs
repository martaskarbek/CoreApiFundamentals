using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreCodeCamp.Data.Models
{
    public class CampModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Moniker { get; set; }
        public DateTime EventDate { get; set; } = DateTime.MinValue;
        [Range(1,100)]
        public int Length { get; set; } = 1;
        
        //those are properties from Location objects
        //and he is binding it by adding Location what is Entity(class) name
        //in front of each property
        public string Venue { get; set; }
        public string LocationAddress1 { get; set; }
        public string LocationAddress2 { get; set; }
        public string LocationAddress3 { get; set; }
        public string LocationCityTown { get; set; }
        public string LocationStateProvince { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationCountry { get; set; }
        
        public ICollection<TalkModel> Talks { get; set; }
    }
}