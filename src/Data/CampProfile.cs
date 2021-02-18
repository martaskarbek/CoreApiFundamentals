using AutoMapper;
using CoreCodeCamp.Data.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            //This part from .ForMember is letting that we can change name to similar like property name in location 
            //and allows to be code in Camp model to be more understendable, and no requires naming by binding
            //with name prefixing. Somehow it helps to avoid exceptions
            this.CreateMap<Camp, CampModel>().ForMember(
                c=> c.Venue,
                o=> o.MapFrom
                    (m=> m.Location.VenueName)).ReverseMap();

            this.CreateMap<TalkModel, TalkModel>().ReverseMap().ForMember
                (t => t.Camp, opt => opt.Ignore())
                .ForMember(t => t.Speaker, opt => opt.Ignore());

            this.CreateMap<Speaker, SpeakerModel>().ReverseMap(); 
        }
    }
}