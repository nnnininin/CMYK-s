using DG.Tweening;
using UnityEngine;
using Util.Animation;

namespace Player.Weapon
{
    public class Weapon: MonoBehaviour
    {
        [SerializeField] private RotationAnimation rotationAnimation;
        
        private Tweener _tweener;
        private Sequence _sequence;

        private const float VectorSize = -0.3f;
        private const float SetDuration = 1.5f;
        private const float KillDuration = 0.1f;
        
        public void UpdateTransform(Vector3 position)
        {
            Debug.Log($"UpdateTransform: {position}");
            transform.localPosition = position;
        }
        
        public void ShotEntryTween()
        {
            rotationAnimation.RotateDegree = 40;
            _tweener?.Kill();
            _tweener = transform
                .DOLocalMoveY(0, KillDuration)
                .SetLink(gameObject);
        }
        
        public void ShotExitTween()
        {
            //ふわふわ漂う動き
            rotationAnimation.RotateDegree = 10;
            _tweener?.Kill();
            var downVector = Vector3.down * VectorSize;
            _tweener = transform
                .DOLocalMove(downVector, SetDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetRelative()
                .SetLink(gameObject);
        }
        
        public void Reload(float reloadTime)
        {
            var timeToDoScale = reloadTime * 0.1f;
            var waitTime = (reloadTime - timeToDoScale)-timeToDoScale;
            
            const float min = 0.2f;
            var minScale = min * Vector3.one;
            
            _tweener?.Kill();
            _sequence?.Kill();
    
            _sequence = DOTween.Sequence()
                .Append(transform.DOScale(minScale, timeToDoScale).SetEase(Ease.InOutSine))
                // AppendInterval を使用して n 秒間の待機時間を追加
                .AppendInterval(waitTime)
                .Append(transform.DOScale(Vector3.one, timeToDoScale).SetEase(Ease.OutBack))
                .SetLink(gameObject);
            _sequence.Play();
        }
        
        //大きさをn秒間でaにする
        public void ScaleTo(float scale, float duration)
        {
            transform.DOScale(scale * Vector3.one, duration).SetEase(Ease.InOutSine).SetLink(gameObject);
        }
    }
}