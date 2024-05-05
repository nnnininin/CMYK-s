using UnityEngine;

namespace Util.Animation
{
    public class ScalePulseAnimation : MonoBehaviour
    {
        [SerializeField] private float period; // 変化する周期 T
        [SerializeField][Range(1, 2)]
        private float maxScaleMultiplier; // 最大スケール倍率
        [SerializeField][Range(0, 1)]
        private float minScaleMultiplier; // 最小スケール倍率
        [SerializeField] private int fps; // FPS

        private float defaultScale; // 初期スケール
        private float amplitude; // 振幅 A

        private void Start()
        {
            defaultScale = transform.localScale.x;
            var maxScale = defaultScale * maxScaleMultiplier;
            var minScale = defaultScale * minScaleMultiplier;
            amplitude = (maxScale - minScale) / 2; // 最大と最小の差の半分を振幅とする
            var interval = 1f / fps;
            InvokeRepeating(nameof(UpdateScale), 0f, interval);
        }

        private void UpdateScale()
        {
            // 現在の時間に基づいてサイン波の値を計算
            var cycleTime = Time.time / period; 
            var sinWave = Mathf.Sin(cycleTime * 2 * Mathf.PI);

            // サイン波の値をスケールの範囲にマッピング
            var scale = defaultScale + amplitude * sinWave; // 振幅を加えて、中心値からの変化を適用

            // 新しいスケールを適用
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}