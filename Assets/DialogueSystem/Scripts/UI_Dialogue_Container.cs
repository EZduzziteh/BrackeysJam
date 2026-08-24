using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Dialogue_Container : MonoBehaviour
{
    public static UI_Dialogue_Container Instance;
    public GameObject AnswerPanelTemplate;
    public GameObject LinePanelTemplate;
    public GameObject TextPromptTemplate;

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
        TextPromptTemplate.SetActive(false);
    }

    public void DisplayPrompt()
    {
        DialogueManager.Instance.SetAwaitingUser(false);
        GameObject temp = Instantiate(TextPromptTemplate, transform);
        temp.SetActive(true);
        
    }
    public void DisplayAnswer(DialogueStage_Answer answerData)
    {

        DialogueManager.Instance.SetAwaitingUser(false);
        Clear();

        GameObject temp = Instantiate(AnswerPanelTemplate, this.transform);
        temp.SetActive(true);

        if(temp.TryGetComponent<UI_Dialogue_Answer_Panel>(out var answerPanel))
        {
            answerPanel.Initialize(answerData);
        }

      
    }

    public void DisplayLine(DialogueStage_Line lineData)
    {
        DialogueManager.Instance.SetAwaitingUser(false);
        Clear();
        GameObject temp = Instantiate(LinePanelTemplate, this.transform);
        temp.SetActive(true);
        if (temp.TryGetComponent<UI_Dialogue_Line_Panel>(out var linePanel)) {

            linePanel.Initialize(lineData);
        }
    }


    public void Clear()
    {
        List<GameObject> childrenToDestroy = new List<GameObject>();
        for(int i = 0; i < transform.childCount; i++)
        {
            if(
                transform.GetChild(i).gameObject != AnswerPanelTemplate &&
                transform.GetChild(i).gameObject != LinePanelTemplate &&
                transform.GetChild(i).gameObject != TextPromptTemplate)
            {
                childrenToDestroy.Add(transform.GetChild(i).gameObject);
            }
        }

        foreach(var c in childrenToDestroy)
        {
            Destroy(c.gameObject);
        }
    }
}
