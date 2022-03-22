using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public Action action;

    public Node(Node parent, float cost, Dictionary<string, int> allstates, Action action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates);
        this.action = action;
    }
}

public class Planner : MonoBehaviour
{
    //returning ready plan (queue of actions)
    public Queue<Action> plan(List<Action> actions, Dictionary<string, int> goal, WorldStates states)
    {
        List<Action> usableActions = new List<Action>();
        foreach(Action a in actions)
        {
            if(a.IsAchievable())
                usableActions.Add(a);
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, World.Instance.GetWorld().GetAllStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if(!success)
        {
            Debug.Log("NO PLAN");
            return null;
        }

        //if there is a plan then find 'cheapest'
        Node cheapest = leaves[0];
        foreach(Node leaf in leaves)
        {
            if(leaf.cost < cheapest.cost)
                cheapest = leaf;
        }

        List<Action> result = new List<Action>();
        Node n = cheapest;
        while(n != null)
        {
            if(n.action != null)
            {
                result.Insert(0, cheapest.action);
            }
            n = n.parent;
        }

        Queue<Action> queue = new Queue<Action>();
        foreach(Action a in result)
        {
            queue.Enqueue(a);
        }

        Debug.Log("Started plan : ");
        foreach(Action a in queue)
        {
            Debug.Log("Action: " + a.actionName);
        }

        return queue;

    }

    bool BuildGraph(Node parent, List<Node> leaves, List<Action> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;

        foreach(Action action in usableActions)
        {
            //checking precondition matching
            if(action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                
                //check effects
                foreach(KeyValuePair<string, int> ef in action.effects)
                {
                    if(!currentState.ContainsKey(ef.Key))
                        currentState.Add(ef.Key, ef.Value);
                }

                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if(GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<Action> subset = ActionSubset(usableActions, action); //reduce by already used action
                    //recursion
                    foundPath = BuildGraph(node, leaves, subset, goal);
                }
            }
        }


        return foundPath;
    }

    bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string, int> g in goal)
        {
            if(!state.ContainsKey(g.Key))
                return false;
        }
        return true;
    }

    List<Action> ActionSubset(List<Action> actions, Action action)
    {
        var subset = new List<Action>();
        foreach(Action a in actions)
        {
            if(!a.Equals(action))
                subset.Add(a);
        }

        return subset;
    }

}
