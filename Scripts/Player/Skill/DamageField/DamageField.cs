using System;
using System.Collections.Generic;
using DG.Tweening;
using Enemy;
using Player.ScriptableObject;
using UnityEngine;
using Util.Calculator;
using Util.RayCaster;

namespace Player.Skill.DamageField
{
    public class DamageField : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;
        
        [SerializeField]
        private Transform transformOfThis;
        [SerializeField]
        private Transform majorAxisEndPoint;
        
        private const float IntervalSeconds = 1f;
        
        [SerializeField]
        private CircularController circularController;
        
        [SerializeField]
        private StraightController straightController;
        
        [SerializeField]
        private SurroundController surroundController;
        
        private int _damage;
        private float _maxScale;
        
        private float _skillCircleRadius;
        
        private const string LayerName = "BulletRayHit";
        private const int HitEnemyArraySize = 100;
        
        private RayCasterNonAllocFromScreen rayCasterNonAllocFromScreen;
        private SphereCasterNonAllocFromScreen sphereCasterNonAllocFromScreen;

        private float _timer;
        
        private bool _isActivated;
        private bool _firstCasted;
        
        private Camera _mainCamera;

        private float[] _ellipseSize;
        private Vector3 CenterOfEllipseInScreen => _mainCamera.WorldToScreenPoint(transformOfThis.position);

        private float fieldRadius;
        
        private SkillEffectType _skillEffectType;
        
        //敵のGameObjectとBaseEnemyコンポーネントをセットで格納するDictionary
        private readonly Dictionary<GameObject, IEnemy> _enemyComponents = new();
        
        // ReSharper disable once UnusedMethodReturnValue.Global
        public static DamageField Init(
            GameObject prefab,
            Vector3 spawnPosition,
            int damage,
            float maxScale,
            SkillEffectType skillEffectType = SkillEffectType.Normal
        )
        {
            var damageField = Instantiate(prefab).GetComponentInChildren<DamageField>();
            damageField.gameObject.transform.position = spawnPosition;
            damageField._maxScale = maxScale;
            damageField._damage = damage;
            damageField._skillEffectType = skillEffectType;
            return damageField;
        }
        
        public void Activate()
        {
            DisAbleAllControllers();
            //Fieldの最大値を初期化
            InitializeMaxScale();
            
            //MainCameraを取得
            _mainCamera = Camera.main;
            if (_mainCamera == null) { Debug.LogError("MainCamera is not found."); }
            
            //スキルの半径範囲を設定
            _skillCircleRadius = transformOfThis.localScale.x / 2;
            
            //raycastとspherecastの初期化
            const float rayLength = 10f;
            const float sphereRayLength = 100f;
            rayCasterNonAllocFromScreen = new RayCasterNonAllocFromScreen(HitEnemyArraySize,rayLength ,LayerName);
            sphereCasterNonAllocFromScreen = new SphereCasterNonAllocFromScreen(_skillCircleRadius,HitEnemyArraySize,sphereRayLength,LayerName);
            
            //スクリーン座標上での楕円の最大の大きさと、ワールド座標での円の半径を初期化
            var transformPosition = transformOfThis.position;
            var majorAxisEndPointPosition = majorAxisEndPoint.position;
            if (_mainCamera != null)
                _ellipseSize = EllipseCalculator.CalculateEllipseSize(_mainCamera,
                    _mainCamera.WorldToScreenPoint(majorAxisEndPointPosition).x,
                    _mainCamera.WorldToScreenPoint(transformPosition).x
                );
            fieldRadius = Vector3.Distance(transformPosition, majorAxisEndPointPosition);
            
            Debug.Log("fieldRadius: " + fieldRadius);
            
            _isActivated = false;
            _firstCasted = false;
            
            //音を再生
            audioSource.Play();
        }
        
        private void FixedUpdate()
        {
            if (!_isActivated) return;
            ActivatingBehaviour();
        }
        
        private void ActivatingBehaviour()
        {
            _timer += Time.fixedDeltaTime;
            // 初回のCastSphereToEnemyを実行する条件
            if (!_firstCasted)
            { 
                CastSphereToCurrentPosition(); 
                _firstCasted = true;
                _timer = 0; // タイマーをリセット
                return; // このフレームでの処理を終了
            }
            // intervalSecondsごとにCastSphereToEnemyを実行する条件
            if (!(_timer >= IntervalSeconds)) return;
            CastSphereToCurrentPosition(); 
            _timer -= IntervalSeconds; // タイマーからintervalSecondsを減算
        }
        
