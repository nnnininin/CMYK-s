using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;

// List<T>を使用するため

namespace Player.Util
{
    public class BlinkPlayer : MonoBehaviour
    {
        private IPlayer _player;
        
        private const string Tag = "Heart";
        private const float InitialAlpha = 1.0f; // 初期のアルファ値
        
        private readonly List<Renderer> _validRenderers = new(); // _Colorプロパティを持つレンダラーのリスト
        private readonly List<Color> _originalColors = new(); // 元の色を保存するリスト
        private bool _isTweening; // Tween中かどうかのフラグ
        
        private const float BlinkTime = 0.05f; // 点滅する時間
        private const float MinAlpha = 0.0f; // 最小のアルファ値

        private void Awake()
        {
            _player = GetComponent<IPlayer>();
        }
        
        private void Start()
        {
            var allChildren = GetComponentsInChildren<Transform>(); // すべての子オブジェクトのTransformを取得
            foreach (var child in allChildren)
            {
                // 特定のtagを持つオブジェクトをチェック
                if (!child.CompareTag(Tag)) continue;
                var childRenderer = child.GetComponent<Renderer>();
                if (childRenderer == null || !childRenderer.material.HasProperty("_BaseColor")) continue;
                
                // 透明度の初期値を設定
                var material = childRenderer.material;
                var color = material.color;
                color.a = InitialAlpha;
                material.color = color;            
                
                _validRenderers.Add(childRenderer);
                _originalColors.Add(childRenderer.material.color);
            }
            //Debug.Log($"_validRenderers.Count: {_validRenderers.Count}");
            _isTweening = false;
            SetUniRx();
        }

        private void SetUniRx()
        {
            _player.EventManager.OnDamageHitPoint
                .Subscribe(_ => Blink())
                .AddTo(this);
        }

        private void Blink()
        {
            if (_isTweening) return;
            _isTweening = true;

            foreach (var validRenderer in _validRenderers)
            {
                var material = validRenderer.material;
                var index = _validRenderers.IndexOf(validRenderer);
                var originalColor = _originalColors[index];

                // アルファ値を0に変化させる
                DOTween.To(() => material.color.a, 
                        x => material.color = 
                            new Color(material.color.r, 
                                material.color.g, 
                                material.color.b, x), MinAlpha, BlinkTime)
                    .SetLoops(2, LoopType.Yoyo)
                    .OnComplete(() => {
                        material.color = originalColor;
                        _isTweening = false;
                    })
                    .SetLink(gameObject);
            }
        }
    }
}