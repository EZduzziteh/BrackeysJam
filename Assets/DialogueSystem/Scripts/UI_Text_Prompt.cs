using UnityEngine;
using TMPro;
public class UI_Text_Prompt : MonoBehaviour
{
    TextMeshProUGUI Text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Text = GetComponentInChildren<TextMeshProUGUI>();
    }

}
