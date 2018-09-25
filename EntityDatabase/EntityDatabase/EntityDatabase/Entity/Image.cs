﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabase
{
    public class Image
    {
        [Key]
        public uint Id { get; set; }
        
        public uint PostId { get; set; }
        public Post Post { get; set; }

        public byte[] BinaryImage { get; set; }
        public uint Likes { get; set; }
        public uint Dislikes { get; set; }

        public List<Vote> Votes { get; set; }
    }
}
