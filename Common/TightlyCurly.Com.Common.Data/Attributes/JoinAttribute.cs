using System;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JoinAttribute : Attribute
    {
        private string _parentProperty;
        private string _childProperty;

        public JoinAttribute(JoinType relationshipType, Type joinedType, string leftKey,
            string rightKey, string joinTable = null,
            string joinTableLeftKey = null, string joinTableRightKey = null,
            JoinType joinTableJoinType = JoinType.None,
            string parentProperty = null, string childProperty = null)
        {
            JoinType = Guard.EnsureIsValid("relationshipType", d => d != JoinType.None, relationshipType);
            JoinedType = Guard.EnsureIsNotNull("joinedType", joinedType);
            LeftKey = Guard.EnsureIsNotNullOrEmpty("leftKey", leftKey);
            RightKey = Guard.EnsureIsNotNullOrEmpty("rightKey", rightKey);
            JoinTable = joinTable;
            JoinTableLeftKey = joinTableLeftKey;
            JoinTableRightKey = joinTableRightKey;
            JoinTableJoinType = joinTableJoinType;
            ParentProperty = parentProperty;
            ChildProperty = childProperty;
        }

        public JoinType JoinType { get; private set; }
        public Type JoinedType { get; private set; }
        public string LeftKey { get; private set; }
        public string RightKey { get; private set; }
        public string JoinTable { get; private set; }
        public string JoinTableLeftKey { get; private set; }
        public string JoinTableRightKey { get; private set; }
        public JoinType JoinTableJoinType { get; private set; }

        public string ParentProperty
        {
            get
            {
                if (_parentProperty.IsNullOrEmpty())
                {
                    return LeftKey;
                }

                return _parentProperty;
            }
            private set
            {
                _parentProperty = value;
            }
        }

        public string ChildProperty
        {
            get
            {
                if (_childProperty.IsNullOrEmpty())
                {
                    return RightKey;
                }

                return _childProperty;
            }
            private set
            {
                _childProperty = value;
            }
        }
    }
}