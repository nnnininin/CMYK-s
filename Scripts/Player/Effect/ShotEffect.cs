using DG.Tweening;
using UnityEngine;

namespace Player.Effect
{
    public class ShotEffect : MonoBehaviour
    {
        private Tweener _tweener;
        private Sequence _sequence;
        
        private const float Duration = 0.5f;
        private const float MinScaleValue = 0.5f;
        
        private Vector3 currentScale;
        private Vector3 minScale;
        
        public void SetLocalScaleZero()
        {
            transform.localScale = Vector3.zero;
        }
        
        //残弾数のpercentageに応じてスケールを変更
        public void ScaleMagazinePercentage(float percentage)
        {
            //percentageをMinScaleValueから1.0fにマッピング
            var scaleValue = Mathf.Lerp(MinScaleValue, 1.0f, percentage);
            currentScale = Vector3.one * scaleValue;
            _tweener?.Kill();
            _tweener = transform.DOScale(currentScale, Duration)
                .SetEase(Ease.OutBack).SetLink(gameObject);
        }

        public void ScaleToOne()
        {
            _tweener?.Kill();
            _tweener = transform.DOScale(Vector3.one, Duration)
                .SetEase(Ease.OutBack).SetLink(gameObject);
        }

        public void ScaleToZero()
        {
            _tweener?.Kill();
            _tweener = transform.DOScale(Vector3.zero, Duration)
                .SetEase(Ease.OutBack).SetLink(gameObject);
        }
        
        public void Reload(float reloadTime)
        {
            var timeToZero = reloadTime * 0.3f;
            var timeToOne = reloadTime - (timeToZero * 2);

            _tweener?.Kill();
            _sequence?.Kill();

            _sequence = DOTween.Sequence()
                .Append(transform.DOScale(Vector3.zero, timeToZero).SetEase(Ease.InOutSine))
                .Append(transform.DOScale(Vector3.one, timeToOne).SetEase(Ease.OutBack))
                .Append(transform.DOScale(Vector3.zero, timeToZero).SetEase(Ease.OutBack))
                .SetLink(gameObject);

            _sequence.Play();
        }
    }
}
