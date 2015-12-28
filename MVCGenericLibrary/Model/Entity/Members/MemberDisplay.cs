// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberDisplay.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Enum that indicates the members mode an visibility in each kind of View
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Model.Entity.Members
{
    using System;

    /// <summary>
    /// Enum that indicates the members mode an visibility in each kind of View
    /// </summary>
    [Flags]
    public enum MemberDisplay
    {
        /// <summary>
        /// Displays the member in Details View
        /// </summary>
        Details = 1,

        /// <summary>
        /// Displays the member in Index View
        /// </summary>
        Index = 2,

        /// <summary>
        /// Displays the member in edit view, in editable mode
        /// </summary>
        Edit = 4,

        /// <summary>
        /// Displays the Member in readonly modes in editable views (create and edit)
        /// </summary>
        Readonly = 8,

        /// <summary>
        /// Displays the Member in create mode, in editable mode
        /// </summary>
        Create = 16
    }
}

