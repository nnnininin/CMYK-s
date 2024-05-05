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
        
        public void InstantiateDropItem(Vector3 position)
        {
            itemInstantiateController.InstantiateItem(position, ColorType);
        }
        
        public ColorType GetRandomColorType()
        {
            var randomColorType = (ColorType) Random.Range(0, 2);
            return randomColorType;
        }

        public void SetColorType(ColorType colorType)
        {
            ColorType = colorType;
            colorController.SetColorTypeMaterial(ColorType);
        }

        public void ChangeColorCountDown(float deltaTime)
        {
            if (_isBlinkingColor)return;
            var countDown 
                = colorController.ChangeColorCountDown(deltaTime);
            if (!(countDown <= 0)||_isBlinkingColor) return;
            _isBlinkingColor = true;
            StartCoroutine(BlinkColor());
        }
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