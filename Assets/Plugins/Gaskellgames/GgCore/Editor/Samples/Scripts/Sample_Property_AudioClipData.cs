using System.Collections.Generic;
using UnityEngine;

namespace Gaskellgames
{
    /// <summary>
    /// Code created by Gaskellgames
    /// </summary>
    
    public class Sample_Property_AudioClipData : MonoBehaviour
    {
        // ---------- AudioClipData ----------

        [SerializeField]
        private AudioClipData audioClipData;

        [SerializeField]
        private AudioClipData audioClipData2;

        [SerializeField, Space]
        private List<AudioClipData> audioClipDataList = new List<AudioClipData>();

        private void RemoveConsoleWarning()
        {
            if (audioClipData != null) { }
            if (audioClipData2 != null) { }
            if (audioClipDataList != null) { }
        }

    } // class end
}
