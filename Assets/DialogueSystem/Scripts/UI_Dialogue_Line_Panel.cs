using TMPro;
using UnityEngine;

public class UI_Dialogue_Line_Panel : MonoBehaviour
{

    TextMeshProUGUI Text;

    private void Awake()
    {
        Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Initialize(DialogueStage_Line lineData)
    {
        Text.text = lineData.LineData.text;
    }
}
