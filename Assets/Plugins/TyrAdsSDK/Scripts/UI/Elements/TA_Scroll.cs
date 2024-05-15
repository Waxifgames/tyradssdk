using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_Scroll : MonoBehaviour
    {
        [SerializeField] protected Canvas canvas;
        [SerializeField] protected GridLayoutGroup grid;
        [SerializeField] protected int step;
        [SerializeField] protected bool isEnable;
        protected RectTransform rect;
        public int startingPadding;
        [FormerlySerializedAs("realItemSize")]
        public Vector3 itemSize;

        void Awake()
        {
            if (!rect) rect = (RectTransform)grid.transform;
        }

        protected Vector3 GetSize(RectTransform rt, Canvas c)
        {
            var realWorldWidth = rt.rect.width * c.scaleFactor;
            var realWorldHeight = rt.rect.height * c.scaleFactor;
            return new Vector3(realWorldWidth, realWorldHeight, 0f);
        }
    }
}