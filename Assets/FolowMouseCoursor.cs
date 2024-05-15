using UnityEngine;
using UnityEngine.UI;

public class FolowMouseCoursor : MonoBehaviour
{
    public Canvas myCanvas;
    public Transform coursor;
    public Vector3 offset;
    public Image img;
    void Update()
    {
        RectTransformUtility
            .ScreenPointToLocalPointInRectangle(
            myCanvas.transform as RectTransform, Input.mousePosition,
            myCanvas.worldCamera,
            out Vector2 pos);
        coursor.position = myCanvas.transform.TransformPoint(pos) + offset;


        if (Input.GetMouseButton(0))
        {
            img.color = Color.green;
            okay = true;
            flashTimer = flastTime;
        }

        if (okay && flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0)
            {
                okay = false;
                img.color = Color.white;
            }
        }
    }

    bool okay;
    public float flastTime = 0.1f;
    float flashTimer;
}
