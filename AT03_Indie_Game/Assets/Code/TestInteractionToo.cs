using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractionToo : MonoBehaviour, IInteractable
{
    public TestInteraction interaction;

    public void Activate()
    {
        Debug.Log("The interaction is currently turned on: " + interaction.ExampleBool);
        interaction.Activate();
    }
}
