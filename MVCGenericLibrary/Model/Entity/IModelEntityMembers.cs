// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IModelEntityMembers.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Interface to implement by class that returns the &lt;see cref="ModelEntityMemberSingleValue" /&gt; for a given &lt;typeparamref name="TEntity" /&gt;.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MVCGenericLibrary.Model.Entity
{
    using System.Collections.Generic;
    using Members;

    /// <summary>
    /// Interface to implement by class that returns the 
    /// <see cref="ModelEntityMemberSingleValue"/> for a given <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity that iplementers returs it's viwable/editable <see cref="ModelEntityMemberSingleValue"/>.</typeparam>
    public interface IModelEntityMembers<TEntity>
    {
        /// <summary>
        /// Gets the list of <see cref="ModelEntityMember"/> that represent the visible and 
        /// updatable members for <typeparam name="TEntity"/>.
        /// </summary>
        IList<ModelEntityMember> Members { get; }
    }
}