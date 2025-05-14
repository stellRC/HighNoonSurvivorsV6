using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private Texture2D _cursorTexture;

    private Vector2 _cursorHotspot;

    void Start()
    {
        _cursorHotspot = new Vector2(_cursorTexture.width / 2, _cursorTexture.height / 2);
        Cursor.SetCursor(_cursorTexture, _cursorHotspot, CursorMode.Auto);
    }
}