        //Fieldの最大値を初期化
        private void InitializeMaxScale()
        {
            //初めはlocalScaleをmaxScaleにしておく
            const float maxScaleY = 0.1f;
            var thisTransform = transform;
            thisTransform.localScale = new Vector3(_maxScale,maxScaleY , _maxScale);
        }
        
        public void StartLife(float lifeTime)
        {
            _isActivated = true;
            StartCoroutine(DestroyAfterSeconds(lifeTime));
        }
        
        private IEnumerator<WaitForSeconds> DestroyAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            const float fadeOutDuration = 0.5f;
            
            // 子オブジェクトにある全てのParticleSystemRendererを取得
            var particleSystemRenderers = GetComponentsInChildren<ParticleSystemRenderer>();
            foreach (var particleSystemRenderer in particleSystemRenderers)
            {
                // マテリアルの透明度を徐々に0にする
                if (particleSystemRenderer.material.HasProperty("_Color"))
                {
                    particleSystemRenderer.material.DOFade(0f, fadeOutDuration)
                        .SetLink(gameObject);
                }
            }
            // フェードアウトが完了するのを待つ
            yield return new WaitForSeconds(fadeOutDuration);
            Destroy(gameObject);
        }
        private void CastSphereToCurrentPosition()
        {
            //sphereCasterを飛ばして、当たった全ての敵の座標を取得
            //スクリーン上での見た目と実際の判定を一致させるため
            //スクリーン上でこの敵が楕円内にあるかを判定する必要がある
            var hits = sphereCasterNonAllocFromScreen.GetRayCastHits(CenterOfEllipseInScreen, Color.red);
            //spherecastに当たっても、実際には敵は楕円外いる可能性があるため
            //foreachで当たった敵がスクリーン上で楕円内にあるかを判定
            if (hits == null) return;
            //全ての各hitに対して楕円内にあるかを判定
            foreach (var hit in hits)
            {
                //あるhitに注目
                if (hit.distance <= 0) { Debug.Log("NotHitFirstSphere"); continue; }
                Debug.Log("HitFirstSphere");
                var hitGameObject = hit.collider.gameObject;
                //hit.pointが画面上で楕円内にあるかを判定
                var isHit = CheckAppearanceOnScreenInEllipse(hitGameObject, hit.point);
                //画面上で楕円境界上にrayを飛ばして当たるかを判定
                if(!isHit) isHit = CheckAppearanceOnScreenOnEllipse(hitGameObject, hit.point);
                //どちらもfalseの場合は次のループへ
                if (!isHit) continue;
                Debug.Log("Hit");
                //敵に楕円が当たった場合は、そのオブジェクトとセットのEnemyコンポーネントを使ってダメージを与える
                CauseDamageToEnemy(_enemyComponents, hitGameObject);
                Debug.Log("ReceiveDamage");
            }
        }

        private void CauseDamageToEnemy(
            IReadOnlyDictionary<GameObject, IEnemy> enemyComponents,
            GameObject hitGameObject
            )
        {
            if (!enemyComponents.ContainsKey(hitGameObject)) return;
            var enemyComponent = enemyComponents[hitGameObject];
            CheckEffectType(enemyComponent);
            enemyComponent.HitPoint.ReceiveDamage(_damage);
        }
        
        private void CheckEffectType(IEnemy enemy)
        {
            Debug.Log($"CheckEffectType: {_skillEffectType}");
            switch (_skillEffectType)
            {
                case SkillEffectType.Normal:
                    break;
                case SkillEffectType.Deceleration:
                    enemy.MovementParameter.DecelerateSpeed();
                    break;
                case SkillEffectType.DamageBoost:
                    enemy.HitPoint.SetDamageBoost();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool CheckAppearanceOnScreenInEllipse(GameObject hitGameObject, Vector3 hitPoint)
        {
            var hitPointInScreen = _mainCamera.WorldToScreenPoint(hitPoint);
            var isHit = EllipseCalculator.IsInEllipse(CenterOfEllipseInScreen, _ellipseSize, hitPointInScreen);
            if(isHit) RegisterEnemyComponent(hitGameObject);
            return isHit;
        }
        
        private bool CheckAppearanceOnScreenOnEllipse(GameObject hitGameObject, Vector3 hitPoint) 
        { 
            Debug.Log("CheckAppearanceOnScreenInEllipse");
            //当たった敵の座標が楕円外にある場合
            //スクリーン上では敵は楕円内に侵入している可能性があるため、楕円境界上での当たり判定を行う
            //ワールド座標での円上の点（スクリーン座標での楕円）を取得
            var rayStartInWorld = transformOfThis.position;
            var rayEndInWorld = new Vector3(hitPoint.x, rayStartInWorld.y, hitPoint.z); //当たった点のy座標を中心のy座標に合わせる
            var directionInWorld = (rayEndInWorld - rayStartInWorld).normalized; //円中心から当たった点への方向ベクトル
            var directionRadiusInWorld = directionInWorld * fieldRadius;    //ベクトルの大きさを円(worldPositionで考えているため)の半径に合わせる
            Debug.Log("directionRadiusInWorld: " + directionRadiusInWorld);
            
            const int degree = 20;
            const int degreeIncrement = 10;
            //-degreeからdegreeまでdegreeIncrement刻みでの楕円の境界上の点をスクリーン座標で取得
            var borderPointsInScreen = GenerateBorderPoints(rayStartInWorld, directionRadiusInWorld, degree, degreeIncrement);

            foreach (var borderPointInScreen in borderPointsInScreen)
            {
                //境界上の点へのレイを飛ばして、当たったオブジェクトが敵であればダメージを与える
                var borderHits = rayCasterNonAllocFromScreen.GetRayCastHits(borderPointInScreen, Color.cyan);
                if (borderHits == null || borderHits.Length == 0) continue;
                foreach (var borderHit in borderHits)
                {
                    //当たったオブジェクトが判定したいhitGameObjectでない場合は次のループへ
                    if (borderHit.distance <= 0 || borderHit.collider.gameObject != hitGameObject) continue;
                    //当たったら登録されていない場合は登録
                    RegisterEnemyComponent(hitGameObject);
                    return true;
                }
            }
            return false;
        }
        
        //円(ワールド座標において)の境界上の点を-degree~degreeの間で生成
        private List<Vector3> GenerateBorderPoints(Vector3 rayStartInWorld, Vector3 directionRadiusInWorld, int degree,
            int degreeIncrement)
        {
            Debug.Log("GenerateBorderPoints");
            var borderPointsInScreen = new List<Vector3>();
            for (var i = -degree; i <= degree; i += degreeIncrement)
            {
                var adjustedDirection = Quaternion.Euler(0, i, 0) * directionRadiusInWorld;
                var borderPointInWorld = rayStartInWorld + adjustedDirection;
                //ワールド座標上での境界上の点をスクリーン上での座標に変換
                var borderPointInScreen = _mainCamera.WorldToScreenPoint(borderPointInWorld);
                borderPointsInScreen.Add(borderPointInScreen);
            }
            return borderPointsInScreen;
        }
        
        //gameObjectが既にDictionaryに登録されているかを確認し、登録されていない場合は登録する
        private void RegisterEnemyComponent(GameObject enemyGameObject)
        {
            //既に登録されている場合はreturn
            if (_enemyComponents.ContainsKey(enemyGameObject)) return;
            var enemyComponent = enemyGameObject.GetComponent<IEnemy>();
            if(enemyComponent == null) return;
            _enemyComponents[enemyGameObject] = enemyComponent;
        }
        
        public void EnableCircularController(Vector3 direction, float speed)
        {
            if(straightController != null) straightController.enabled = false;
            if (circularController != null) circularController.enabled = true;
            if (surroundController != null) surroundController.enabled = false;
            circularController.ActivateMove(direction, speed);
        }
        
        public void EnableStraightController(Vector3 direction, float speed)
        {
            if(circularController != null) circularController.enabled = false;
            if (straightController != null) straightController.enabled = true;
            if (surroundController != null) surroundController.enabled = false;
            straightController.ActivateMove(direction, speed);
        }
        
        public void EnableSurroundController()
        {
            if(circularController != null) circularController.enabled = false;
            if (straightController != null) straightController.enabled = false;
            if (surroundController != null) surroundController.enabled = true;
            surroundController.ActivateMove(Vector3.zero, 0);
        }
        
        private void DisAbleAllControllers()
        {
            if(circularController != null) circularController.enabled = false;
            if (straightController != null) straightController.enabled = false;
        }
        
        //画面外に出た場合は自動で消滅
        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transformOfThis.position , _skillCircleRadius);
        }
    }
}