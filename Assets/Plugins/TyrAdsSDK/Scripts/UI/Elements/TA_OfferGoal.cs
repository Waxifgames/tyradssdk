using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Plugins.TyrAdsSDK.Scripts.Caching.TA_Extensions;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_OfferGoal : MonoBehaviour
    {

        #region Inspector

        [TA_ReadOnly] public int ID;
        [TA_ReadOnly] public bool IsComplete;

        [Space(20)]
        [SerializeField] TA_TPoints points;
        [SerializeField] TextMeshProUGUI difficulty;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Image currentGoalFrame;

        [Space(20)]
        [SerializeField] GameObject footer;
        [SerializeField] TextMeshProUGUI footerText;
        [SerializeField] Image footerBackground;
        [SerializeField] Color completeColor;
        [SerializeField] Color basicColor;
        [SerializeField] Color pendingColor;
        [SerializeField] Color pausedColor;
        [SerializeField] Color expiredColor;
        [SerializeField] Color rejectedColor;

        [Space(20)]
        [SerializeField] Image icon;
        [SerializeField] Sprite basicIcon;
        [SerializeField] Sprite completeIcon;

        [Space(20)]
        [SerializeField] string footerAvailableMsg;
        [SerializeField] string footerNotAvailableMsg;
        [SerializeField] string footerCompleteMsg;
        [SerializeField] string footerRejectedMsg;
        [SerializeField] string footerPendingMsg;
        [SerializeField] string footerExpiredMsg;
        [SerializeField] string footerPausedMsg;

        #endregion



        public void SetID(int id) => ID = id;
        public void SetName(string msg) => title.text = msg;
        public void SetPoints(double amount, string type) => points.SetPoints(amount, type);
        public void SetRemainingTime(int timeRemain, string timeType) => footerText.text = $"Complete within <b>{timeRemain:D2} {timeType}</b>";

        public void SetRemainingTime(int secondsRemain)
        {
            if (secondsRemain <= 0) return;
            var timer = ConvertToString(secondsRemain);
            footerText.text = $"Complete within <b>{timer}</b>";
        }


        public void SetCategory(string msg)
        {
            if (!difficulty) return;
            if (msg.Equals("High")) msg = "Hard";
            difficulty.text = msg;
        }


        #region States

        public void SetStatus(string conversionStatus, bool isCurrent)
        {
            if (IsApproved(conversionStatus))
            {
                Complete();
            }
            else
            {
                if (IsRejected(conversionStatus))
                {
                    Rejected();
                }
                else
                {
                    if (IsPending(conversionStatus))
                        Pending();
                    else
                        Basic(isCurrent);
                }
            }
        }

        [Button]
        void Basic(bool isCurrent)
        {
            NotComplete();
            if (footerBackground) footerBackground.color = basicColor;
            if (isCurrent) currentGoalFrame.enabled = true;
            if (footerText)
                footerText.text = isCurrent ? footerNotAvailableMsg : footerNotAvailableMsg;
        }

        [Button]
        void Pending()
        {
            NotComplete();
            if (footerBackground) footerBackground.color = pendingColor;
            if (footerText) footerText.text = footerPendingMsg;
        }

        [Button]
        void Rejected()
        {
            NotComplete();
            if (footerBackground) footerBackground.color = rejectedColor;
            if (footerText) footerText.text = footerRejectedMsg;
        }

        void NotComplete()
        {
            IsComplete = false;
            if (currentGoalFrame) currentGoalFrame.enabled = false;
            if (icon) icon.sprite = basicIcon;
        }

        [Button]
        void Expired()
        {
            NotComplete();
            if (footerBackground) footerBackground.color = expiredColor;
            if (footerText) footerText.text = footerExpiredMsg;
        }

        [Button]
        void Complete()
        {
            IsComplete = true;
            if (footerBackground) footerBackground.color = completeColor;
            if (currentGoalFrame) currentGoalFrame.enabled = false;
            if (footerText) footerText.text = footerCompleteMsg;
            if (icon) icon.sprite = completeIcon;
        }

        #endregion


        public void SetComplete(bool isComplete)
        {
            IsComplete = isComplete;
            if (IsComplete)
                Complete();
            else
                Basic(false);
        }


        bool IsApproved(string status) => status.Equals("approved");
        bool IsRejected(string status) => status.Equals("rejected");
        bool IsPending(string status) => status.Equals("pending");
        bool IsEmpty(string status) => status.Equals(string.Empty) || status.Equals("null");
        public void Available() => footerText.text = "Complete this to continue";
        public void NotAvailable() => footerText.text = "Not available yet";
        public void NoCompleted() => footerText.text = "Not completed yet";

    }
}