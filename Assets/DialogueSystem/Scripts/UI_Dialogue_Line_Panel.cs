using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UI_Dialogue_Line_Panel : MonoBehaviour
{
    public float baseTextInterval = 0.05f;

    float currentTextInterval;
    string message;

    int currentIndex;

    TextMeshProUGUI Text;
    Coroutine typingCoroutine;

    public UnityEvent OnTypingComplete;

    private void Awake()
    {
        Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        OnTypingComplete = new UnityEvent();
        OnTypingComplete.AddListener(HandleTypingComplete);
    }

    private void HandleTypingComplete()
    {
        //prompt user to hit enter.
        UI_Dialogue_Container.Instance.DisplayPrompt();
        DialogueManager.Instance.SetAwaitingUser(true);
        DialogueManager.Instance.HandleTypingComplete();
    }

    public void SkipText()
    {
        if (typing) {
            currentTextInterval = 0.001f;
        }
    }

    public void Initialize(DialogueStage_Line lineData)
    {
        Text.text = "";
        message = lineData.LineData.text;
        typingCoroutine = StartCoroutine(TypeText());
        currentTextInterval = baseTextInterval;
    }
    public bool typing = false;
    private IEnumerator TypeText()
    {
        typing = true;
        currentTextInterval = baseTextInterval;

        int index = currentIndex; // snapshot index

        if (!Text)
        {
            Debug.Log("no tmp");
            yield break;
        }

        Text.text = "";

        float soundCooldown = 0.03f;
        float lastSoundTime = 0;

        foreach (char c in message)
        {
            Text.text += c;

            if (!char.IsWhiteSpace(c) && Time.time - lastSoundTime > soundCooldown)
            {
                //play sound effect

                lastSoundTime = Time.time;
            }

            yield return new WaitForSeconds(currentTextInterval);
        }

        currentTextInterval = baseTextInterval;

        yield return new WaitForSeconds(0);
        typing = false;
        OnTypingComplete?.Invoke();
    }

}
