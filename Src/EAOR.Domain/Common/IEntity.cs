﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAOR.Domain.Common
{
    public interface IEntity
    {
        Guid Id { get; }
    }

}
