using UnityEngine;

namespace Enemy.Util
{
    public class SpawnAnimation : MonoBehaviour
    {
        [SerializeField] private float duration; // アニメーションの持続時間
        private float _currentTime; // 現在の時間を追跡

        private Vector3 _originalScale; // 初期のスケール
        
        private void Awake()
        {
            var transformOfThis = transform;
            _originalScale = transformOfThis.localScale; // 初期のスケールを保存
            transformOfThis.localScale = Vector3.zero; // 最初にスケールを0に設定
        }

        private void Update()
        {
            if (_currentTime < duration)
            {
                _currentTime += Time.deltaTime; // 経過時間を加算
                var scale = _currentTime / duration; // 0から1の範囲でスケール値を計算
                transform.localScale = _originalScale * scale; // スケールを更新
            }
            else
            {
                transform.localScale = _originalScale; // 最終的なスケールを設定
                enabled = false; // さらなるUpdate呼び出しを防ぐためにコンポーネントを無効化
            }
        }
    }
}