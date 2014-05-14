using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using Quintsys.NUnit.Extensions.Annotations;

namespace Quintsys.NUnit.Extensions
{
    public static class GenericExtensions
    {
        /// <summary>
        ///     Verifies the presence of a specified attribute on the type under test.
        /// </summary>
        /// <typeparam name="T">The type under test</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="attributeTypes">The attribute(s) to verify.</param>
        [UsedImplicitly]
        public static void ShouldHave<T>(this T obj, params Type[] attributeTypes)
        {
            Type type = typeof (T);
            foreach (Type attributeType in attributeTypes)
            {
                object[] customAttributes = type.GetCustomAttributes(attributeType, false);
                Assert.IsTrue(customAttributes.Any(), attributeType.Name + " not found on " + type.Name);
            }
        }

        /// <summary>
        ///     Verifies the presence of a specified attribute on the method or member under test.
        /// </summary>
        /// <typeparam name="T">The type under test</typeparam>
        /// <typeparam name="TT">The specific method of the type under test</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="attributes">The attribute(s) to verify.</param>
        [UsedImplicitly]
        public static void ShouldHave<T, TT>(this T obj, Expression<Func<T, TT>> expression, params Type[] attributes)
        {
            ShouldHave(expression, null, attributes);
        }

        /// <summary>
        ///     Verifies the presence of a specified attribute that has a given argument value, on the method or member under test.
        /// </summary>
        /// <typeparam name="T">The type under test</typeparam>
        /// <typeparam name="TT">The specific method of the type under test</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="defaultArgumentValue">The value of the default argument of the attribute.</param>
        /// <param name="attributes">The attribute(s) to verify.</param>
        [UsedImplicitly]
        public static void ShouldHave<T, TT>(this T obj, Expression<Func<T, TT>> expression, object defaultArgumentValue,
            params Type[] attributes)
        {
            ShouldHave(expression, defaultArgumentValue, attributes);
        }

        #region private members

        private static void ShouldHave<T, TT>(Expression<Func<T, TT>> expression, object defaultArgumentValue,
            IEnumerable<Type> attributeTypes)
        {
            var methodCallExpression = expression.Body as MethodCallExpression;
            if (methodCallExpression != null)
            {
                AssertAttributePresent(methodCallExpression.Method, attributeTypes, defaultArgumentValue);
            }
            else
            {
                var memberExpression = expression.Body as MemberExpression;
                if (memberExpression == null)
                    throw new ArgumentException("The expression does not represent a Type Member or a Method Call.");

                AssertAttributePresent(memberExpression.Member, attributeTypes, defaultArgumentValue);
            }
        }

        private static void AssertAttributePresent(MemberInfo info, IEnumerable<Type> attributeTypes,
            object defaultArgumentValue = null)
        {
            foreach (Type attributeType in attributeTypes)
            {
                object[] customAttributes = info.GetCustomAttributes(attributeType, false);
                Assert.IsTrue(customAttributes.Any(), attributeType.Name + " not found on " + info.Name);

                if (customAttributes.Any() && defaultArgumentValue != null)
                {
                    ShouldHaveDefaultArgument(attributeType, customAttributes.First(), defaultArgumentValue);
                }
            }
        }

        private static void ShouldHaveDefaultArgument(Type attributeType, object attribute, object defaultArgumentValue)
        {
            if (attributeType == typeof (StringLengthAttribute) && defaultArgumentValue is int)
            {
                // Default Argument: MaximumLength
                Assert.AreEqual((int) defaultArgumentValue, ((StringLengthAttribute) attribute).MaximumLength);
            }

            if (attributeType == typeof (DisplayAttribute) && defaultArgumentValue is string)
            {
                // Default Argument: Name
                Assert.AreEqual(defaultArgumentValue.ToString(), ((DisplayAttribute) attribute).Name);
            }

            if (attributeType == typeof (RegularExpressionAttribute) && defaultArgumentValue != null)
            {
                // Default Argument: Pattern
                Assert.AreEqual(defaultArgumentValue.ToString(), ((RegularExpressionAttribute) attribute).Pattern);
            }

            if (attributeType == typeof (CompareAttribute) && defaultArgumentValue != null)
            {
                // Default Argument: OtherProperty
                Assert.AreEqual(defaultArgumentValue.ToString(), ((CompareAttribute) attribute).OtherProperty);
            }

            if (attributeType == typeof (DataTypeAttribute) && defaultArgumentValue != null)
            {
                // Default Argument: DataType
                Assert.AreEqual((DataType) defaultArgumentValue, ((DataTypeAttribute) attribute).DataType);
            }
        }

        #endregion
    }
}