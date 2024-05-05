// using CameraScript;
// using Player.Util;
// using UniRx;
// using UnityEngine;
//
// namespace Player.Shield
// {
//     public class ShieldController : MonoBehaviour
//     {
//         private IPlayer _player;
//         private EventManager eventManager;
//         private GameObject ShieldPrefab => _player.ShieldPrefab;
//         private GameObject CrackedShieldPrefab => _player.CrackedShieldPrefab;
//         private GameObject BrokenShieldPrefab => _player.BrokenShieldPrefab;
//         
//         private GameObject _shieldInstance;
//         private GameObject _crackedShieldInstance;
//         private GameObject _brokenShieldInstance;
//         
//         private Shield _shield;
//         private CrackedShield _crackedShield;
//         
//         private Camera _mainCamera;
//         private ShakeCamera _shakeCamera;
//         
//         
//         private static readonly Vector3 LocalPosition = new(0, 0.1f, 0);
//
//         private void Awake()
//         {
//             _player = GetComponent<IPlayer>();
//             _mainCamera = Camera.main;
//             if (_mainCamera != null) 
//                 _shakeCamera = _mainCamera.GetComponent<ShakeCamera>();
//             SetUniRx();
//             InstantiateShieldObjects();
//         }
//
//         private void SetUniRx()
//         {
//             eventManager = _player.EventManager;
//             eventManager.OnUpdateData
//                 .Subscribe(_ => SetReactiveProperty())
//                 .AddTo(this);
//             eventManager.OnDamageShieldPoint
//                 .Subscribe(_ => BlinkShield())
//                 .AddTo(this);
//         }
//         
//         private void SetReactiveProperty()
//         {
//             _player.ShieldPoint.CurrentShieldPoint
//                 .Pairwise((previous, current) => new { Previous = previous, Current = current })
//                 .Where(pair =>
//                     (pair.Previous > _player.ShieldPoint.MaxShieldPoint.Value * 0.5 && 
//                      pair.Current <= _player.ShieldPoint.MaxShieldPoint.Value * 0.5) || 
//                     (pair.Previous > 0 && 
//                      pair.Current <= 0)
//                 )
//                 .Subscribe(pair =>
//                 {
//                     if (pair.Current <= 0)
//                     {
//                         BreakShield();
//                     }
//                     else
//                     {
//                         CrackShield();
//                     }
//                 })
//                 .AddTo(this);
//         }
//
//         private void InstantiateShieldObjects()
//         {
//             _shieldInstance = InstantiateShieldObject(ShieldPrefab, ref _shieldInstance);
//             _crackedShieldInstance = InstantiateShieldObject(CrackedShieldPrefab, ref _crackedShieldInstance);
//             _brokenShieldInstance = InstantiateShieldObject(BrokenShieldPrefab, ref _brokenShieldInstance);
//             
//             _shield = _shieldInstance.GetComponent<Shield>();
//             _crackedShield = _crackedShieldInstance.GetComponent<CrackedShield>();
//             
//             SetActiveShieldObject(_shieldInstance);
//         }
//         private GameObject InstantiateShieldObject(GameObject prefab, ref GameObject instance)
//         {
//             if(instance != null)
//             {
//                 Destroy(instance);
//             }
//             var parent = transform;
//             var position = parent.position + LocalPosition;
//             instance = Instantiate(prefab, position, Quaternion.identity, parent);
//             return instance;
//         }
//         
//         private void SetActiveShieldObject(GameObject activeObject)
//         {
//             _shieldInstance.SetActive(false);
//             _crackedShieldInstance.SetActive(false);
//             _brokenShieldInstance.SetActive(false);
//             activeObject.SetActive(true);
//         }
//         
//         private void SetAllShieldObjectsActive(bool isActive)
//         {
//             _shieldInstance.SetActive(isActive);
//             _crackedShieldInstance.SetActive(isActive);
//             _brokenShieldInstance.SetActive(isActive);
//         }
//         
//         private void CrackShield()
//         {
//             _shakeCamera.ShakePlayerDamage();
//             SetAllShieldObjectsActive(false);
//             SetActiveShieldObject(_crackedShieldInstance);
//         }
//         
//         private void BreakShield()
//         {
//             _shakeCamera.ShakePlayerDamage();
//             SetActiveShieldObject(_brokenShieldInstance);
//             var particle = _brokenShieldInstance.GetComponentInChildren<ParticleSystem>();
//             particle.Play();
//         }
//         
//         private void BlinkShield()
//         {
//             if (_shieldInstance.activeSelf)
//             {
//                 if (_shield == null) return;
//                 _shield.Blink();
//             }
//             else if(_crackedShieldInstance.activeSelf)
//             {
//                 if (_crackedShield == null) return;
//                 _crackedShield.Blink();
//             }
//         }
//     }
// }