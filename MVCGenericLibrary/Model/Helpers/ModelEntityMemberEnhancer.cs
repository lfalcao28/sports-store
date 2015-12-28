// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelEntityMemberEnhancer.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Static class with extension methods for IModelEntity interface;
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Model.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Entity;
    using Entity.Members;

    /// <summary>
    /// Sttaic class with extension methods for <see cref="IModelEntity"/> interface.
    /// </summary>
    public static class ModelEntityMemberEnhancer
    {
        public static string[] Names(this IList<ModelEntityMember> members)
        {
            return members.Select(mem => mem.Name).ToArray();
        }
    }
}