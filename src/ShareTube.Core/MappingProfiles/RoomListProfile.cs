using AutoMapper;
using ShareTube.Core.Models;
using ShareTube.Core.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareTube.Core.Mappers
{
    public class RoomListProfile : Profile
    {
        public RoomListProfile()
        {
            CreateMap<Room, RoomListItemView>()
                .ForMember(x => x.UserCount, o => o.MapFrom(v => v.UserConnections.Count()))
                ;
        }
    }
}
