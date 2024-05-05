using TMPro;
using UnityEngine;
using DG.Tweening;

namespace UI.StartScene
{
    public class TweenButtonBlink: MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;
        
        //startTextの透明度をYoYoで変化させる
        private void Start()
        {
            text.DOFade(0f, 0.7f)
                .SetLink(gameObject)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}