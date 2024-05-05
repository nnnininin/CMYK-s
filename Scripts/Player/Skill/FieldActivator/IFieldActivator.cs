using Player.ScriptableObject;
using UnityEngine;

namespace Player.Skill
{
    public interface IFieldActivator
    {
        public void ActivatePrefab(
            GameObject prefab,
            Vector3 position,
            int damage,
            float maxScale,
            float lifeTime,
            Transform parent = null,
            int numberOfSkill =1,
            SkillEffectType effectType = SkillEffectType.Normal
            );
        public void StartTween(DamageField.DamageField field, float lifeTime);
        public void SetParent(GameObject gameObject,Transform parent);
    }
}