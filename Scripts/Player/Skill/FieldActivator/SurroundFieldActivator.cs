using DG.Tweening;
using Player.ScriptableObject;
using UnityEngine;

namespace Player.Skill
{
    public class SurroundFieldActivator : IFieldActivator
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
                        maxScale
                    );
                fieldComponent.Activate();
                SetParent(fieldComponent.gameObject, parent);
                StartTween(fieldComponent, lifeTime);
            }
        }
        public void StartTween(DamageField.DamageField field, float lifeTime)
        {
            var transform = field.transform;
            var originalScale = transform.localScale;
            transform.localScale = Vector3.zero;
            
            //ここでは先にstartLifeを呼び出している
            field.StartLife(lifeTime);
            var expandDuration = lifeTime * 0.8f;
            transform.DOScale(originalScale, expandDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                    {
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