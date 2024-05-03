using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public TMP_Text questText;
    private Dictionary<string, Quest> questMap;

    private void Awake()
    {
        questMap=CreateQuestMap();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;
    }
    
    private void Start()
    {
        foreach(Quest quest in questMap.Values)
        {
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.instance.questEvents.QuestStateChange(quest);
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetsRequirements = true;
        
        foreach(QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
                break;
            }
        }
        return meetsRequirements;
    }

    private void Update()
    {
        foreach(Quest quest in questMap.Values)
        {
            if(quest.state==QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
        questText.text = "Quest: " + quest.info.displayName;
    }
    private void AdvanceQuest(string id)
    {
        Quest quest=GetQuestById(id);
        //двигаемся в следущий шаг квеста
        quest.MoveToNextStep();
        //если есть ещё какие-то шаги. создаём экземпляр следующего
        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        //если больше нет шагов, мы можем закончить квест
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
            questText.text = "Quest: " + quest.info.displayName+ " Done!";
        }
    }
    private void FinishQuest(string id)
    {
        Quest quest=GetQuestById(id);
        ClaimRewards(quest, quest.info.moneyReward);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
        questText.text = "";
    }

    private void ClaimRewards(Quest quest, int moneyReward)
    {
        PlayerStats playerStats = GameObject.Find("StatManager").GetComponent<PlayerStats>();
        playerStats.AddMoney(moneyReward);
    }


    private Dictionary<string, Quest> CreateQuestMap()
    {
        //Загружаем всё в папку Assets/Resources/Quests
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        return idToQuestMap;
    }
    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if(quest == null)
        {
            Debug.LogError("ID not found in the QuestMap: " + id);
        }
        return quest;
    }
}
