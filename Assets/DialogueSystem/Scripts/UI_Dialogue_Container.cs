using System.Collections.Generic;
using UnityEngine;

public class UI_Dialogue_Container : MonoBehaviour
{



    public GameObject AnswerPanelTemplate;
    public GameObject LinePanelTemplate;



    public void DisplayAnswer(List<DialogueAnswer> answerData)
    {
        GameObject temp = Instantiate(AnswerPanelTemplate, this.transform);

        foreach (var answer in answerData) {

            Debug.Log(answer.LineData.text);

                    
        }
    }

    public void DisplayLine(DialogueLineData lineData)
    {
        GameObject temp = Instantiate(LinePanelTemplate, this.transform);

        Debug.Log(lineData.text);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
