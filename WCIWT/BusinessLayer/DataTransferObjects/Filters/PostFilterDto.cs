﻿using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects.Filters
{
    public class PostFilterDto : FilterDtoBase
    {
        public PostVisibility Visibility { get; set; } = PostVisibility.Public;
        public Gender GenderRestriction { get; set; } = Gender.NoInformation;

        public bool HasAgeRestriction { get; set; } = false;
        public int AgeRestrictionFrom { get; set; }
        public int AgeRestrictionTo { get; set; }

        public Guid UserId { get; set; }
    }
}
