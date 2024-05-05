using DG.Tweening;
using UnityEngine;

namespace Player.Shield
{
    public class CrackedShield : MonoBehaviour
    {
        private const string AlphaPropertyName = "_Alpha"; // アルファ値を変更するプロパティ名
        
        [SerializeField]
        private Material hologramMaterial; // Hologramマテリアルへの参照

        private const float InitialAlpha = 0.8f; // 初期のアルファ値
        private float _originalAlpha; // 元のアルファ値を保存する変数
        private bool _isBlinking; // Tween中かどうかのフラグ
        private static readonly int Alpha = Shader.PropertyToID(AlphaPropertyName);

        private const float BlinkTime = 0.1f; // 点滅する時間
        private const float MinAlpha = 0.0f; // 最小のアルファ値

        private void Awake()
        {
            // Hologramマテリアルの_Alphaプロパティの初期値を取得
            if (hologramMaterial == null || !hologramMaterial.HasProperty(AlphaPropertyName)) return;
            // 初期値を設定する
            hologramMaterial.SetFloat(Alpha, InitialAlpha);
            // 設定した初期値を_originalAlphaに保存
            _originalAlpha = InitialAlpha;
        }

        public void Blink()
        {
            if (_isBlinking || hologramMaterial == null) return;
            _isBlinking = true;

            // Hologramマテリアルの_AlphaプロパティをTweenさせる
            DOTween.To(() => hologramMaterial.GetFloat(Alpha), 
                    x => hologramMaterial.SetFloat(Alpha, x), 
                    MinAlpha, 
                    BlinkTime)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() => {
                    hologramMaterial.SetFloat(Alpha, _originalAlpha);
                    _isBlinking = false;
                });
        }
    }
}