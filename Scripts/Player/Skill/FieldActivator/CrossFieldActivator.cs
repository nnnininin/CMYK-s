using DG.Tweening;
using Player.ScriptableObject;
using UnityEngine;

namespace Player.Skill
{
    public class CrossFieldActivator : IFieldActivator
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
                
                //デフォルトの角度
                const float defaultAngle =13;
                
                //number of skill に応じて360度を等分した角度を求める
                var angle = 360f / numberOfSkill * i + defaultAngle;
                var x = Mathf.Cos(angle * Mathf.Deg2Rad);
                var z = Mathf.Sin(angle * Mathf.Deg2Rad);
                var direction = new Vector3(x, 0, z);
                const float speed = 6;
                fieldComponent.EnableStraightController(direction, speed);
            }
        }
        public void StartTween(DamageField.DamageField field, float lifeTime)
        {
            var transform = field.transform;
            var originalScale = transform.localScale;
            transform.localScale = Vector3.zero;
            
            const float expandDuration = 0.5f;
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
        
        // public void ActivatePrefab()
        // {
        //     //360度の円周上を等間隔に分けた角度
        //     var angleDiff = 360f / numberOfSkill;
        //     for (var i = 0; i < numberOfSkill; i++)
        //     {
        //         var angle = angleDiff * i;
        //         var x = Mathf.Cos(angle * Mathf.Deg2Rad);
        //         var z = Mathf.Sin(angle * Mathf.Deg2Rad);
        //         var direction = new Vector3(x, 0, z);
        //         var damageZone =
        //         DamageField.Init(
        //             prefab,
        //             position,
        //             damage,
        //             initialScale,
        //             lifeTime
        //             );
        //         var damageZoneInstance = damageZone.gameObject;
        //     }
        // }
    }
}