using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_OfferItem : MonoBehaviour
    {
        public CanvasGroup group;
        public TextMeshProUGUI title;
        public TA_TPoints points;
        public RawImage icon;
        public RawImage background;
        public Button playButton;
        public TA_OfferTag category;
        public TA_OfferTag difficulty;
        public TextMeshProUGUI rewardsAmount;
        [TA_ReadOnly] public int campaignID;
        [TA_ReadOnly] public string packageName;
        public int ID;
        public event Action<TA_OfferItem> OnPlay = delegate { };
        public Vector3 Position => transform.position;

        public void Show()
        {
            gameObject.SetActive(true);
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }

        void OnEnable()
        {
            playButton.onClick.AddListener(Play);
        }

        void OnDisable()
        {
            playButton.onClick.RemoveListener(Play);
        }

        void Play()
        {
            OnPlay(this);
        }

        public void SetCampaignID(int id)
            => campaignID = id;

        public void SetPackage(string id)
            => packageName = id;

        public void SetName(string msg)
        {
            name = msg;

            if (msg.Length > 22)
                msg = msg[..22] + "...";

            title.text = msg;
        }

        public void SetTotalRewards(int amount)
            => rewardsAmount.text = "+" + amount + " Rewards";

        public void SetPoints(double amount, string type)
        {
            // var data = TA_TPoint.ConvertTPoints(amount);
            // points.SetPoints(data.amount.ToString(data.format) + data.suffix + " " + type);
            points.SetPoints(amount, type);
        }

        public TextMeshProUGUI currencyName;

        public void SetCurrencyName(string type)
        {
            if (currencyName)
                currencyName.text = type;
        }

        public void SetCategory(string msg)
        {
            category.Set(msg);
        }

        public void SetDifficulty(string msg)
            => difficulty.Set(msg);

        public void SetIcon(Texture texture)
            => icon.texture = texture;

        public void SetPreview(Texture texture)
        {
            background.texture = texture;
        }
    }
}