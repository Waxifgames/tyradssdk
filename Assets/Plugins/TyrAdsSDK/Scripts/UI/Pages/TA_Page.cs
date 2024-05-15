using System;
using System.Collections.Generic;
using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Pages
{
    [AddComponentMenu("")]
    public class TA_Page : MonoBehaviour
    {
        [Space(10)]
        public bool disableCanvasAtHide = true;
        [SerializeField] Canvas canvas;
        [SerializeField] GraphicRaycaster canvasRaycast;
        [Space(10)]
        [SerializeField] List<Button> backButtons = new();
        [SerializeField] List<TA_PageLink> connectedPages = new();

        [Space(20)]
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] [TA_ReadOnly] public TA_Page _previousPage;
        public event Action<TA_Page, TA_Page> OnShowPlease = delegate { };

        protected virtual void Showing()
        {
        }

        protected virtual void Hiding()
        {
        }

        void Start()
        {
            ResetPosition();
        }

        void ResetPosition()
        {
            var rect = (RectTransform)transform;
            rect.anchoredPosition = Vector2.zero;
        }

        public void Show()
        {
            if (disableCanvasAtHide)
            {
                canvas.gameObject.SetActive(true);
            }
            else 
            {
                if (canvasRaycast) canvasRaycast.enabled = true;
                canvas.enabled = true;
            }
            Enable(canvasGroup);
            Subscribe();
            Showing();
        }

        public void Hide()
        {
            if (disableCanvasAtHide)
            {
                canvas.gameObject.SetActive(false);
            }
            else
            {
                if (canvasRaycast) canvasRaycast.enabled = false;
                canvas.enabled = false;
            }
            Disable(canvasGroup);
            UnSubscribe();
            Hiding();
        }

        void Back() => ShowPagePlease(_previousPage);
        void ShowPagePlease(TA_Page page) => OnShowPlease(this, page);

        void Subscribe()
        {
            foreach (var button in backButtons)
                button.onClick.AddListener(Back);

            foreach (var connected in connectedPages)
            {
                connected.Subscribe();
                connected.OnShow += ShowPagePlease;
            }
        }

        void UnSubscribe()
        {
            foreach (var button in backButtons)
                button.onClick.RemoveListener(Back);

            foreach (var connected in connectedPages)
            {
                connected.UnSubscribe();
                connected.OnShow -= ShowPagePlease;
            }
        }

        void Enable(CanvasGroup group)
        {
            if (!group) return;
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        void Disable(CanvasGroup group)
        {
            if (!group) return;
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }
}