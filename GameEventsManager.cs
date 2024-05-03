using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }
    public InputEvents inputEvents;
    public QuestEvents questEvents;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one GameEventsManager in the scene");
        }
        instance=this;

        inputEvents = new InputEvents();
        questEvents = new QuestEvents();
    }
}
