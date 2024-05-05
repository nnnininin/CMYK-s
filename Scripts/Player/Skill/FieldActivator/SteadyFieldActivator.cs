using DG.Tweening;
using Player.ScriptableObject;
using UnityEngine;

namespace Player.Skill
{
    public class SteadyFieldActivator : IFieldActivator
    {
        public void ActivatePrefab(
            GameObject prefab,
            Vector3 position,
            int damage,
            float maxScale,
            float lifeTime,
            Transform parent,
            int numberOfSkill,
            SkillEffectType effectType
        )
        {
            for (var i = 0; i < numberOfSkill; i++)
            {
                //damageFieldの生成
                var fieldComponent =
                    DamageField.DamageField.Init(
                        prefab,
                        position,
                        damage,
                        maxScale,
                        effectType
                    );
                fieldComponent.Activate();
                SetParent(fieldComponent.gameObject, parent);
                StartTween(fieldComponent, lifeTime);
            }
        }
        public void StartTween(DamageField.DamageField field, float lifeTime)
        {
            var transform = field.transform;
            //localScaleをoriginalScaleに保存
            var originalScale = transform.localScale;
            //localScaleを0にする
            transform.localScale = Vector3.zero;
            const float expandDuration = 0.5f;
            //originalScaleにTweenで戻す
            transform.DOScale(originalScale, expandDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                    {
                        field.StartLife(lifeTime);
                    }
                )
                .SetLink(field.gameObject);
        }
        
        public void SetParent(GameObject gameObject,Transform parent)
        {
            if(parent == null) return;
            gameObject.transform.SetParent(parent);
        }
    }
}