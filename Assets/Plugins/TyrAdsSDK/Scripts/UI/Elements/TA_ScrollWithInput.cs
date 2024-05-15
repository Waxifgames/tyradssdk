using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_ScrollWithInput : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        [SerializeField] float scrollSpeedX = 12f;
        [SerializeField] float scrollSpeedY = 10f;

        [Space(10)]
        [SerializeField] float detectDirDuration = 0.1f;
        [SerializeField] float decelerateTimeX = 0.5f;
        [SerializeField] float angleThresholdX = 45f;


        [Space(10)]
        [SerializeField] float maxSwipeMagnitude = 30f;
        [SerializeField] float minSwipeMagnitude = 5f;

        [Space(20)]
        [SerializeField] RectTransform horizontalContent;
        [SerializeField] ScrollRect verticalScroll;

        [Space(20)]
        [TA_ReadOnly][SerializeField] bool isDetectingDir;
        [TA_ReadOnly][SerializeField] bool isDragging;
        [TA_ReadOnly][SerializeField] SwipeDirection swipeDir;
        [TA_ReadOnly][SerializeField] SwipeAxis swipeAxis;
        [TA_ReadOnly][SerializeField] float width;
        [TA_ReadOnly][SerializeField] float screenWidth;

        [Space(10)]
        [TA_ReadOnly][SerializeField] float xVelocity;
        [TA_ReadOnly][SerializeField] float yVelocity;
        [TA_ReadOnly][SerializeField] float curVelocity;
        [TA_ReadOnly][SerializeField] float angle;
        [TA_ReadOnly][SerializeField] float swipeMagnitude;
        [TA_ReadOnly][SerializeField] float swipeMagnitudeNEW;
        [TA_ReadOnly][SerializeField] float swipeValue;
        bool dragProcess;
        bool setSpeed;


        float detectDirTimer;
        const float SCROLL_MULT = -350;
        float _touchX, _touchY, _curX, _curY, _lastX, _lastY, _posX;
        bool SwipeLeft => (angle >= 0 && angle < angleThresholdX) || (angle < 360 && angle >= 360 - angleThresholdX);
        bool SwipeRight => angle > 180 - angleThresholdX && angle <= 180 + angleThresholdX;

        void Start()
        {
            screenWidth = Screen.width;
        }



        void LateUpdate()
        {
            if (isDetectingDir)
            {
                detectDirTimer -= Time.deltaTime;
            }

            if (!isDetectingDir && dragProcess)
            {
                dragProcess = false;
                setSpeed = true;
                var swipe = new Vector2(_curX - _lastX, _curY - _lastY);
                _lastX = _curX;
                _lastY = _curY;
                var s = swipeDir == SwipeDirection.Left ? 1 : -1; // swipe.magnitude;
                swipeMagnitudeNEW = Mathf.Abs(swipe.magnitude);
                if (swipeMagnitudeNEW < minSwipeMagnitude) swipeMagnitudeNEW = 0;
                swipeValue = Mathf.Abs(swipeMagnitudeNEW / maxSwipeMagnitude);
                swipeValue = Mathf.Clamp(swipeValue, 0, 1f);
            }
            else
            {
                //  swipeMagnitudeNEW = 0f;
            }


            width = horizontalContent.sizeDelta.x;

            if (isDragging && setSpeed) SetSpeed();
            else DecelerateSpeed();

            if (swipeAxis == SwipeAxis.Vertical)
                VerticalScroll();
            else if (swipeDir != SwipeDirection.None) HorizontalScroll();
        }


        void SetSpeed()
        {
            setSpeed = false;
            var dir = swipeDir == SwipeDirection.Right ? -1 : 1;
            // var dir = xVelocity == 0 ? 0 : xVelocity < 0 ? -1 : 1;
            curVelocity = dir * scrollSpeedX * swipeValue;
        }

        void VerticalScroll()
        {
            verticalScroll.velocity = new Vector2(0, -yVelocity * scrollSpeedY);
        }

        void DecelerateSpeed()
        {
            if (Mathf.Abs(curVelocity) < 0.3f)
            {
                if (isDragging)
                {
                    _touchX = _lastX;
                    _touchY = _lastY;
                    isDetectingDir = true;
                }

                curVelocity = 0f;
                return;
            }

            if (swipeDir == SwipeDirection.Right)
            {
                curVelocity += scrollSpeedX / decelerateTimeX * Time.deltaTime;
                if (curVelocity >= 0) curVelocity = 0f;
            }
            else
            {
                curVelocity -= scrollSpeedX / decelerateTimeX * Time.deltaTime;
                if (curVelocity <= 0) curVelocity = 0f;
            }

            // var dir = swipeDir == SwipeDirection.Right ? 1 : -1;
            // var dir = curVelocity == 0 ? 0 : curVelocity < 0 ? 1 : -1;
            // curVelocity += dir * scrollSpeedX / decelerateTimeX * Time.deltaTime;
        }

        void HorizontalScroll()
        {
            if (Mathf.Abs(curVelocity) < 0.1f) return;
            _posX += curVelocity * SCROLL_MULT * Time.deltaTime;
            _posX = Mathf.Clamp(_posX, -width, 0);
            horizontalContent.anchoredPosition = new Vector2(_posX, horizontalContent.anchoredPosition.y);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            swipeDir = SwipeDirection.None;
            curVelocity = 0;

            _touchX = eventData.position.x;
            _touchY = eventData.position.y;
            _curX = _touchX;
            _curY = _touchY;
            _lastX = _touchX;
            _lastY = _touchY;

            isDragging = true;

            isDetectingDir = true;
            detectDirTimer = detectDirDuration;
        }


        public void OnDrag(PointerEventData eventData)
        {
            dragProcess = true;
            _curX = eventData.position.x;
            _curY = eventData.position.y;

            if (isDetectingDir && detectDirTimer <= 0)
            {
                isDetectingDir = false;
                var initialSwipe = new Vector2(_curX - _touchX, _curY - _touchY);
                var dir = initialSwipe.normalized;
                angle = GetAngle(dir);

                if (SwipeLeft || SwipeRight)
                {
                    //  xVelocity = _touchX - _curX;
                    swipeAxis = SwipeAxis.Horizontal;
                    swipeDir = SwipeLeft ? SwipeDirection.Right : SwipeDirection.Left;
                }
                else
                {
                    yVelocity = _touchY - _curY;
                    swipeAxis = SwipeAxis.Vertical;
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            swipeAxis = SwipeAxis.None;
        }

        enum SwipeAxis
        {
            None,
            Horizontal,
            Vertical
        }

        float GetAngle(Vector2 dir)
        {
            var a = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            return (a + 360f) % 360f;
        }
        public enum SwipeDirection
        {
            None,
            Left,
            Right
        }

    }
}