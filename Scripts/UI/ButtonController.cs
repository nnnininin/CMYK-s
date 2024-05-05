using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        private Image image;
        private Sprite defaultSprite;
        [SerializeField] private Sprite pressedSprite;

        //idが同じボタンを押した時に他のボタンを押せなくする
        [SerializeField] private int id;

        //押した時に下にずれる量
        [SerializeField] private float pressOffsetY;

        //押した時に呼ばれるイベント
        [SerializeField] private UnityEvent onClick;

        private Transform child;
        private float defaultY;
        private ButtonController[] buttonControllers;

        private bool isPushed;
        private bool haveText;
        
        protected void Awake()
        {
            SetButtonControllers();
        }

        private void SetButtonControllers()
        {
            image = GetComponent<Image>();
            defaultSprite = image.sprite;
            haveText = transform.childCount > 0;
            if (haveText)
            {
                child = transform.GetChild(0);
                defaultY = child.localPosition.y;
            }
            var canvas = GameObject.Find("Canvas").transform;
            buttonControllers = canvas.GetComponentsInChildren<ButtonController>();
        }

        private void OnEnable()
        {
            ButtonActive(true);
        }

        private void ButtonActive(bool active)
        {
            isPushed = !active;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isPushed) return;
            if (haveText)
            {
                var pos = child.localPosition;
                pos.y = defaultY - pressOffsetY;
                child.localPosition = pos;
            }
            if (pressedSprite != null) image.sprite = pressedSprite;
            OnButtonDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isPushed) return;
            if (haveText)
            {
                var pos = child.localPosition;
                pos.y = defaultY;
                child.localPosition = pos;
            }
            image.sprite = defaultSprite;
            OnButtonUp();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (id != -1)
            {
                foreach (var controller in buttonControllers)
                {
                    controller.ButtonActive(controller.id != id);
                }
            }

            OnButtonClick();
            onClick?.Invoke();
            ResetAllButtons();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnButtonEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnButtonExit();
        }

        private void ResetAllButtons()
        {
            foreach (var controller in buttonControllers)
            {
                controller.ButtonActive(true);
            }
        }

        private void OnButtonDown()
        {
            // Down時の共通処理
        }

        private void OnButtonUp()
        {
            // Up時の共通処理
        }

        private void OnButtonClick()
        {
            // Click時の共通処理（SE鳴らすなど）
        }

        private void OnButtonEnter()
        {
            // Enter時の共通処理
            OverrideOnButtonEnter();
        }

        private void OnButtonExit()
        {
            // Exit時の共通処理
            OverrideOnButtonExit();
        }
        
        protected virtual void OverrideOnButtonEnter()
        {
            // Enter時の個別処理
        }
        
        protected virtual void OverrideOnButtonExit()
        {
            // Exit時の個別処理
        }
    }
}