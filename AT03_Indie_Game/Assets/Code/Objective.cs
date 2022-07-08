using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour, IInteractable
{
    public bool GoalActive { get; private set; }
    
    private void Awake()
    {
        GoalCollect.ObjectiveActivate += Excape;
    }

    private void Excape()
    {
        if (GoalActive == false)
        {
            GoalActive = true;
        }
    }

    public void Activate()
    {
        if(GoalActive == true)
        {
            //Thanks for Playing
        }
    }
}
