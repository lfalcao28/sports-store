// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDynamicObject.cs" company="CCISEL">
//   Luís Falcão - 2009
// </copyright>
// <summary>
//   The DynamicObjectReflection interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DownloadServer.GenericModel.DynamicObject
{
    using System;

    /// <summary>
    /// Instances of this interface are objects with dynamic behavior accessing public data members 
    /// os an underlying static type instance. It is constructed with an 
    /// instance of type <typeparamref name="T"/> and acces to it's public properties or fields
    /// can be made by name through an indexer. When an access is made to an
    /// undefined member in the underlying object an <see cref="ArgumentException"/>
    /// </summary>
    /// <typeparam name="T">The underlying dynamic object static type.
    /// </typeparam>
    public interface IDynamicObject<T>
    {
        /// <summary>
        /// Gets or sets the undelying <typeparam name="T"/> instance.
        /// </summary>
        T StaticTypeInstance { get; set;  }

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
        object this[string memberName]
        {
            get; set;
        }
    }
}