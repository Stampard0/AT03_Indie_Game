using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCollect : MonoBehaviour, IInteractable
{
    [SerializeField] public static bool allCollected = false;
    
    public delegate void ObjectiveDelegate();

    private static bool active = false;

    public static event ObjectiveDelegate ObjectiveActivate = delegate { ObjectiveActivate = delegate { }; };

    public delegate void GoalDelegate();

    public static event GoalDelegate GoalActivate;
    private void Awake()
    {
        active = false;
        allCollected = false;
        GoalCounting.FinalActivate += Final;
        //GoalTotal = GoalCount;
        //ObjectiveActivate += allCollected;
    }

    public void Activate()
    {
        if (allCollected == false)
        {
            GoalActivate.Invoke();
        }
        //if (GoalCount > 3)
        //{
            
        //}
        transform.position = transform.position + new Vector3(0.0f, -5.0f, 0.0f);
    }

    private void Final()
    {
        if (active == false)
        {
            allCollected = true;
            active = true;
            ObjectiveActivate.Invoke();
        }
    }
}