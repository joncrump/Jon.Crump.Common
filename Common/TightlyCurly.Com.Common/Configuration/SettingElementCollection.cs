using System.Configuration;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Configuration
{
    public class SettingElementCollection : ConfigurationElementCollection
    {
        public SettingElement this[int index]
        {
            get { return (SettingElement)BaseGet(index); }
            set
            {
                if (BaseGet(index).IsNotNull())
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        public void Add(SettingElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SettingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var parserElement = (SettingElement)element;

            return parserElement.Value;
        }
    }
}
