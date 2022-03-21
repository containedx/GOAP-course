using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target; //place when action takes place
    public GameObject targetTag;
    public float duration = 0;

    public WorldState[] preconditionsStates;
    public WorldState[] effectsStates;

    public NavMeshAgent agent;

    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    public WorldStates agentBeliefs;

    public bool running = false;


    public Action()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();

        //take preconditions and effects from WorlStates
        if(preconditionsStates != null)
        {
            foreach(WorldState w in preconditionsStates)
            {
                preconditions.Add(w.key, w.value);
            }
        }
        if(effectsStates != null)
        {
            foreach(WorldState w in effectsStates)
            {
                effects.Add(w.key, w.value);
            }
        }
    }

    // Can we do it?
    public bool IsAchievable()
    {
        return true;
    }

    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach(var p in preconditions)
        {
            if(!conditions.ContainsKey(p.Key))
                return false;
        }
        return true;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();

}
