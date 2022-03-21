using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteraction1 : MonoBehaviour, IInteractable
{
    #region boolean
    private bool exampleBool;

    public bool ExampleBool
    {
        get { return exampleBool; }
    }
    #endregion

    public delegate void InteractionDelegate();

    public InteractionDelegate interactionEvent = delegate { };

    private void OnEnable()
    {
        interactionEvent += TestMethod;
        interactionEvent += TestMethodTwo;
    }

    private void OnDisable()
    {
        interactionEvent -= TestMethod;
        interactionEvent -= TestMethodTwo;
    }

    public void Activate()
    {
        interactionEvent.Invoke();
    }

    private void TestMethod()
    {
        Debug.Log("First method has been executed.");
    }

    private void TestMethodTwo()
    {
        Debug.Log("Second method has been executed.");
    }
}
