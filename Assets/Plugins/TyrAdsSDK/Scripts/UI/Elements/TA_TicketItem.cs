using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_TicketItem : MonoBehaviour
    {
        [TA_ReadOnly] public int ID;
        [TA_ReadOnly] public int campaignID;
        [SerializeField] Color completeColor;

        [Space(20)]
        [SerializeField] Button button;

        [Space(10)]
        [SerializeField] Image checkBoxMark;
        [SerializeField] Image completeMark;
        [SerializeField] Image checkBoxBack;

        [Space(10)]
        [SerializeField] Image back;
        [SerializeField] Image backDisabled;

        [Space(10)]
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TA_TPoints points;

        bool _isSelected;
        bool _isSelectionAllowed = true;

        public event Action<TA_TicketItem> OnSelected = delegate { };
        public event Action<TA_TicketItem> OnDeselected = delegate { };

        void Start()
        {
            Refresh();
        }

        void OnEnable()
        {
            button.onClick.AddListener(Switch);
        }

        public void AllowSelection()
        {
            _isSelectionAllowed = true;
            checkBoxBack.enabled = false;
            back.enabled = true;
            backDisabled.enabled = false;
            button.interactable = true;
        }

        public void DisableSelection()
        {
            _isSelectionAllowed = false;
            checkBoxBack.enabled = true;
            back.enabled = false;
            backDisabled.enabled = true;
            button.interactable = false;
        }

        void OnDisable()
        {
            button.onClick.RemoveListener(Switch);
        }

        public void SetID(int id) => ID = id;
        public void SetCampaignID(int id) => campaignID = id;
        public void SetName(string msg)
        {
            if (msg.Length > 26)
                msg = msg[..26] + "...";
            title.text = msg;
        }

        public void SetPoints(double msg) => points.SetPoints(msg, "TPoints");

        public void SetCompletion(bool isComplete)
        {
            title.color = isComplete ? completeColor : Color.black;
            completeMark.enabled = isComplete;
        }

        void Switch()
        {
            if (!_isSelectionAllowed) return;
            _isSelected = !_isSelected;
            Refresh();
        }

        void Refresh()
        {
            checkBoxMark.enabled = _isSelected;
            if (_isSelected) OnSelected(this);
            else OnDeselected(this);
        }
    }
}