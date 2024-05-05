using System;
using System.Collections;
using Scene;
using UniRx;
using UnityEngine;

namespace Enemy.Util
{
    public class ColorController: MonoBehaviour
    {
        //objectの色を変えるためのマテリアル
        [SerializeField][Tooltip("Blink, Grid, OverRayの順にMaterialを設定")]
        private Material[] cyanMaterials;
        [SerializeField][Tooltip("Blink, Grid, OverRayの順にMaterialを設定")]
        private Material[] magentaMaterials; 
        [SerializeField][Tooltip("Blink, Grid, OverRayの順にMaterialを設定")]
        private Material[] yellowMaterials;
        [SerializeField]
        private Renderer enemyRenderer;
        
        //childの色を変えるためのマテリアル
        [SerializeField][Tooltip("子オブジェクトのMaterialがある場合はアタッチ-ない場合は例外処理済なのでアタッチ不要")]
        private Material cyanChildMaterial;
        [SerializeField][Tooltip("子オブジェクトのMaterialがある場合はアタッチ-ない場合は例外処理済なのでアタッチ不要")]
        private Material magentaChildMaterial;
        [SerializeField][Tooltip("子オブジェクトのMaterialがある場合はアタッチ-ない場合は例外処理済なのでアタッチ不要")]
        private Material yellowChildMaterial;
        [SerializeField][Tooltip("子オブジェクトのRendererがある場合はアタッチ-ない場合は例外処理済なのでアタッチ不要")]
        private Renderer childRenderer;
        
        private const float ColorChangeInterval = 8.0f;
        private ColorType _originalColorType;   //変更前の色
        private float _colorCountDown;
        
        private readonly Subject<Unit> _onMaterialChanged = new();
        public IObservable<Unit> OnMaterialChanged => _onMaterialChanged;
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _colorCountDown = ColorChangeInterval;
        }
        
        //ColorTypeに応じたMaterialに変更
        public void SetColorTypeMaterial(ColorType colorType)
        {
            //colorTypeに応じて新しいマテリアルを取得
            var newMaterials = colorType switch
            {
                ColorType.Cyan => cyanMaterials,
                ColorType.Magenta => magentaMaterials,
                ColorType.Yellow => yellowMaterials,
                _ => enemyRenderer.materials
            };

            // materialsの配列数が0でない場合、指定された処理を行う
            if (enemyRenderer.materials.Length > 0)
            {
                var currentMaterials = enemyRenderer.materials;

                // 最小限、2つの要素が必要（1, 2番目の要素にアクセスするため）
                if (currentMaterials.Length > 2)
                {
                    // 1, 2番目の要素に対応する新しいマテリアルを設定
                    if (newMaterials.Length > 2)
                    {
                        currentMaterials[1] = newMaterials[1];
                        currentMaterials[2] = newMaterials[2];
                    }
                }
                // 更新されたマテリアル配列を適用
                enemyRenderer.materials = currentMaterials;
            }
            else
            {
                // materialsの配列数が0の場合、新しいマテリアル配列をそのまま適用
                enemyRenderer.materials = newMaterials;
            }

            //childのマテリアルがある場合、色を変更
            if (childRenderer != null)
            {
                childRenderer.material = colorType switch
                {
                    ColorType.Cyan => cyanChildMaterial,
                    ColorType.Magenta => magentaChildMaterial,
                    ColorType.Yellow => yellowChildMaterial,
                    _ => childRenderer.material
                };
            }
            
            //UniRx通知を送る
            _onMaterialChanged.OnNext(Unit.Default);
        }
        
        public float ChangeColorCountDown(float deltaTime)
        {
            //カウントダウンを減らす
            if (_colorCountDown > 0)
            {
                _colorCountDown -= deltaTime; 
            }
            return _colorCountDown;
        }
        
        //現在の色に応じて次の色を返す
        public static ColorType GetNextColorType(ColorType colorType)
        {
            return colorType switch
            {
                ColorType.Cyan => ColorType.Magenta,
                ColorType.Magenta => ColorType.Yellow,
                ColorType.Yellow => ColorType.Cyan,
                _ => ColorType.Cyan
            };
        }
        public IEnumerator BlinkColor(ColorType colorType)
        {
            //一定回数点滅させた後に次の色に変更する
            const int flashCount = 5;   //点滅回数
            var count = 0;
            _originalColorType = colorType;
            var nextColorType = GetNextColorType(_originalColorType);
            while (count < flashCount)
            {
                const float nextColorDuration = 0.05f;
                const float originalColorDuration = 0.4f;
                
                SetColorTypeMaterial(nextColorType);
                yield return new WaitForSeconds(nextColorDuration);
                SetColorTypeMaterial(_originalColorType);
                yield return new WaitForSeconds(originalColorDuration);
                count++;
            }
            //点滅終了でタイマーをリセット
            _colorCountDown = ColorChangeInterval;
            //originalColorTypeをnextColorTypeに変更
            _originalColorType = nextColorType;
        }
    }
}