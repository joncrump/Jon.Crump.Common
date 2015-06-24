using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IPersonRelationship : IModel
    {
        Guid PrimaryId { get; set; }
        IPerson Primary { get; set; }
        IEnumerable<IPerson> Secondaries { get; set; } 
    }
}
