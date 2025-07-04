#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Gaskellgames.EditorOnly
{
    /// <summary>
    /// Code created by Gaskellgames
    /// </summary>
    
    [System.Serializable]
    public class GgMenuTree
    {
        public string header = "";
        public Color underlineColor = new Color32(179, 179, 179, 255);
        public List<GgMenuTreePage> pages;
        
        private int selectionIndex;

        public int SelectionIndex
        {
            get => selectionIndex;
            internal set => selectionIndex = value;
        }
        
    } // class end
}

#endif