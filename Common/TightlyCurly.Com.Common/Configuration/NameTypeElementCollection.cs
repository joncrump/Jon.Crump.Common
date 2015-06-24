using System.Configuration;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Configuration
{
    public class NameTypeElementCollection : ConfigurationElementCollection
    {
        public NameTypeElement this[int index]
        {
            get { return (NameTypeElement)BaseGet(index); }
            set
            {
                if (BaseGet(index).IsNotNull())
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        public void Add(NameTypeElement element)
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
            return new NameTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var parserElement = (NameTypeElement)element;

            return parserElement.ProcessorType;
        }
    }
}
