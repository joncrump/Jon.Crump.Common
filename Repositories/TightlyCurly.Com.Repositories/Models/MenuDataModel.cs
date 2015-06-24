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
    [Table(Tables.Menus)]
    public class MenuDataModel : ValueFactoryModelBase, IMenu
    {
        private IContentItem _contentItem;
        private IEnumerable<IMenuItem> _menuItems;
        private IEnumerable<IMenuText> _menuTexts;

        [FieldMetadata(Columns.MenuId, SqlDbType.UniqueIdentifier, Parameters.MenuId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        public IContentItem ContentItem
        {
            get
            {
                if (_contentItem.IsNull())
                {
                    _contentItem = GetOrLoadLazyValue(_contentItem, LoaderKeys.MenuContentItem);
                }

                return _contentItem;
            }
            set { _contentItem = value; }
        }
        
        [FieldMetadata(Columns.Name, SqlDbType.NVarChar, Parameters.Name)]
        public string Name { get; set; }
        
        public IEnumerable<IMenuItem> MenuItems
        {
            get
            {
                if (_menuItems.IsNull())
                {
                    _menuItems = GetOrLoadLazyValue(_menuItems, LoaderKeys.MenuMenuItems);
                }

                return _menuItems;
            }
            set { _menuItems = value; }
        }

        public IEnumerable<IMenuText> MenuTexts
        {
            get
            {
                if (_menuTexts.IsNull())
                {
                    _menuTexts = GetOrLoadLazyValue(_menuTexts, LoaderKeys.MenuMenuTexts);
                }

                return _menuTexts;
            }
            set { _menuTexts = value; }
        }
    }
}
