using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCollect : MonoBehaviour, IInteractable
{
    public float GoalCount = 0;

    [SerializeField] public static bool allCollected = false;
    
    public delegate void ObjectiveDelegate();

    private static bool active = false;

    public static event ObjectiveDelegate ObjectiveActivate = delegate { ObjectiveActivate = delegate { }; };

    private void Awake()
    {
        active = false;
        GoalCount = 0;
        allCollected = false;
        //Objective.ObjectiveActivate += allCollected;
    }

    public void Activate()
    {
        if (allCollected == false)
        {
            GoalCount = GoalCount + 1;
        }
        if (GoalCount > 3)
        {
            allCollected = true;
            if (active == false)
            {
                active = true;
                ObjectiveActivate.Invoke();
            }
        }
        transform.position = transform.position + new Vector3(0.0f, -5.0f, 0.0f);
    }

}