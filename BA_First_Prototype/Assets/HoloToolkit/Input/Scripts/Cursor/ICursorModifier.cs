// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace HoloToolkit.Unity.InputModule
{
    /// <summary>
    /// Cursor Modifier Interface that provides basic overrides for Cursor behaviour.
    /// </summary>
    public interface ICursorModifier
    {
        /// <summary>
        /// Indicates whether the Cursor should be visible or not.
        /// </summary>
        /// <returns>True if Cursor should be visible, false if not.</returns>
        bool GetCursorVisibility();

        /// <summary>
        /// Returns the Cursor position after considering this modifier.
        /// </summary>
        /// <param name="cursor">Cursor that is being modified.</param>
        /// <returns>New position for the Cursor</returns>
        Vector3 GetModifiedPosition(ICursor cursor);

        /// <summary>
        /// Returns the Cursor rotation after considering this modifier.
        /// </summary>
        /// <param name="cursor">Cursor that is being modified.</param>
        /// <returns>New rotation for the Cursor</returns>
        Quaternion GetModifiedRotation(ICursor cursor);

        /// <summary>
        /// Returns the Cursor local scale after considering this modifier.
        /// </summary>
        /// <param name="cursor">Cursor that is being modified.</param>
        /// <returns>New local scale for the Cursor</returns>
        Vector3 GetModifiedScale(ICursor cursor);

        /// <summary>
        /// Returns the modified transform for the Cursor after considering this modifier.
        /// </summary>
        /// <param name="cursor">Cursor that is being modified.</param>
        /// <param name="position">Modified position.</param>
        /// <param name="rotation">Modified rotation.</param>
        /// <param name="scale">Modified scale.</param>
        void GetModifiedTransform(ICursor cursor, out Vector3 position, out Quaternion rotation, out Vector3 scale);
    }
}