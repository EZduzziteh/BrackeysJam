using UnityEditor;
using UnityEngine;

public class UI_Dialogue_Answer_Panel : MonoBehaviour
{

    public GameObject AnswerButtonTemplate;
    private void Awake()
    {
        AnswerButtonTemplate.SetActive(false);
    }

    public void Initialize(DialogueStage_Answer answerData)
    {
        for(int i = 0; i < answerData.answers.Count; i++)
        {
            AddAnswerButton(answerData.answers[i], i);
        }
    }

    public void AddAnswerButton(DialogueAnswer answerData, int optionID)
    {
        GameObject temp = Instantiate(AnswerButtonTemplate, transform);
        temp.SetActive(true);
        if(temp.TryGetComponent<UI_Dialogue_Answer_Button>(out var answerButton))
        {
            answerButton.gameObject.SetActive(true);
            answerButton.Initialize(answerData, optionID);
        }

    }
}
