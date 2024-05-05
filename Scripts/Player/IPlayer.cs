using Player.Parameter;
using Player.State;
using Player.Util;
using UnityEngine;

namespace Player
{
    public interface IPlayer
    {
        public Transform ChildTransform { get; }
        public StateContext Context { get; }
        public EventManager EventManager { get; }
        
        public Accuracy Accuracy { get; }
        public AttackPower AttackPower { get; }
        public BulletSpeed BulletSpeed { get; }
        public ReloadTime ReloadTime { get; }
        public bool IsActionInputEnabled { get;}
        public DamageController DamageController { get; }
        public Vector2 MouseInputDirection { get; }
        public Vector2 InputMoveDirection { get;}
        public bool IsShotInput { get;}
        public WeaponParameter WeaponParameter { get; }
        public SkillParameter SkillParameter { get; }
        public AutoSkillParameter AutoSkillParameter { get; }
        
        public Magazine Magazine { get; }
        public MoveSpeed MoveSpeed { get; }
        public ButtonPressedAction ButtonPressedAction { get;}
        
        public FireRate FireRate { get; }
        public HitPoint HitPoint { get; }
        
        public Animator Animator { get; }
        public void SetRunAnimation(bool isRun);
        
        public void SetButtonPressedAction(
            ButtonPressedAction buttonPressedAction
            );
        
        public void SetInputMoveDirection(Vector2 inputMoveDirection);
        
        public void SetIsActionInputEnabled(bool isActionInputEnabled);
        
        public void SetIsShotInput(bool isShotInput);
        
        public void SetVelocity(Vector3 velocity);
        
        public void SetMouseInputDirection(Vector2 mouseInputDirection);
        public GameObject GetPlayerInstance();
    }
}