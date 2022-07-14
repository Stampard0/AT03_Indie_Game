using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour, IInteractable
{
    public AudioSource AudioSource { get; private set; }
    public bool GoalActive { get; private set; }
    [SerializeField] private AudioClip finishClip;
    private void Awake()
    {
        //if (TryGetComponent(out AudioSource aSrc) == true)
        //{
        //    Instance = Instance;
        //}
        GoalCollect.ObjectiveActivate += Excape;
        TryGetComponent(out AudioSource aSrc);
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
        }
    }
}
