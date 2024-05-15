using TMPro;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_UserAge : MonoBehaviour
    {
        public TextMeshProUGUI txt;
        public int age;

        public void SetAge(int id)
        {
            age = id;
            txt.text = id.ToString();
            name = "Age - " + age;
        }

        public Vector3 Position => transform.position;

        public void Active(Color color)
        {
            txt.color = color;
            txt.fontStyle = FontStyles.Bold;
            txt.fontSize = 66;
        }

        public void Passive(Color color)
        {
            txt.color = color;
            txt.fontStyle = FontStyles.Normal;
            txt.fontSize = 66;
        }
    }
}