// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DynamicObjectReflection.cs" company="CCISEL">
//   Luis Falcão 2009
// </copyright>
// <summary>
//   Defines the DynamicObjectReflection type. 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DownloadServer.GenericModel.DynamicObject {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// Instances of this class are objects with dynamic behaviour 
    /// accessing public data members os an underlying static type instance. It is constructed with an 
    /// instance of type <typeparamref name="T"/> and acces to it's public properties or fields
    /// can be made by name through an indexer. When an access is made to an
    /// undefined member in the underlying object an <see cref="ArgumentException"/>
    /// </summary>
    /// <typeparam name="T">The underlying dynamic object static type.
    /// </typeparam>
    public class DynamicObjectReflection<T> : IDynamicObject<T> {
        /// <summary>
        /// The underlying instance <see cref="Type"/> object.
        /// </summary>
        private readonly Type _staticTypeType;

        /// <summary>
        /// The dictionary cache of the already accessed members
        /// </summary>
        private readonly Dictionary<string, MemberInfo> _memberInfos;

        /// <summary>
        /// The underliying sttaic type instance.
        /// </summary>
        private T _staticTypeInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicObjectReflection{T}"/> class.
        /// </summary>
        /// <param name="staticTypeInstance">
        /// The static type underlying instance.
        /// </param>
        public DynamicObjectReflection(T staticTypeInstance) {
            _staticTypeInstance = staticTypeInstance;
            _staticTypeType = typeof(T);
            _memberInfos = new Dictionary<string, MemberInfo>();
        }

        // public this

        /// <summary>
        /// Gets or sets the undelying <typeparam name="T"/> instance.
        /// </summary>
        public T StaticTypeInstance {
            get { return _staticTypeInstance; }
            set { _staticTypeInstance = value; }
        }

        /// <summary>
        /// Dynamic access to the <paramref name="memberName"/> public data member os the underlying
        /// <typeparamref name="T"/> instance.
        /// </summary>
        /// <param name="memberName">
        /// The member name.
        /// </param>
        /// <exception cref="ArgumentException">
        /// This exception is thrown when the member does not exist or does not have public accesibility.
        /// </exception>
        public object this[string memberName] {
            get {
                MemberInfo member = GetMemberInfo(memberName);
                if (member as PropertyInfo != null) {
                    PropertyInfo pi = member as PropertyInfo;
                    MethodInfo[] mi = pi.GetAccessors();
                    if (mi != null) {
                        if (mi.Length == 0 || (mi.Length == 1 && mi[0].ReturnType == typeof(void))) {
                            throw new ArgumentException(
                                String.Format("No public get accessor set method for property with name '{0}'", memberName));
                        }
                    }

                    return GetValue(pi);
                }

                FieldInfo fi = member as FieldInfo;
                return GetValue(fi);
            }

            set {
                MemberInfo member = GetMemberInfo(memberName);
                if (member as PropertyInfo != null) {
                    PropertyInfo pi = member as PropertyInfo;
                    MethodInfo[] mi = pi.GetAccessors();
                    if (mi.Length == 0 || (mi.Length == 1 && mi[0].ReturnType != typeof(void))) {
                        throw new ArgumentException(String.Format("No public set accessor set method for property with name '{0}'", memberName));
                    }

                    pi.SetValue(_staticTypeInstance, value, null);
                }
                else 
                {
                    FieldInfo fi = member as FieldInfo;
                    fi.SetValue(_staticTypeInstance, value);
                }
            }
        }

        /// <summary>
        /// Gets the value of the field represeted by <paramref name="fi"/> for the underlying <see cref="_staticTypeInstance"/>.
        /// </summary>
        /// <param name="fi">The <see cref="FieldInfo"/> representing the field to get the value from.</param>
        /// <returns>The value of the field</returns>
        protected virtual object GetValue(FieldInfo fi)
        {
            return fi.GetValue(_staticTypeInstance);
        }

        /// <summary>
        /// Gets the value of the property represeted by <paramref name="pi"/> for the underlying <see cref="_staticTypeInstance"/>.
        /// </summary>
        /// <param name="pi">The <see cref="FieldInfo"/> representing the property to get the value from.</param>
        /// <returns>The value of the field</returns>
        protected virtual object GetValue(PropertyInfo pi) {
            return pi.GetValue(_staticTypeInstance, null);
        }

        /// <summary>
        /// Gets the <see cref="MemberInfo"/> with the specified <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName">
        /// The member name.
        /// </param>
        /// <returns>
        /// The MemberInfo for the given <paramref name="memberName"/>
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the member name is not found for
        /// the specified usage (get ou set)</exception>
        private MemberInfo GetMemberInfo(string memberName) {
            MemberInfo member;
            if (_memberInfos.ContainsKey(memberName)) {
                member = _memberInfos[memberName];
            }
            else 
            {
                _memberInfos[memberName] = null;
                MemberInfo[] members = _staticTypeType.GetMember(
                    memberName, BindingFlags.Instance | BindingFlags.Public);
                Debug.Assert(members.Length < 2, "There are more than two public members with the same name");
                if (members.Length == 0) {
                    throw new ArgumentException(String.Format("No field or property with name '{0}'", memberName));
                }

                member = members[0];
                _memberInfos[memberName] = member;
            }

            return member;
        }
    }
}