using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Linq;


public class SubGoal
{
    public Dictionary<string, int> goals;
    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        goals = new Dictionary<string, int>();
        goals.Add(s,i);
        remove = r;
    }
}

public class Agent : MonoBehaviour
{
    public List<Action> actions = new List<Action>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    Planner planner;

    Queue<Action> actionQueue;
    public Action currentAction;
    SubGoal currentGoal;


    void Start()
    {
        //get all actions
        Action[] act = GetComponents<Action>();
        foreach(Action a in act)
            actions.Add(a);

    }

    void LateUpdate()
    {
        
    }

}
