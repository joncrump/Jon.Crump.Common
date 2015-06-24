using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface ICompanyRelationshipNode : IModel
    {
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }
        string Name { get; set; }
        string Notes { get; set; }
        RelationshipType RelationshipType { get; set; }
        IEnumerable<ICompanyRelationshipNode> Parents { get; set; } 
        IEnumerable<ICompanyRelationshipNode> Children { get; set; } 
    }
}
