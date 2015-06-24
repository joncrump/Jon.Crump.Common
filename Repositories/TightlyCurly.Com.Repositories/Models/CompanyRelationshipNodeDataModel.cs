using System;
using System.Collections.Generic;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.CompanyRelationships)]
    public class CompanyRelationshipNodeDataModel : ValueFactoryModelBase, ICompanyRelationshipNode
    {
        private IEnumerable<ICompanyRelationshipNode> _parents;
        private IEnumerable<ICompanyRelationshipNode> _children;

        [FieldMetadata(Columns.RelationshipNodeId, SqlDbType.UniqueIdentifier, Parameters.RelationshipNodeId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.SmallDateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.SmallDateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Columns.StartDate, SqlDbType.SmallDateTime, Parameters.StartDate, allowDbNull: true)]
        public DateTime? StartDate { get; set; }

        [FieldMetadata(Columns.EndDate, SqlDbType.SmallDateTime, Parameters.EndDate, allowDbNull: true)]
        public DateTime? EndDate { get; set; }

        [FieldMetadata(Columns.Name, SqlDbType.NVarChar, Parameters.Name, allowDbNull: true)]
        public string Name { get; set; }

        [FieldMetadata(Columns.Notes, SqlDbType.NVarChar, Parameters.Notes, allowDbNull: true)]
        public string Notes { get; set; }

        [FieldMetadata(Columns.RelationshipType, SqlDbType.Int, Parameters.RelationshipType)]
        public RelationshipType RelationshipType { get; set; }
        
        [ValueFactory(LoaderKeys.GetParentsByRelationshipNode)]
        public IEnumerable<ICompanyRelationshipNode> Parents
        {
            get
            {
                if (_parents.IsNull())
                {
                    _parents = GetOrLoadLazyValue(_parents, LoaderKeys.GetParentsByRelationshipNode);
                }

                return _parents;
            }
            set
            {
                _parents = value;
            }
        }

        [ValueFactory(LoaderKeys.GetChildrenByRelationshipNode)]
        public IEnumerable<ICompanyRelationshipNode> Children
        {
            get
            {
                if (_children.IsNull())
                {
                    _children = GetOrLoadLazyValue(_children, LoaderKeys.GetChildrenByRelationshipNode);
                }

                return _children;
            }
            set
            {
                _children = value;
            }
        }
    }
}
