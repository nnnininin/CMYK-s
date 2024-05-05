using System;
using UnityEngine;
using Util.RayCaster;

namespace Player.Bullet
{
    public class Bullet : MonoBehaviour
    {
        protected Camera MainCamera;
        protected SphereCasterFromScreen SphereCasterFromScreen;
        
        private Transform _parentTransform;

        protected int Damage { get; private set; }

        protected bool IsNearEnemy;
        protected bool IsNotifyDamage;
        
        protected Vector3 SpawnPosition;
        
        private const float SphereRadius = 0.25f;
        private const float RayLength = 10f;
        
        // ScreenOutMargin > DamageOutMargin
        //if minus, it is inside of screen
        private const float ScreenOutMargin = 1.0f;

        // プールに戻すためのアクションを追加
        public Action<Bullet> ReturnToPool { get; set; }

        private void Awake()
        {
            SphereCasterFromScreen = new SphereCasterFromScreen(SphereRadius,RayLength,"BulletRayHit");
            IsNearEnemy = false;
            IsNotifyDamage = true;
        }

        private void Start()
        {
            MainCamera = Camera.main;
            //parentのtransformコンポーネントを取得
            _parentTransform = transform.parent;
        }

        protected virtual void FixedUpdate()
        {
            var currentWorldPosition = transform.localPosition + _parentTransform.position;
            LimitByViewPort(currentWorldPosition);
            //敵が近くにいる場合のみレイを飛ばす
            if (!IsNearEnemy)
                return;
            CastRayToCurrentPosition(currentWorldPosition);
        }
        
        public void SetDamage(int damage)
        {
            Damage = damage;
        }
        
        public void SetSpawnPosition(Vector3 spawnPosition)
        {
            SpawnPosition = spawnPosition;
        }

        //現在の位置にレイを飛ばす
        protected virtual void CastRayToCurrentPosition(Vector3 worldPosition)
        {
            IsNearEnemy = false;

            var screenPosOfThis = MainCamera.WorldToScreenPoint(worldPosition);
            
            var hitInfo = SphereCasterFromScreen.GetRayCastHit(screenPosOfThis, Color.green);
            var hitEnemy = hitInfo?.collider.GetComponent<Enemy.IEnemy>();
            if (hitEnemy == null) return;
            if (IsNotifyDamage) { hitEnemy.HitPoint.ReceiveDamage(Damage); }
            ReturnToPool?.Invoke(this);
        }

        //敵タグのオブジェクトが近くにいるかどうかを判定
        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Enemy"))
                return;
            IsNearEnemy = true;
        }

        //画面外に出たらプールに戻す
        private void LimitByViewPort(Vector3 worldPosition)
        {
            //convert world position to viewport position!!! <<not screen position>>
            var viewPortPosition = MainCamera.WorldToViewportPoint(worldPosition);
            //if it is out of damage range, invalid damage trigger
            IsNotifyDamage = IsDamageable(viewPortPosition);
            //if it is out of screen, return to pool
            ReturnThisToPool(viewPortPosition);
        }
        private static bool IsDamageable(Vector3 viewPortPosition)
        {
            const float damageOutMargin = 0.02f;
            return viewPortPosition.x is >= damageOutMargin and <= 1 -damageOutMargin
                   && viewPortPosition.y is >= damageOutMargin and <= 1 - damageOutMargin;
        }
        private void ReturnThisToPool(Vector3 viewPortPosition)
        {
            if (
                viewPortPosition.x < -ScreenOutMargin
                || viewPortPosition.x > 1 + ScreenOutMargin
                || viewPortPosition.y < -ScreenOutMargin
                || viewPortPosition.y > 1 + ScreenOutMargin
            )
            { ReturnToPool?.Invoke(this); }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, SphereRadius);
        }
    }
}
