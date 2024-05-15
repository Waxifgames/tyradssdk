using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable PossibleLossOfFraction

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_ScrollAge : MonoBehaviour
    {
        [SerializeField] TA_ScrollHorizontal scroll;
        public ScrollRect scrollRect;
        [SerializeField] int startAge = 13;
        [SerializeField] int endAge = 100;
        [SerializeField] int centeredAge = 18;
        [SerializeField] Color chosenColor;
        [SerializeField] Color normalColor;
        [SerializeField] RectTransform centeredSquare;

        [Space(20)]
        [TA_ReadOnly] public int chosenAge = 18;
        [TA_ReadOnly] public TA_UserAge centered;
        [SerializeField] [TA_ReadOnly] float rightCorner;
        [SerializeField] [TA_ReadOnly] float leftCorner;
        [TA_ReadOnly] [SerializeField] List<TA_UserAge> ages = new();

        void Awake()
        {
            Invoke(nameof(Init), 0.2f);
        }

        void Start()
        {
            scroll.Init(startAge, endAge, centeredAge);
        }

        void RefreshLast(TA_UserAge obj)
        {
            var age = obj.GetComponent<TA_UserAge>();
            var previous = scroll.items[^2].GetComponent<TA_UserAge>();
            age.SetAge(previous.age + 1);
        }

        void RefreshFirst(TA_UserAge obj)
        {
            var age = obj.GetComponent<TA_UserAge>();
            var previous = scroll.items[1].GetComponent<TA_UserAge>();
            age.SetAge(previous.age - 1);
        }

        public void Refresh(Vector2 p)
        {
            foreach (var age in ages)
            {
                var pos = age.Position;

                if (pos.x < leftCorner ||
                    pos.x > rightCorner)
                {
                    continue;
                }

                centered = age;
                chosenAge = centered.age;
                break;
            }

            foreach (var age in ages)
                age.Passive(normalColor);

            if (centered)
            {
                if (centeredSquare.parent != centered.transform)
                {
                    centeredSquare.SetParent(centered.transform);
                    centeredSquare.anchoredPosition = Vector2.zero;
                    centeredSquare.transform.SetSiblingIndex(0);
                }

                centered.Active(chosenColor);
            }
        }

        void Init()
        {
            rightCorner = Screen.width / 2 + scroll.itemSize.x * 2f;
            leftCorner = Screen.width / 2 - scroll.itemSize.x / 2;

            for (var i = 0; i < scroll.items.Count; i++)
            {
                var age = scroll.items[i];
                if (!age) continue;
                ages.Add(age);
                age.SetAge(startAge + i);
            }

            scroll.OnScrollRight += RefreshLast;
            scroll.OnScrollLeft += RefreshFirst;

            Refresh(Vector2.zero);
        }
    }
}