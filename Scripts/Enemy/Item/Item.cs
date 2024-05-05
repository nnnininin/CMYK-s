using DG.Tweening;
using Scene;
using UnityEngine;

namespace Enemy.Item
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private TemporaryGameDataSo temporaryGameData;

        [SerializeField]
        private ColorType colorType;
        
        private const int Point = 1;

        private const string PlayerTag = "Player";
        
        private Sequence _sequence;

        public void Awake()
        {
            const float seconds = 10f;
            const float interval = 0.2f;
            const int blinkCount = 30;
            
            // DOTweenのシーケンスを開始
            _sequence = DOTween.Sequence();

            // n秒待つ
            _sequence
                .SetLink(gameObject)
                .AppendInterval(seconds);
            
            // ここで点滅を開始
            _sequence.Append(GetComponent<Renderer>().material.
                DOColor(new Color(1, 1, 1, 0), interval).
                SetLoops(blinkCount, LoopType.Yoyo)).
                SetLink(gameObject);

            // オブジェクトを破壊
            _sequence
                .SetLink(gameObject)
                .AppendCallback(() => Destroy(gameObject));
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                GetItem(other.transform.position);
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                GetItem(other.transform.position);
            }
        }

        private void GetItem(Vector3 position)
        {
            const float tweenTime = 0.5f;
            // DOTweenのシーケンスをキル
            _sequence.Kill();
            transform.DOMove(position,tweenTime)
                .SetLink(gameObject)
                .OnComplete(() =>
            {
                GetItemExp();
                Destroy(gameObject);
            });
        }
        private void GetItemExp()
        {
            temporaryGameData.AddColorExp(colorType, Point);
        }
    }
}