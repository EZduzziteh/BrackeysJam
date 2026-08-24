using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialogue_Answer_Button : MonoBehaviour
{


    TextMeshProUGUI Text;
    Button Button;
    int OptionID;

    public void Awake()
    {
        Text = GetComponentInChildren<TextMeshProUGUI>();
        Button = GetComponentInChildren<Button>();
    }

    public void Initialize(DialogueAnswer answerData, int optionID)
    {
        Text.text = answerData.LineData.text;
        Button.onClick.AddListener(HandleButtonClicked);
        OptionID = optionID;
    }

    private void HandleButtonClicked()
    {
        DialogueManager.Instance.TryAdvanceDialogue(OptionID);
    }
}
