using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.Utilities
{
    public static class ObjectUtilities
    {

        /// <summary>
        /// Checks if T and V are of the same type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public static bool IsTypeT_TypeV<T, V>()
        {
            return typeof(V).IsAssignableFrom(typeof(T));
        }

        /// <summary>
        /// Converts object to Type T.
        /// </summary>
        /// <typeparam name="F">From Type</typeparam>
        /// <typeparam name="T">To Type</typeparam>
        /// <param name="objectInstance"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">If unable to convert object type and exception will be thrown.</exception>
        public static T CastAs<F, T>(F objectInstance)
        {
            if (objectInstance is T)
            {
                return (T)Convert.ChangeType(objectInstance, typeof(T));
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        /// <summary>
        /// Get property value by property selection expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="objectInstance"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static V GetPropertyValue<T, V>(T objectInstance, Expression<Func<T, object>> predicate) 
        {
            string propertyName = GetPropertyName(predicate);
            return (V)objectInstance.GetType().GetProperty(propertyName).GetValue(objectInstance, null);
        }

        /// <summary>
        /// Get field value by property selection expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="objectInstance"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static V GetFieldValue<T, V>(T objectInstance, Expression<Func<T, object>> predicate)
        {
            string propertyName = GetPropertyName(predicate);
            return (V)objectInstance.GetType().GetField(propertyName).GetValue(objectInstance);
        }

        /// <summary>
        /// Gets a hash set list of properies of an object instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectInstance"></param>
        /// <returns></returns>
        public static HashSet<PropertyInfo> GetAllPropertyInfos<T>(T objectInstance)
        {
            if(objectInstance is null)
            {
                return [];
            }
            return objectInstance.GetType().GetProperties().ToHashSet();
        }

        /// <summary>
        /// Gets a hash set list of properies of an object instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectInstance"></param>
        /// <returns></returns>
        public static HashSet<string> GetAllPropertyNames<T>(T objectInstance)
        {
            HashSet<PropertyInfo> properties = GetAllPropertyInfos(objectInstance).ToHashSet();
            HashSet<string> names = new HashSet<string>();
            foreach (PropertyInfo propertyInfo in properties)
            {
                names.Add(propertyInfo.Name);
            }
            return names;
        }

        /// <summary>
        /// Gets the property name of an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> property)
        {
            if (property.Body is MemberExpression)
            {
                return ((MemberExpression)property.Body).Member.Name;
            }
            else
            {
                var op = ((UnaryExpression)property.Body).Operand;
                return ((MemberExpression)op).Member.Name;
            }
        }

        public static Expression<Func<T, object>> PropertyNameToExpression<T>(string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression property = Expression.Property(parameter, propertyName);
            Expression converted = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(converted, parameter);
        }

        /// <summary>
        /// Converts object instance of type F to object instance of type T. Note: the T class must contain a constructor that accepts an instance of object F.
        /// </summary>
        /// <typeparam name="F"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="DTOs"></param>
        /// <returns></returns>
        public static IEnumerable<T> ConvertToNewObject<F, T>(IEnumerable<F> DTOs)
        {
            foreach (F dto in DTOs)
            {
                var instance = Activator.CreateInstance(typeof(T), dto);
                if(instance is null)
                {
                    throw new InvalidOperationException("Unable to convert from object of type F to T.");
                }
                yield return (T)instance;
            }
        }

        /// <summary>
        /// Converts object instance of type F to object instance of type T. Note: the T class must contain a constructor that accepts an instance of object F.
        /// </summary>
        /// <typeparam name="F"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="DTOs"></param>
        /// <returns></returns>
        public static T ConvertToNewObject<F, T>(F dto)
        {
            return (T)Activator.CreateInstance(typeof(T), dto);
        }

        /// <summary>
        /// Determines if an object instance contains a method by name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        public static bool HasMethod<T>(T obj, string MethodName)
        {
            var type = obj.GetType();
            return type.GetMethod(MethodName) != null;
        }

        /// <summary>
        /// Determines if an object instance contains a property by name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="PropertyName"></param>
        /// <returns></returns>
        public static bool HasProperty<T>(T obj, string PropertyName)
        {
            var type = obj.GetType();
            return type.GetProperty(PropertyName) != null;
        }

        /// <summary>
        /// Determines if an object instance contains a field by name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public static bool HasField<T>(T obj, string FieldName)
        {
            var type = obj.GetType();
            return type.GetField(FieldName) != null;
        }


        /// <summary>
        /// Returns an object instance property/field value by name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Object instance</param>
        /// <param name="AttributeName">Name of prperty or field </param>
        /// <returns></returns>
        public static T GetPropertyOrFieldValue<T>(object obj, string AttributeName)
        {
            if(HasProperty(obj, AttributeName) == true)
            {
                return GetPropertyValue<T>(obj, AttributeName);
            }
            return GetFieldValue<T>(obj, AttributeName);
        }

        /// <summary>
        /// Gets the property value from an object by name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectedObject"></param>
        /// <param name="PropertyName"></param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(object selectedObject, string PropertyName)
        {
            return (T)selectedObject.GetType().GetProperty(PropertyName).GetValue(selectedObject, null);
        }

        /// <summary>
        /// Gets the field value from an object by name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectedObject"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public static T GetFieldValue<T>(object selectedObject, string FieldName)
        {
            return (T)selectedObject.GetType().GetField(FieldName).GetValue(selectedObject);
        }

        /// <summary>
        /// Determine if an object instance has a property of type Z.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Z"></typeparam>
        /// <param name="objectInstance"></param>
        /// <returns></returns>
        public static bool HasPropertyOfType<T, Z>(T objectInstance)
        {
            PropertyInfo[] propertyInfos = objectInstance.GetType().GetProperties();
            foreach(PropertyInfo propertyInfo in propertyInfos)
            {
                if(propertyInfo.PropertyType == typeof(Z))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
