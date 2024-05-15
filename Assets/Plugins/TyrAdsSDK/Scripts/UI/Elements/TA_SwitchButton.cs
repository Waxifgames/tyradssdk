using System;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_SwitchButton : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] bool switchOn;
        [SerializeField] Image handle;
        [SerializeField] Image background;
        [SerializeField] Color switchOnColor;
        [SerializeField] Color switchOffColor;
        [SerializeField] float moveSpeed = 50;

        bool _isAnimate;
        bool _moveRight;
        RectTransform Rect => (RectTransform)transform;
        RectTransform HandleRect => (RectTransform)handle.transform;
        float Width => Rect.rect.width;
        float HandleWidth => HandleRect.rect.width;

        public event Action OnSwitchOn = delegate { };
        public event Action OnSwitchOff = delegate { };

        void OnEnable()
        {
            button.onClick.AddListener(Switch);
        }

        void OnDisable()
        {
            button.onClick.RemoveListener(Switch);
        }

        void Switch()
        {
            switchOn = !switchOn;

            if (switchOn)
                SwitchOn();
            else
                SwitchOff();
        }

        void SwitchOn()
        {
            _isAnimate = true;
            _moveRight = true;
        }

        void SwitchOff()
        {
            _isAnimate = true;
            _moveRight = false;
        }

        void Update()
        {
            if (!_isAnimate) return;
            if (_moveRight)
            {
                var x = HandleRect.anchoredPosition.x;
                x += Time.deltaTime * moveSpeed;
                HandleRect.anchoredPosition = new Vector2(x, 0);
                if (x >= Width - HandleWidth)
                {
                    _isAnimate = false;
                    EnabledState();
                }
            }
            else
            {
                var x = HandleRect.anchoredPosition.x;
                x -= Time.deltaTime * moveSpeed;
                HandleRect.anchoredPosition = new Vector2(x, 0);
                if (x <= 0)
                {
                    _isAnimate = false;
                    DisabledState();
                }
            }
        }

        void EnabledState()
        {
            OnSwitchOn();
            background.color = switchOnColor;
        }

        void DisabledState()
        {
            OnSwitchOff();
            background.color = switchOffColor;
        }
    }
}