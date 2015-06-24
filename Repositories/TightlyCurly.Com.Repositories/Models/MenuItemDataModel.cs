using System;
using System.Collections.Generic;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Model.Enums;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.MenuItems)]
    public class MenuItemDataModel : ValueFactoryModelBase, IMenuItem
    {
        private string _uri;
        private IEnumerable<IMenuItemText> _menuItemTexts;
        private IEnumerable<IMenuItem> _children;

        [FieldMetadata(Columns.MenuItemId, SqlDbType.UniqueIdentifier, Parameters.MenuItemId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Parameters.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Parameters.LinkKind, SqlDbType.Int, Parameters.LinkKind)]
        public LinkKind? LinkKind { get; set; }

        [FieldMetadata(Parameters.Uri, SqlDbType.NVarChar, Parameters.Uri)]
        public Uri Uri
        {
            get { return new Uri(_uri); }
            set { _uri = value.ToString(); }
        }

        public IEnumerable<IMenuItemText> MenuItemTexts
        {
            get
            {
                if (_menuItemTexts.IsNull())
                {
                    _menuItemTexts = GetOrLoadLazyValue(_menuItemTexts, LoaderKeys.MenuItemMenuItemTexts);
                }

                return _menuItemTexts;
            }
            set { _menuItemTexts = value; }
        }

        [FieldMetadata(Columns.Order, SqlDbType.Int, Parameters.Order)]
        public int? Order { get; set; }

        public IEnumerable<IMenuItem> Children
        {
            get
            {
                if (_children.IsNull())
                {
                    _children = GetOrLoadLazyValue(_children, LoaderKeys.MenuItemChildren);
                }

                return _children;
            }
            set { _children = value; }
        }

        [FieldMetadata(Columns.MenuId, SqlDbType.UniqueIdentifier, Parameters.MenuId)]
        public Guid MenuId { get; set; }
    }
}
