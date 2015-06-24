using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data.Repositories.Strategies
{
    public class DictionaryBasedDataReaderJoinedBuilderStrategy : DataReaderBuilderStrategyBase, IBuilderStrategy
    {
        private static readonly MethodInfo WhereMethod;

        static DictionaryBasedDataReaderJoinedBuilderStrategy()
        {
            WhereMethod = GetWhereMethod();
        }

        public DictionaryBasedDataReaderJoinedBuilderStrategy(IDataReaderBuilder dataReaderBuilder) : base(dataReaderBuilder)
        {
        }

        public IEnumerable<TValue> BuildItems<TValue>(dynamic parameters, IDataReader dataSource) 
            where TValue : class, new()
        {
            Guard.EnsureIsNotNull("parameters", parameters);
            Guard.EnsureIsNotNull("dataSource", dataSource);

            IEnumerable<TableObjectMapping> tableObjectMappings = parameters.TableObjectMappings;
            IDictionary<object, TValue> parents = new Dictionary<object, TValue>();

            var type = typeof (TValue);

            var primaryKeyProperties = (type.GetProperties()
                .Select(
                    property =>
                        new
                        {
                            property,
                            fieldMetadataAttributes =
                                property.GetCustomAttributes(typeof (FieldMetadataAttribute), true)
                                    .SafeSelect(a => (FieldMetadataAttribute) a)
                        })
                .Where(@t => !@t.fieldMetadataAttributes.IsNullOrEmpty())
                .Where(@t => @t.fieldMetadataAttributes.Where(f => f.IsPrimaryKey).IsNotNullOrEmpty())
                .Select(@t => @t.property)).ToList();

            if (primaryKeyProperties.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Type {0} either has no FieldMetaDataAttributes or no Primary Key defined."
                    .FormatString(type.ToString()));
            }

            if (primaryKeyProperties.Count > 1)
            {
                throw new InvalidOperationException("Type {0} has too many primary keys defined."
                    .FormatString(type.ToString()));
            }

            while (dataSource.Read())
            {
                BuildParent(dataSource, parents, tableObjectMappings, primaryKeyProperties.First());
            }

            return parents.Values.ToList();
        }

        private void BuildParent<TParent>(IDataReader dataSource, IDictionary<object, TParent> parents, 
            IEnumerable<TableObjectMapping> tableObjectMappings, 
            PropertyInfo primaryKeyProperty)
            where TParent : class, new()
        {
            if (tableObjectMappings.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Cannot build objects.  No table mappings found.");
            }

            var parent = DataReaderBuilder.Build<TParent>(dataSource, tableObjectMappings.First().Prefix,
                false);

            var parentKey = primaryKeyProperty.GetValue(parent);

            if (!parents.ContainsKey(parentKey))
            {
                parents.Add(parentKey, parent);
            }

            BuildChildren(dataSource, tableObjectMappings.Skip(1), primaryKeyProperty, parents);
        }
        
        private void BuildChildren<TParent>(IDataReader dataSource, 
            IEnumerable<TableObjectMapping> tableObjectMappings, 
            PropertyInfo primaryKeyProperty,
            IDictionary<object, TParent> parents)
        {
            foreach (var tableObjectMapping in tableObjectMappings)
            {
                BuildAndLinkChild(dataSource, primaryKeyProperty, parents, tableObjectMapping);
            }
        }

        private void BuildAndLinkChild<TParent>(IDataReader dataSource, PropertyInfo primaryKeyProperty, IDictionary<object, TParent> parents,
            TableObjectMapping tableObjectMapping)
        {
            var childPropertyName = tableObjectMapping.ChildKey;
            var parentPropertyName = tableObjectMapping.ParentKey;
            var childType = tableObjectMapping.JoinType;
            var prefix = tableObjectMapping.Prefix;
            var parentInstancePropertyName = tableObjectMapping.InstancePropertyName;

            //VerifyParentPropertyName(primaryKeyProperty, parentPropertyName);

            var child = BuildConvertedChild(dataSource, childType, prefix);
            var childPropertyValue = GetChildPropertyValue(childType, childPropertyName, child);

            // TODO: Change to KeyNotFoundException as time permits.
            if (!parents.ContainsKey(childPropertyValue))
            {
                throw new InvalidOperationException("Could not locate parent key from child.");
            }

            var parent = parents[childPropertyValue];
            var parentInstanceProperty = GetParentInstanceProperty(parentInstancePropertyName, parent);

            LinkChildToParent(child, parent, parentInstanceProperty);

            parents[childPropertyValue] = parent;
        }

        private static void LinkChildToParent(object child, object parent, PropertyInfo parentProperty)
        {
            var parentPropertyType = parentProperty.PropertyType;
            var childType = child.GetType();
            var parentPropertyDescriptor = GetParentPropertyDescriptor(parentPropertyType, childType);

            switch (parentPropertyDescriptor)
            {
                case ParentPropertyDescriptor.Single:
                    LinkToSingleProperty(parent, child, parentProperty);
                    break;
                case ParentPropertyDescriptor.Enumerable:
                    LinkToEnumerableProperty(parent, child, parentProperty, childType);
                    break;
                default:
                    // TODO: Change to CouldNotLinkChildToParentException as time permits.
                    throw new InvalidOperationException("Could not link child to parent");
            }
        }

        private static void LinkToSingleProperty(object parent, object child, PropertyInfo parentProperty)
        {
            parentProperty.SetValue(parent, child);
        }

        private static void LinkToEnumerableProperty(object parent, object child, PropertyInfo parentProperty, 
            Type childType)
        {
            var enumerable = parentProperty.GetValue(parent);

            if (enumerable.IsNull())
            {
                var listType = typeof (List<>);
                var genericListType = listType.MakeGenericType(new[] {childType});

                enumerable = Activator.CreateInstance(genericListType);

                //enumerable = Convert.ChangeType(enumerable, genericListType);
            }

            var enumerableType = typeof (IEnumerable<>);
            var genericEnumerableType = enumerableType.MakeGenericType(new[] {childType});

            //var convertedEnumerable = Convert.ChangeType(enumerable, genericEnumerableType);

            if (!IsChildAdded(enumerable, //convertedEnumerable, 
                child, childType))
            {
                enumerable = AddChild(enumerable,//convertedEnumerable, 
                    child, childType);
            }

            parentProperty.SetValue(parent, enumerable);
        }

        private static object AddChild(object convertedEnumerable, object child, Type childType)
        {
            var concatMethod = typeof (Enumerable).GetMethod("Concat");
            var genericConcactMethod = concatMethod.MakeGenericMethod(new[] {childType});

            var listType = typeof(List<>);
            var genericListType = listType.MakeGenericType(new[] { childType });
            var listInstance = Activator.CreateInstance(genericListType);
            var addMethod = genericListType.GetMethod("Add");

            addMethod.Invoke(listInstance, new[] { child });

            convertedEnumerable = genericConcactMethod.Invoke(null, new[] {convertedEnumerable, listInstance});

            return convertedEnumerable;
        }

        private static bool IsChildAdded(object convertedEnumerable, object child, Type childType)
        {
            var childPrimaryKey = GetChildPrimaryKey(childType);
            var expression = BuildChildPrimaryKeySelector(childPrimaryKey, childType, child);
            var anyMethod = typeof (Enumerable).GetMethods().FirstOrDefault(m => m.Name == "Any" && m.GetParameters().Count() == 2);
            var genericAnyMethod = anyMethod.MakeGenericMethod(new[] {childType});
            var isChildAdded = (bool) genericAnyMethod.Invoke(null, new object[] {convertedEnumerable, expression});

            return isChildAdded;
        }

        private static object BuildChildPrimaryKeySelector(PropertyInfo childPrimaryKey, Type childType, object child)
        {
            var parameter = Expression.Parameter(childType, "c");
            var propertyValue = childPrimaryKey.GetValue(child);
            var member = Expression.Property(parameter, childPrimaryKey);
            var constant = Expression.Constant(propertyValue);
            var equalExpression = Expression.Equal(member, constant);
            var expressionType = typeof (Expression);
            var funcType = typeof (Func<,>);
            var genericFuncType = funcType.MakeGenericType(new[] {childType, typeof (bool)});
            var lambdaMethod = expressionType.GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m => m.Name == "Lambda");
            var genericLambdaMethod = lambdaMethod.MakeGenericMethod(genericFuncType);

            var lambdaExpression = 
                genericLambdaMethod.Invoke(null, new object[] {equalExpression, new[] {parameter}}) as Expression;

            var compiledMethod = lambdaExpression.GetType().GetMethods().FirstOrDefault(m => m.Name == "Compile");
            var predicate = compiledMethod.Invoke(lambdaExpression, null);

            return predicate;
        }

        private static PropertyInfo GetChildPrimaryKey(Type childType)
        {
            return (childType.GetProperties()
                .Select(
                    property =>
                        new
                        {
                            property,
                            attributes =
                                property.GetCustomAttributes()
                                    .Where(a => a.GetType() == typeof (FieldMetadataAttribute))
                                    .SafeSelect(a => (FieldMetadataAttribute) a)
                        })
                .Where(@t => !@t.attributes.IsNullOrEmpty() && !@t.attributes.Where(a => a.IsPrimaryKey).IsNullOrEmpty())
                .Select(@t => @t.property)).FirstOrDefault();
        }

        private static MethodInfo GetWhereMethod()
        {
            // The Where method lives on the Enumerable type in System.Linq
            var whereMethods = typeof(System.Linq.Enumerable)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(mi => mi.Name == "Where");

            return whereMethods
                .FirstOrDefault(w => w.GetParameters().Count() == 2);
        }

        private static ParentPropertyDescriptor GetParentPropertyDescriptor(Type parentPropertyType, Type childType)
        {
            if (parentPropertyType == childType)
            {
                return ParentPropertyDescriptor.Single;
            }

            foreach (var typeInterface in parentPropertyType.GetInterfaces())
            {
                if (IsEnumerable(typeInterface, childType))
                {
                    return ParentPropertyDescriptor.Enumerable;
                }

                if (typeInterface.IsInterface && !typeInterface.IsGenericType && childType.IsAssignableFrom(typeInterface))
                {
                    return ParentPropertyDescriptor.Single;
                }
            }

            return ParentPropertyDescriptor.Unknown;
        }

        private static bool IsEnumerable(Type typeInterface, Type childType)
        {
            if (typeInterface.IsGenericType
                    && typeInterface.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var implementType = typeInterface.GetGenericArguments().FirstOrDefault();

                if (implementType.IsInterface &&
                    childType.GetInterfaces().SafeWhere(i => i == implementType).IsNullOrEmpty()
                    || (implementType == childType))
                {
                    return true;
                }
            }

            if (typeInterface == typeof(IEnumerable))
            {
                return true;
            }

            return false;
        }

        private static PropertyInfo GetParentInstanceProperty<TParent>(string parentInstancePropertyName, TParent parent)
        {
            var parentInstanceProperty = typeof (TParent).GetProperty(parentInstancePropertyName);

            return parentInstanceProperty;
        }

        private static object GetChildPropertyValue(Type childType, string childPropertyName, object convertedChild)
        {
            var childProperty = childType.GetProperty(childPropertyName);

            // TODO: Change to PropertyNotFoundException as time permits.
            if (childProperty.IsNull())
            {
                throw new InvalidOperationException("Could not locate property {0} on child type {1}."
                    .FormatString(childPropertyName, childType.ToString()));
            }

            var childPropertyValue = childProperty.GetValue(convertedChild);
            return childPropertyValue;
        }

        private object BuildConvertedChild(IDataReader dataSource, Type childType, string prefix)
        {
            var builderMethod = DataReaderBuilder.GetType().GetMethod("Build");
            var genericBuilderMethod = builderMethod.MakeGenericMethod(new[] {childType});

            var child = genericBuilderMethod.Invoke(DataReaderBuilder, new object[]
            {
                dataSource, prefix, false
            });

            var convertedChild = Convert.ChangeType(child, childType);
            return convertedChild;
        }

        private enum ParentPropertyDescriptor
        {
            Unknown,
            Enumerable, 
            Single
        }
    }
}