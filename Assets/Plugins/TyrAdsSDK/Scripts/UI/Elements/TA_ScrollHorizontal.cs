using System;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_ScrollHorizontal : TA_Scroll
    {
        int _start, _end, _firstStep;

        public List<TA_UserAge> items = new();
        public event Action<TA_UserAge> OnScrollLeft = delegate { };
        public event Action<TA_UserAge> OnScrollRight = delegate { };

        public void Init(int min, int max, int centered)
        {
            _start = min;
            _end = max;
            _firstStep = centered - min;

            SetRealItemSize();
            SetLeftPadding();
            SetContainerSize();
            Invoke(nameof(GetRight), 0.1f);
        }

        void SetRealItemSize()
        {
            itemSize = GetSize((RectTransform)items[0].transform, canvas);
        }

        void SetContainerSize()
        {
            var width = (_end - _start) * (grid.cellSize.x + grid.spacing.x) + 950;
            rect.sizeDelta = new Vector2(width, 400);
        }

        void SetLeftPadding()
        {
            var scale = canvas.scaleFactor;
            var center = Screen.width / 2f;
            var size = grid.cellSize.x / 2;
            var padding = 75f;
            var leftPadding = center / scale - padding - size;
            grid.padding.left = (int)leftPadding;
        }

        void GetRight()
        {
            startingPadding = grid.padding.left;
            step = (int)(grid.cellSize.x + grid.spacing.x);
            isEnable = true;

            var firstOffset = _firstStep * (grid.cellSize.x + grid.spacing.x);
            rect.anchoredPosition = new Vector2(-firstOffset, 0);
        }

        public void Refresh(Vector2 pos)
        {
            if (!isEnable) return;

            var first = items[0];
            var last = items[^1];
            var lastPos = last.Position;

            if (lastPos.x < Screen.width && last.age < _end)
            {
                grid.padding.left += step;
                first.transform.SetSiblingIndex(items.Count);
                items.RemoveAt(0);
                items.Add(first);
                OnScrollRight(first);
                return;
            }

            var firstPos = first.Position;
            if (firstPos.x > 0 && first.age > _start)
            {
                grid.padding.left -= step;
                if (grid.padding.left < startingPadding) grid.padding.left = startingPadding;
                last.transform.SetSiblingIndex(0);
                items.RemoveAt(items.Count - 1);
                items.Insert(0, last);
                OnScrollLeft(last);
                return;
            }
        }
    }
}