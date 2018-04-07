﻿using System.Diagnostics.CodeAnalysis;

namespace InlineIL
{
    /// <summary>
    /// Represents a field reference, for use with <see cref="IL.Emit(OpCodes, FieldRef)" />.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public sealed class FieldRef
    {
        /// <summary>
        /// Constructs a field reference.
        /// </summary>
        /// <param name="type">The field type.</param>
        /// <param name="fieldName">The field name.</param>
        public FieldRef(TypeRef type, string fieldName)
            => IL.Throw();
    }
}
