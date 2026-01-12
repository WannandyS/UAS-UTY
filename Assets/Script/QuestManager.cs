using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class QuestItem
{
    public List<string> messages = new();
    public List<string> fallbacks = new();
    public List<string> completes = new();
    public UnityEvent OnQuestMessageFinish;
    public UnityEvent OnQuestCompleteMessageFinish;
}

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject canvas;

    public List<QuestItem> quests = new();

    private List<int> checkmark = new();
    private int completedId = 0;
    private Coroutine _questCoroutine;

    public TypewriterEffect effect;
    private int messageIndex = 0;

    public void QuestComplete()
    {
        checkmark[completedId] = 2;
    }

    public void QuestUncomplete()
    {
        if (checkmark[completedId] == 2)
            checkmark[completedId] = 1;
    }

    void Awake()
    {
        checkmark.Clear();
        checkmark.AddRange(Enumerable.Repeat(0, quests.Count));
    }
    
    public void ShowQuest(bool isDelay)
    {
        if (_questCoroutine != null)
            StopCoroutine(_questCoroutine);
        _questCoroutine = StartCoroutine(ShowQuestEnumerator(isDelay));
    }

    public IEnumerator ShowQuestEnumerator(bool isDelay)
    {
        if (isDelay)
            yield return new WaitForSeconds(1);

        canvas.SetActive(true);
        switch (checkmark[completedId])
        {
            case 0:
                effect.Run(quests[completedId].messages[0]);
                break;
            case 1:
                effect.Run(quests[completedId].fallbacks[0]);
                break;
            case 2:
                effect.Run(quests[completedId].completes[0]);
                break;
        }
    }

    public void NextMessage()
    {
        List<string> m = new();
        int status = checkmark[completedId];

        if (status == 0) m = quests[completedId].messages;
        else if (status == 1) m = quests[completedId].fallbacks;
        else if (status == 2) m = quests[completedId].completes;


        if (++messageIndex < m.Count)
        {
            canvas.SetActive(true);
            Debug.Log("Complete ID: " + completedId.ToString() + " " + messageIndex.ToString());
            effect.Run(m[messageIndex]);
        }
        else
        {
            messageIndex = 0;
            canvas.SetActive(false);
            if (status == 0)
            {
                if (quests[completedId].fallbacks.Count > 0)
                    checkmark[completedId] = 1;
                quests[completedId].OnQuestMessageFinish?.Invoke();
            }
            else if (status == 2)
            {
                var old = completedId;
                if (completedId + 1 < quests.Count)
                    completedId++;
                quests[old].OnQuestCompleteMessageFinish?.Invoke();
            }
        }
    }
}
