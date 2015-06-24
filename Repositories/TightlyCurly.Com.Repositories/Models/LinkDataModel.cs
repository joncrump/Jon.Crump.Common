using System;
using System.Data;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Model.Enums;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.Links)]
    public class LinkDataModel : ILink
    {
        private string _uri;

        [FieldMetadata(Columns.LinkKind, SqlDbType.Int, Parameters.LinkKind)]
        public LinkKind? LinkKind { get; set; }

        [FieldMetadata(Columns.Uri, SqlDbType.NVarChar, Parameters.Uri)]
        public Uri Uri
        {
            get
            {
                return new Uri(_uri);
            }
            set { _uri = value.ToString(); }
        }

        [FieldMetadata(Columns.Text, SqlDbType.NVarChar, Parameters.Text)]
        public string Text { get; set; }

        [FieldMetadata(Columns.LinkId, SqlDbType.UniqueIdentifier, Parameters.LinkId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }
    }
}
