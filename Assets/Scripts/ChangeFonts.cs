using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class ChangeFonts : MonoBehaviour
{
    public TMP_FontAsset targetFont; // The font you want to change to
    public bool change;

    void Update()
    {
        if (!change) return;
        change = false;
        ChangeAllFonts();
    }

    void ChangeAllFonts()
    {
        // Find all Text components in the scene
        var allTexts = FindObjectsOfType<TextMeshProUGUI>();

        // Iterate through each Text component and change its font
        foreach (var text in allTexts)
        {
            text.font = targetFont;
        }
    }
}