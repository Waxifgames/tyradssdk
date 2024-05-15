using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_ScrollVertical : TA_Scroll
    {
        [TA_ReadOnly] public List<TA_OfferItem> items = new();
        public event Action<TA_OfferItem> OnScrollDown = delegate { };
        public event Action<TA_OfferItem> OnScrollUp = delegate { };

        #region Local

        int _total;
        float _topTrigger;
        float _bottomTrigger;
        TA_OfferItem First => items[0];
        TA_OfferItem Last => items[^1];
        bool NotLast => Last.ID < _total - 1;
        bool NotFirst => First.ID > 0;
        bool ShouldSwapTop => Last.Position.y > _bottomTrigger && NotLast;
        bool ShouldSwapDown => First.Position.y < _topTrigger && NotFirst;

        #endregion

        public void FindItems()
        {
            if (!rect) rect = (RectTransform)grid.transform;
            items = rect.GetComponentsInChildren<TA_OfferItem>().ToList();
            _topTrigger = Screen.height;
            _bottomTrigger = -grid.cellSize.y / 2;
        }

        public void Init(int total)
        {
            _total = total;
            var height = total * (grid.cellSize.y + grid.spacing.y) + grid.spacing.y;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);

            startingPadding = grid.padding.top;
            step = (int)(grid.cellSize.y + grid.spacing.y);

            isEnable = true;
        }

        void LateUpdate()
        {
            if (!isEnable) return;

            if (ShouldSwapTop)
            {
                var swap = First;
                AddPaddingTop(step);
                SwapTop(swap);
                OnScrollUp(swap);
                return;
            }

            if (ShouldSwapDown)
            {
                var swap = Last;
                RemovePaddingTop(step);
                SwapDown(swap);
                OnScrollDown(swap);
                return;
            }
        }

        void SwapTop(TA_OfferItem item)
        {
            item.transform.SetSiblingIndex(items.Count);
            items.RemoveAt(0);
            items.Add(item);
        }

        void SwapDown(TA_OfferItem item)
        {
            item.transform.SetSiblingIndex(0);
            items.RemoveAt(items.Count - 1);
            items.Insert(0, item);
        }

        void AddPaddingTop(int value)
        {
            grid.padding.top += value;
        }

        void RemovePaddingTop(int value)
        {
            grid.padding.top -= value;
            if (grid.padding.top < startingPadding) grid.padding.top = startingPadding;
        }
    }
}