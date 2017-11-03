using ShareTube.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShareTube.Data.Mappings
{
    internal class VideoMap : EntityMap<Video>
    {
        public override void Map(EntityTypeBuilder<Video> e)
        {
            e.HasKey(nameof(Video.ID), nameof(Video.RoomID), nameof(Video.Order));
        }
    }
}
