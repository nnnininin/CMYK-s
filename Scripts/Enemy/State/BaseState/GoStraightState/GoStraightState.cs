using System;
using UnityEngine;
using Util.RayCaster;
using Random = UnityEngine.Random;

namespace Enemy.State.BaseState.GoStraightState
{
    public class GoStraightState : BaseState
    {
        public GoStraightState(IEnemy enemy)
            : base(enemy) { }

        public override IState.State State => IState.State.GoStraight;

        private RayCasterFromScreen rayCasterFromScreen;
        private float _originalSpeed;
        private Vector3 direction;

        protected override void OnEntry()
        {
            _originalSpeed = MovementParameter.MovingSpeed;
            rayCasterFromScreen = new RayCasterFromScreen();
            var spawnPositionInWorld = SpawnPositionInWorld;
            
            var spawnPositionInViewport = MainCamera.ScreenToViewportPoint(SpawnPositionInScreen);
            var targetPositionInScreen = GetTargetPositionInScreen(spawnPositionInViewport);
            var hitInfo = rayCasterFromScreen.GetRayCastHit(targetPositionInScreen, Color.red);
            if (hitInfo == null) return;
            var targetPositionInWorld = hitInfo.Value.point;
            var velocityDirection = targetPositionInWorld - spawnPositionInWorld;
            velocityDirection.y = 0;
            direction = velocityDirection.normalized;
            var initialVelocity = direction * MovementParameter.MovingSpeed;
            MovementParameter.SetCurrentVelocity(initialVelocity);
        }

        protected override void OnUpdate() { }

        protected override void OnFixedUpdate()
        {
            const float tolerance = 0.01f;
            if (!(Math.Abs(_originalSpeed - MovementParameter.MovingSpeed) > tolerance)) return;
            _originalSpeed = MovementParameter.MovingSpeed;
            var newVelocity = direction * _originalSpeed;
            MovementParameter.SetCurrentVelocity(newVelocity);
        }

        protected override void OnExit() { }

        private Vector3 GetTargetPositionInScreen(Vector3 spawnPositionInViewport)
        {
            var isVertical = spawnPositionInViewport.y > 0 && spawnPositionInViewport.y < 1;
            var coefficient = Random.Range(0f, 0.5f);
            var centerInViewport = new Vector3(0.5f, 0.5f, 0);
            var diff = centerInViewport - spawnPositionInViewport;
            var targetPositionInViewport = centerInViewport + diff;
            targetPositionInViewport.z = 0;

            if (isVertical)
            {
                var num = spawnPositionInViewport.y > 0.5f ? 1 : -1;
                var diffFromCenter = Mathf.Abs(0.5f - spawnPositionInViewport.y);
                var springLength = diffFromCenter / 0.5f;
                targetPositionInViewport.y += springLength * coefficient * num;
            }
            else
            {
                var num = spawnPositionInViewport.x > 0.5f ? 1 : -1;
                var diffFromCenter = Mathf.Abs(0.5f - spawnPositionInViewport.x);
                var springLength = diffFromCenter / 0.5f;
                targetPositionInViewport.x += springLength * coefficient * num;
            }
            return MainCamera.ViewportToScreenPoint(targetPositionInViewport);
        }
    }
}