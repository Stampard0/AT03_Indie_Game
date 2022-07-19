using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour, IInteractable
{
    public delegate void VictoryDelegate();
    public static event VictoryDelegate VictoryEvent = delegate { };
    public AudioSource AudioSource { get; private set; }
    public bool GoalActive { get; private set; }
    public AudioClip finishClip;
    private void Awake()
    {
        if (TryGetComponent(out AudioSource aSrc) == true)
        {
            AudioSource = aSrc;
        }
        GoalCollect.ObjectiveActivate += Excape;
        //TryGetComponent(out AudioSource aSrc);
        GoalCollect.ObjectiveActivate += delegate { GoalActive = true; };
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
            Debug.Log("Thanks for Playing");
            AudioSource.PlayOneShot(finishClip);
            VictoryEvent.Invoke();
            GameHUD.Instance.ActivateEndPrompt(true);
        }
    }
}
