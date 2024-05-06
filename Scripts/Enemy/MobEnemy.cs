using System.Collections;
using Enemy.Util;
using Scene;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(ColorController))]
    [RequireComponent(typeof(ItemInstantiateController))]
    public class MobEnemy : BaseEnemy, IColorChangeEnemy, IDropItemEnemy
    {
        //色が変わるモブ敵のクラス
        [SerializeField]
        private ColorController colorController;
        [SerializeField]
        private ItemInstantiateController itemInstantiateController;
        
        private bool _isBlinkingColor;
        private ColorType ColorType { get; set; }
        
        protected override void FixedUpdate()
        {
            ChangeColorCountDown(Time.fixedDeltaTime);
            base.FixedUpdate();
        }
        
        protected override void Initialize()
        {
            _isBlinkingColor = false;
            ColorType = GetRandomColorType();
            colorController.SetColorTypeMaterial(ColorType);
            base.Initialize();
        }
        
        protected override void OnDie()
        {
            InstantiateDropItem(transform.position);
        }
        //アイテムを生成
        public void InstantiateDropItem(Vector3 position)
        {
            itemInstantiateController.InstantiateItem(position, ColorType);
        }
        //ランダムな色を取得
        public ColorType GetRandomColorType()
        {
            var randomColorType = (ColorType) Random.Range(0, 2);
            return randomColorType;
        }
        //色を変更
        public void SetColorType(ColorType colorType)
        {
            ColorType = colorType;
            colorController.SetColorTypeMaterial(ColorType);
        }
        //色の変更のカウントダウン
        public void ChangeColorCountDown(float deltaTime)
        {
            if (_isBlinkingColor)return;
            var countDown 
                = colorController.ChangeColorCountDown(deltaTime);
            if (!(countDown <= 0)||_isBlinkingColor) return;
            _isBlinkingColor = true;
            //カウントダウンを知らせるために
            //色を点滅させるコルーチンを開始
            StartCoroutine(BlinkColor());
        }
        //色を点滅させる
        public IEnumerator BlinkColor()
        {
            var blinkColor = colorController.BlinkColor(ColorType);
            //コルーチンの終了を待つ
            yield return blinkColor;
            //点滅が終わったら点滅フラグをfalseにする
            _isBlinkingColor = false;
            //ループが終わったら次の色に変更
            SetColorType(ColorController.GetNextColorType(ColorType));
        }
    }
}