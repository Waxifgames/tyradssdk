using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_RefreshPageDetector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] RectTransform content;
        [SerializeField] float refreshTriggerY = 150;
        [SerializeField] ScrollRect scroll;
        [SerializeField] float animTriggerY = 100;
        [SerializeField] float animRotateSpeed = 150;
        [SerializeField] RectTransform animTransform;

        [Space(20)]
        [SerializeField] [TA_ReadOnly] float currentDelta;
        public float animationTime = 1f;
        public float _animTimer;
        public bool refreshProcess;
        public bool _isAnimate;
        
        float _startPos, _lastPos;
        bool OutOfBound => content.anchoredPosition.y < refreshTriggerY;
        bool InAnimateTrigger => content.anchoredPosition.y < animTriggerY;
        public event Action OnRefresh = delegate { };
        void Update()
        {
            if (!_isAnimate) return;
            var z = animTransform.localRotation.eulerAngles.z;
            z += Time.deltaTime * animRotateSpeed;
            animTransform.localRotation = Quaternion.Euler(0, 0, z);

            if (refreshProcess)
            {
                _animTimer -= Time.deltaTime;
                if (_animTimer <= 0)
                {
                    StopRefresh();
                }
            }
        }
        public void Refresh()
        {
            if (refreshProcess) return;
            refreshProcess = true;

            _animTimer = animationTime;

            RefreshAnimation();
            OnRefresh();
        }

        public void RefreshAnimation()
        {
            _isAnimate = true;
            scroll.enabled = false;
            content.anchoredPosition = new Vector2(0, refreshTriggerY);
        }

        public void StopRefresh()
        {
            _isAnimate = false;
            refreshProcess = false;
            scroll.enabled = true;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPos = GetComponent<ScrollRect>().verticalNormalizedPosition;
            _lastPos = _startPos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var currentScrollPosition = scroll.verticalNormalizedPosition;
            currentDelta = currentScrollPosition - _lastPos;
            _lastPos = currentScrollPosition;
            if (InAnimateTrigger)
            {
                _isAnimate = true;
            }

            if (OutOfBound)
                Refresh();
        }

   
        public void OnEndDrag(PointerEventData eventData)
        {
            currentDelta = 0f;
        }



    }
}