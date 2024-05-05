using UnityEngine;

namespace Util
{
    public class ScreenResolutionManager
    {
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            // フルスクリーンに設定
            Screen.fullScreen = true;

            // モニターのアスペクト比を計算
            var screenAspect = (float)Screen.width / Screen.height;
            const float targetAspect = 16.0f / 9.0f;

            // モニターのアスペクト比と目標のアスペクト比を比較
            var scalingWidth = screenAspect / targetAspect;

            // カメラのrectを設定するための変数
            if (Camera.main == null) return;
            var rect = Camera.main.rect;

            if (scalingWidth < 1.0f)
            {
                // 縦長の解像度の場合（ピラーボックス）
                rect.width = scalingWidth;
                rect.x = (1.0f - scalingWidth) / 2.0f;
            }
            else
            {
                // 横長の解像度の場合（レターボックス）
                var scalingHeight = 1.0f / scalingWidth;
                rect.height = scalingHeight;
                rect.y = (1.0f - scalingHeight) / 2.0f;
            }

            // カメラのrectを更新
            Camera.main.rect = rect;
        }
    }
}