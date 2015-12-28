// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DynamicObjectExpression.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Defines the DynamicObjectExpression type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.GenericModel.DynamicObject
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using DownloadServer.GenericModel.DynamicObject;

    /// <summary>
    /// Instances of this class are objects with dynamic behaviour 
    /// accessing public data members os an underlying static type instance. It is constructed with an 
    /// instance of type <typeparamref name="T"/> and acces to it's public properties or fields
    /// can be made by name through an indexer. When an access is made to an
    /// undefined member in the underlying object an <see cref="ArgumentException"/>
    /// </summary>
    /// <typeparam name="T">The underlying dynamic object static type.
    /// </typeparam>
    public class DynamicObjectExpression<T> : DynamicObjectReflection<T> {
        /// <summary>
        /// The dictionary cache of the already accessed members
        /// </summary>
        private readonly Dictionary<string, Func<T, object>> _memberAccesFuncs = new Dictionary<string, Func<T, object>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicObjectExpression{T}"/> class. 
        /// </summary>
        /// <param name="staticTypeInstance">
        /// The static type underlying instance.
        /// </param>
        public DynamicObjectExpression(T staticTypeInstance) : base(staticTypeInstance)
        {
        }

        /// <summary>
        /// Gets the value of the field represeted by <paramref name="fi"/> for the underlying <see cref="DynamicObjectReflection{T}._staticTypeInstance"/>.
        /// </summary>
        /// <param name="fi">The <see cref="FieldInfo"/> representing the field to get the value from.</param>
        /// <returns>The value of the field</returns>
        protected override object GetValue(FieldInfo fi)
        {
            return GetValue(fi.Name);
        }

        /// <summary>
        /// Gets the value of the property represeted by <paramref name="pi"/> for the underlying entity.
        /// </summary>
        /// <param name="pi">The <see cref="FieldInfo"/> representing the property to get the value from.</param>
        /// <returns>The value of the field</returns>
        protected override object GetValue(PropertyInfo pi) {
            return GetValue(pi.Name);
        }

        /// <summary>
        /// Gets the value of the underlying memmber (field or property) using <see cref="Expression{TDelegate}"/>.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <returns>The member value</returns>
        private object GetValue(string memberName)
        {
            Func<T, object> accessorFunc;
            if (_memberAccesFuncs.ContainsKey(memberName) == false) 
            {
                ParameterExpression prm = Expression.Parameter(typeof (T), "x");
                Expression<Func<T, object>> accessor = Expression.Lambda<Func<T, object>>(
                    Expression.Convert(Expression.PropertyOrField(prm, memberName), typeof (object)), prm);
                accessorFunc = accessor.Compile();
            }
            else {
                accessorFunc = _memberAccesFuncs[memberName];
            }

            object o = accessorFunc(StaticTypeInstance);
            return o;
        }
    }
}