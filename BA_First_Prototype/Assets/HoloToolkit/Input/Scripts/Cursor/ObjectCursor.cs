// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;

namespace HoloToolkit.Unity.InputModule
{
    /// <summary>
    /// The object Cursor can switch between different game objects based on its state.
    /// It simply links the game object to set to active with its associated Cursor state.
    /// </summary>
    public class ObjectCursor : Cursor
    {
        [Serializable]
        public struct ObjectCursorDatum
        {
            public string Name;
            public CursorStateEnum CursorState;
            public GameObject CursorObject;
        }

        [SerializeField]
        public ObjectCursorDatum[] CursorStateData;

        /// <summary>
        /// Sprite renderer to change.  If null find one in children
        /// </summary>
        public Transform ParentTransform;

        /// <summary>
        /// On enable look for a sprite renderer on children
        /// </summary>
        protected override void OnEnable()
        {
            if(ParentTransform == null)
            {
                ParentTransform = transform;
            }

            for (int i = 0; i < ParentTransform.childCount; i++)
            {
                ParentTransform.GetChild(i).gameObject.SetActive(false);
            }

            base.OnEnable();
        }

        /// <summary>
        /// Override OnCursorState change to set the correct animation
        /// state for the Cursor
        /// </summary>
        /// <param name="state"></param>
        public override void OnCursorStateChange(CursorStateEnum state)
        {
            base.OnCursorStateChange(state);
            if (state != CursorStateEnum.Contextual)
            {

                // First, try to find a Cursor for the current state
                var newActive = new ObjectCursorDatum();
                for(int cursorIndex = 0; cursorIndex < CursorStateData.Length; cursorIndex++)
                {
                    ObjectCursorDatum cursor = CursorStateData[cursorIndex];
                    if (cursor.CursorState == state)
                    {
                        newActive = cursor;
                        break;
                    }
                }

                // If no Cursor for current state is found, let the last active Cursor be
                // (any Cursor is better than an invisible Cursor)
                if (newActive.Name == null)
                {
                    return;
                }

                // If we come here, there is a Cursor for the new state, 
                // so de-activate a possible earlier active Cursor
                for(int cursorIndex = 0; cursorIndex < CursorStateData.Length; cursorIndex++)
                {
                    ObjectCursorDatum cursor = CursorStateData[cursorIndex];
                    if (cursor.CursorObject.activeSelf)
                    {
                        cursor.CursorObject.SetActive(false);
                        break;
                    }
                }

                // ... and set the Cursor for the new state active.
                newActive.CursorObject.SetActive(true);
            }
        }
    }
}
