//using ASI.Contracts.Tailor;
//using AutoMapper;
//using ShareTube.Core.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace ShareTube.Core.Mappers
//{
//    public class ModelProfile : Profile
//    {
//        public ModelProfile()
//        {
//            CreateMap<Model, ModelView>()
//                .AfterMap((m, v) =>
//                {
//                    v.Fields = v.Fields?.OrderBy(x => x.Sequence).ToList();
//                    var counter = 0;
//                    foreach (var field in v.Fields.OrEmptyIfNull())
//                    {
//                        field.Sequence = counter++;
//                    }
//                })
//                ;
//            CreateMap<ModelView, Model>()
//                .ForMember(x => x.Id, opt => opt.Ignore()) // don't allow mapping the Id
//                .ForMember(x => x.CreateDate, opt => opt.Ignore()) // managed by the api
//                .ForMember(x => x.CreatedBy, opt => opt.Ignore()) //  managed by the api
//                .AfterMap((v, m) =>
//                {
//                    var counter = 0;
//                    foreach (var field in m.Fields.OrEmptyIfNull())
//                    {
//                        field.Sequence = counter++;
//                    }
//                })
//                ;
//        }

//    }
//}
