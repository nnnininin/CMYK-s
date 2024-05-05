using UnityEngine;

namespace UI
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField]
        private Texture2D cursorTexture;

        private void Awake()
        {
            // カーソルの中心をテクスチャの中心に設定
            var hotSpot = new Vector2((float)cursorTexture.width / 2, (float)cursorTexture.height / 2);
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        }
    }
}