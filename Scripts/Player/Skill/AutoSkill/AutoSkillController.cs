using UnityEngine;

namespace Player.Skill.AutoSkill
{
    public class AutoSkillController : MonoBehaviour
    {
        private IPlayer _player;
        
        // ゲッターをリアクティブプロパティのValueへのアクセスに変更
        private GameObject DamageFieldPrefab => _player.AutoSkillParameter.DamageFieldPrefab.Value;

        private int Damage => _player.AutoSkillParameter.Attack.Value;
        private float LifeTime => _player.AutoSkillParameter.LifeTime.Value;
         private float MaxScale => _player.AutoSkillParameter.MaxScale.Value;
        private int NumberOfSkill => _player.AutoSkillParameter.NumberOfSkill.Value;
        private IFieldActivator FieldActivator => _player.AutoSkillParameter.FieldActivator.Value;
        
        private void Awake()
        {
            _player = GetComponent<IPlayer>();
        }
        private void FixedUpdate()
        {
            if (_player.AutoSkillParameter.CoolTimeCount.Value > 0)
            {
                _player.AutoSkillParameter.CountDownCoolTime(Time.deltaTime);
            }
            else
            {
                _player.AutoSkillParameter.ResetCoolTimeCount();
                InstantiateSkillField();
            }
        }

        private void InstantiateSkillField()
        {
            Debug.Log("InstantiateSkillField");
            if (DamageFieldPrefab == null)
            {
                Debug.LogError("DamageZonePrefab is not assigned.");
                return;
            }
            Debug.Log($"FieldActivator: {FieldActivator}");

            var transformOfThis = transform;
            FieldActivator.ActivatePrefab(
                DamageFieldPrefab,
                transformOfThis.position,
                Damage,
                MaxScale,
                LifeTime,
                transformOfThis,
                NumberOfSkill
            );
        }
    }
}