using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Player.Util;
using Util.RayCaster;

namespace Player.Bullet
{
    public class BulletController : MonoBehaviour
    {
        private IPlayer _player;
        private EventManager _eventManager;
        [SerializeField]
        private AudioSource audioSource;

        private GameObject BulletPrefab => _player.WeaponParameter.BulletPrefab.Value;
        private int WayNumber => _player.WeaponParameter.WayNumber.Value;

        private RayCasterFromScreen rayCasterFromScreen;
        
        private Vector3 _bulletLocalPosition;
        private Vector3 _originalDirection;
        
        private static Vector3 ScreenCenter => new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0);

        private const float Radius = 1.3f;
        private const float SpreadAngle = 5f;

        private void Awake()
        {
            _player = GetComponent<IPlayer>();
            rayCasterFromScreen = new RayCasterFromScreen();
            
            SetUniRx();
        }

        private void SetUniRx()
        {
            _eventManager = _player.EventManager;
            _eventManager.OnShotStart.Subscribe(_ => ShotBullet()).AddTo(this);
        }
        
        private void ShotBullet()
        {
            if (WayNumber % 2 == 0)
            {
                Debug.LogError("WayNumber must be odd number!");
                return;
            }
            var mousePosition = UnityEngine.Input.mousePosition;
            var fromCenterToMouse = mousePosition - ScreenCenter;
            _player.SetMouseInputDirection(fromCenterToMouse);
            
            var hitInfo = rayCasterFromScreen.GetRayCastHit(mousePosition, Color.red);
            if (hitInfo == null) return;
            var hitPosition = hitInfo.Value.point;
            
            _originalDirection = OriginalDirection(hitPosition);
            _bulletLocalPosition = _originalDirection * Radius;
            
            var bullets = GetBullets(WayNumber);
            AssignVelocityToBullets(bullets, _originalDirection);
            AssignSpawnPosition(bullets, transform.position);
            AssignDamage(bullets, _player.AttackPower.AttackPowerValue.Value);
            
            audioSource.Play();
            
            _player.Magazine.DecreaseBullet();
            UpdateRemainingBullet();
        }

        private void AssignVelocityToBullets(IReadOnlyList<Bullet> bullets, Vector3 originalDirection)
        {
            var spreadRange = _player.Accuracy.AccuracyValue.Value;
            var firstDirection = RotateVectorAroundY(originalDirection, Random.Range(-spreadRange, spreadRange));

            AssignVelocity(bullets[0], firstDirection);
            
            if (WayNumber <= 1) return;

            for (var i = 1; i <= WayNumber / 2; i++)
            {
                var leftDirection = RotateVectorAroundY(firstDirection, -SpreadAngle * i);
                var rightDirection = RotateVectorAroundY(firstDirection, SpreadAngle * i);

                AssignVelocity(bullets[i * 2 - 1], leftDirection);
                AssignVelocity(bullets[i * 2], rightDirection);
            }
        }
        
        private Vector3 OriginalDirection(Vector3 targetPosition)
        {
            var direction = (targetPosition - transform.position).normalized;
            direction.y = 0;
            return direction;
        }

        private Bullet[] GetBullets(int count)
        {
            var bullets = new Bullet[count];
            for (var i = 0; i < count; i++)
            {
                bullets[i] = CreateNewBullet();
                SetUpBullet(bullets[i]);
            }
            return bullets;
        }
        
        private Bullet CreateNewBullet()
        {
            var bulletGameObject = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            var bullet = bulletGameObject.GetComponent<Bullet>();
            // プールに戻す代わりにDestroyする
            bullet.ReturnToPool = b => Destroy(b.gameObject);
            return bullet;
        }
        
        private void SetUpBullet(Component bullet)
        {
            var bulletTransform = bullet.transform;
            bulletTransform.SetParent(transform);
            bulletTransform.localPosition = _bulletLocalPosition;
        }

        private void AssignVelocity(Component bullet, Vector3 velocityDirection)
        {
            bullet.GetComponent<Rigidbody>().velocity = velocityDirection * _player.BulletSpeed.BulletSpeedValue.Value;
        }

        private static void AssignSpawnPosition(IEnumerable<Bullet> bullets, Vector3 spawnPosition)
        {
            foreach (var b in bullets)
            {
                b.SetSpawnPosition(spawnPosition);
            }
        }

        private void AssignDamage(IEnumerable<Bullet> bullets, int damage)
        {
            foreach (var bullet in bullets)
            {
                bullet.SetDamage(damage);
            }
        }

        private static Vector3 RotateVectorAroundY(Vector3 originalDirection, float angle)
        {
            var rotation = Quaternion.Euler(0, angle, 0);
            return rotation * originalDirection;
        }
        
        private void UpdateRemainingBullet()
        {
            var bulletPercentage = _player.Magazine.CurrentBullet.Value / (float)_player.Magazine.MaxBullet.Value;
            _eventManager.TriggerOnShotComplete(_bulletLocalPosition, bulletPercentage);
        }
    }
}