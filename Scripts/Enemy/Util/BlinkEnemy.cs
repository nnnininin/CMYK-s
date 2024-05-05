using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Enemy.Util
{
    public class BlinkEnemy : MonoBehaviour
    {
        [SerializeField] private BaseEnemy baseEnemy;
        [SerializeField] private new Renderer renderer;
        [SerializeField] private ColorController colorController;
        
        private Material blinkMaterial; // 点滅させるマテリアル

        private bool _isTweening; // Tween中かどうかのフラグ
        private Color _originalColor; // 元の色を保存

        private const float BlinkTime = 0.1f; // 点滅する時間
        private const string MaterialName = "Blink"; // 点滅させたいマテリアルの名前
        
        private void Start()
        {
            _isTweening = false;
            // ColorControllerがアタッチされていない場合は直接Materialを取得
            if(colorController == null) GetBlinkMaterial();
            // ColorControllerがアタッチされている場合はUniRxを使って変更の度に随時Materialを取得
            else SetUniRx();
        }
        private void SetUniRx()
        {
            // ダメージを受けたら点滅させる
            baseEnemy.HitPoint.OnDamageReceived
                .Subscribe(_ =>
                {
                    if (_isTweening) return; // Tween中であれば何もしない
                    _isTweening = true;
                    const float maxAlpha = 1.0f; // 透明度の最大値
                    // 透明度を0から変更する
                    if (blinkMaterial != null)
                    {
                        blinkMaterial.DOFade(maxAlpha, BlinkTime)
                            .SetLink(gameObject)
                            .OnComplete(() =>
                            {
                                // 透明度を0.6から元の値に戻す
                                blinkMaterial.DOFade(_originalColor.a, BlinkTime)
                                    .SetLink(gameObject)
                                    .OnComplete(() => { _isTweening = false; });
                            });
                    }
                })
                .AddTo(this);
            // マテリアルが変更されたらMaterialを取得
            colorController.OnMaterialChanged
                .Subscribe(_ =>
                {
                    GetBlinkMaterial();
                })
                .AddTo(this);
        }
        
        private void GetBlinkMaterial()
        {
            blinkMaterial = Array.Find(renderer.materials, m => m.name.StartsWith(MaterialName));
            _originalColor = blinkMaterial.color; //元の色を保存
            if (blinkMaterial == null)
            {
                Debug.LogError($"Material named {MaterialName} not found.");
            }
        }
    }
}