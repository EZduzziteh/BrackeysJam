using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Dialogue_Container : MonoBehaviour
{
    public static UI_Dialogue_Container Instance;
    public GameObject AnswerPanelTemplate;
    public GameObject LinePanelTemplate;

    public void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }


        AnswerPanelTemplate.SetActive(false);
        LinePanelTemplate.SetActive(false);


    }

    public void DisplayAnswer(DialogueStage_Answer answerData)
    {
        GameObject temp = Instantiate(AnswerPanelTemplate, this.transform);
        temp.SetActive(true);

        if(temp.TryGetComponent<UI_Dialogue_Answer_Panel>(out var answerPanel))
        {
            answerPanel.Initialize(answerData);
        }

      
    }

    public void DisplayLine(DialogueStage_Line lineData)
    {
        GameObject temp = Instantiate(LinePanelTemplate, this.transform);
        temp.SetActive(true);
        if (temp.TryGetComponent<UI_Dialogue_Line_Panel>(out var linePanel)) {

            linePanel.Initialize(lineData);
        }
    }

    
    
}
