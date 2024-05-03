using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private bool playerIsNear=false;

    private string questId;
    private QuestState currentQuestState;
    private QuestIcon questIcon;

    private void Awake()
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<QuestIcon>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
       // GameEventsManager.instance.inputEvents.onSubmitPressed += SubmitPressed;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        //GameEventsManager.instance.inputEvents.onSubmitPressed -= SubmitPressed;
    }

    private void SubmitPressed(InputAction.CallbackContext context)
    {
        if (!playerIsNear)
        {
            return;
        }
        if (context.started)
        {
            //Начинаем или заканчиваем квест
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                GameEventsManager.instance.questEvents.StartQuest(questId);
            }
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                GameEventsManager.instance.questEvents.FinishQuest(questId);
            }
            //GameEventsManager.instance.questEvents.StartQuest(questId);
            //GameEventsManager.instance.questEvents.AdvanceQuest(questId);
            //GameEventsManager.instance.questEvents.FinishQuest(questId);
        }
        //GameEventsManager.instance.questEvents.StartQuest(questId);
        //GameEventsManager.instance.questEvents.AdvanceQuest(questId);
        //GameEventsManager.instance.questEvents.FinishQuest(questId);
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState, startPoint, finishPoint);
            //Debug.Log("Quest with id: " + questId + " update to state: " + currentQuestState);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.CompareTag("Player"))
        {
            playerIsNear = true;
            otherCollider.GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay");
            otherCollider.GetComponent<PlayerInput>().currentActionMap["Submit"].started += SubmitPressed;
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = false;
            otherCollider.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            otherCollider.GetComponent<PlayerInput>().currentActionMap["Submit"].started -= SubmitPressed;
        }
    }
}
