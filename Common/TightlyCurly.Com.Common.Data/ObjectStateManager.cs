using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Common.Data
{
    public class ObjectStateManager : IStateManager
    {
        private readonly IHashHelper _hashHelper;
        private readonly IStateStore _stateStore;

        protected IDictionary<string, InitialObjectState> InitialObjectStates; 

        public ObjectStateManager(IHashHelper hashHelper, IStateStore stateStore)
        {
            _hashHelper = Guard.EnsureIsNotNull("hashHelper", hashHelper);

            //InitialObjectStates = new ConcurrentDictionary<string, InitialObjectState>();
        }

        public void SetState(object value)
        {
            Guard.EnsureIsNotNull("value", value);

            var type = value.GetType();
            var objectState = ExtractStateInfo(value, type);

            if (InitialObjectStates.ContainsKey(objectState.Id))
            {
                InitialObjectStates[objectState.Id] = objectState;
            }
            else
            {
                InitialObjectStates.Add(objectState.Id, objectState);
            }

            UpdateChildStates(objectState);
        }

        public ObjectStateInfo GetObjectState(object value)
        {
            Guard.EnsureIsNotNull("value", value);

            var type = value.GetType();
            var state = ExtractStateInfo(value, type);

            var stateInfo = new ObjectStateInfo
            {
                ObjectType = type
            };

            AssignObjectState(value, state, stateInfo, false);
            AssignChildObjectStates(value, state, stateInfo.ChildInfos);

            return stateInfo;
        }

        public void RemoveState(object value)
        {
            throw new NotImplementedException();
        }

        private void AssignChildObjectStates(object parent, InitialObjectState state, IList<ObjectStateInfo> childStateInfos)
        {
            if (childStateInfos.IsNull())
            {
                childStateInfos = new List<ObjectStateInfo>();
            }

            foreach (var childState in state.ChildObjectStates)
            {
                var childStateInfo = new ObjectStateInfo
                {
                    ObjectType = childState.ObjectType,
                    PropertyName = childState.PropertyName
                };

                AssignObjectState(parent, childState, childStateInfo, true);
                AssignChildState(childState, childStateInfo);

                childStateInfos.Add(childStateInfo);
            }
        }

        private void AssignObjectState(object item, InitialObjectState state, ObjectStateInfo stateInfo, bool isChild)
        {
            var value = GetValue(item, state, isChild);

            if (!InitialObjectStates.ContainsKey(state.Id))
            {
                stateInfo.ObjectState = ObjectState.New;
                return;
            }

            var stateAsParent = InitialObjectStates[state.Id];
            var stateAsChild = isChild ? GetChildInitialState(item, value) : null;

            if (stateAsParent.IsDeleted || (isChild && stateAsChild.IsNotNull() && stateAsChild.IsDeleted))
            {
                stateInfo.ObjectState = ObjectState.Deleted;
                return;
            }

            var hash = _hashHelper.GenerateHash(value);

            if (String.Compare(stateAsParent.HashCode, hash, StringComparison.InvariantCulture) != 0)
            {
                stateInfo.ObjectState = ObjectState.Updated;
                return;
            }

            stateInfo.ObjectState = ObjectState.NoChange;
            return;
        }

        private InitialObjectState GetChildInitialState(object parent, object child)
        {
            // TODO: Optimize since GetId uses reflection and it is expensive.
            var parentId = GetId(parent, parent.GetType());
            var childId = GetId(child, child.GetType());

            var childState = InitialObjectStates[parentId].ChildObjectStates.FirstOrDefault(c => c.Id == childId);

            return childState;
        }

        private object GetValue(object item, InitialObjectState state, bool isChild)
        {
            if (!isChild)
            {
                return item;
            }

            var itemType = item.GetType();
            var property = itemType.GetProperties().First(p => p.Name == state.PropertyName);
            var value = property.GetValue(item);
            var expectedId = state.Id;

            if (!IsEnumerable(property.PropertyType))
            {
                var valueId = GetId(value, property.PropertyType);

                if (expectedId != valueId)
                {
                    // TODO: Convert to ChildIdMismatchException.
                    throw new InvalidOperationException("Child Id does not match.  Expected {0}.  Actual {1}"
                        .FormatString(expectedId, valueId));
                }

                return value;
            }

            foreach (var enumeratedItem in (IEnumerable) value)
            {
                var itemId = GetId(enumeratedItem, enumeratedItem.GetType());

                if (expectedId == itemId)
                {
                    return enumeratedItem;
                }
            }

            // TODO: Convert to ChildNotFoundException.
            throw new InvalidOperationException("Could not locate child object.");
        }

        private void AssignChildState(InitialObjectState childObjectState, ObjectStateInfo childStateInfo)
        {
            var foundAsParent = InitialObjectStates.ContainsKey(childObjectState.Id);
            var attachedToParent = false;

            if (InitialObjectStates.ContainsKey(childObjectState.ParentId))
            {
                var parentState = InitialObjectStates[childObjectState.ParentId];

                attachedToParent = parentState.ChildObjectStates
                    .Any(child => child.Id == childObjectState.Id);
            }

            var childState = AssignChildStateInternal(foundAsParent, attachedToParent);

            childStateInfo.ChildState = childState;
        }

        private ChildState AssignChildStateInternal(bool foundAsParent, bool attachedToParent)
        {
            if (!foundAsParent && !attachedToParent)
            {
                return ChildState.NotTracked;
            }

            if (foundAsParent && !attachedToParent)
            {
                return ChildState.Detached;
            }

            if (foundAsParent && attachedToParent)
            {
                return ChildState.Attached;
            }

            if (!foundAsParent && attachedToParent)
            {
                return ChildState.AttachedToParentAsChildNoParentExists;
            }

            return ChildState.Unknown;
        }

        private void UpdateChildStates(InitialObjectState objectState)
        {
            foreach (var state in InitialObjectStates.Values
                .SafeWhere(i => i.ChildObjectStates.IsNotNullOrEmpty()))
            {
                for (var index = 0; index < state.ChildObjectStates.Count; index++)
                {
                    var childState = state.ChildObjectStates[index];

                    UpdateChildState(objectState, childState, state, index);
                }

                InitialObjectStates[state.Id] = state;
            }
        }

        private static void UpdateChildState(InitialObjectState objectState, InitialObjectState childState,
            InitialObjectState state, int index)
        {
            if (childState.Id != objectState.Id || childState.ObjectType != objectState.ObjectType)
            {
                return;
            }

            childState.HashCode = objectState.HashCode;
            state.ChildObjectStates[index] = childState;
        }

        private InitialObjectState ExtractStateInfo(object value, Type type)
        {
            var id = GetId(value, type);
            var hash = _hashHelper.GenerateHash(value);
            var objectState = new InitialObjectState
            {
                Id = id,
                ObjectType = type,
                HashCode = hash,
            };

            var joinProperties = type
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(JoinAttribute), true)
                    .IsNotNullOrEmpty())
                .ToList();

            foreach (var property in joinProperties)
            {
                var propertyValue = property.GetValue(value);
                var propertyType = property.PropertyType;
                Type valueType = null;
                var isEnumerable = IsEnumerable(propertyType);

                if (propertyValue.IsNotNull())
                {
                    if (isEnumerable)
                    {
                        valueType = propertyType.GetGenericArguments().First();
                        var objectValues = (IEnumerable) propertyValue;

                        foreach (var objectValue in objectValues)
                        {
                            var state = ExtractStateInfo(objectValue, valueType);

                            state.PropertyName = property.Name;
                            state.ParentId = id;
                            objectState.ChildObjectStates.Add(state);
                        }
                    }
                    else
                    {
                        valueType = propertyType;
                        var state = ExtractStateInfo(propertyValue, valueType);
                        state.ParentId = id;
                        objectState.ChildObjectStates.Add(state);
                    }
                }
            }

            return objectState;
        }

        private static bool IsEnumerable(Type propertyType)
        {
            return propertyType.IsGenericType && propertyType.GetInterface("IEnumerable").IsNotNull();
        }

        private static string GetId(object value, Type type)
        {
            var primaryKeyProperty = type
                .GetProperties()
                .FirstOrDefault(
                    p => p.GetCustomAttributes(typeof (FieldMetadataAttribute), true)
                        .Where(a => ((FieldMetadataAttribute) a).IsPrimaryKey)
                        .IsNotNullOrEmpty());

            if (primaryKeyProperty.IsNull())
            {
                throw new InvalidOperationException("Cannot set state.  Object has no primary key defined");
            }

            var id = primaryKeyProperty.GetValue(value).ToString();
            return id;
        }
    }
}