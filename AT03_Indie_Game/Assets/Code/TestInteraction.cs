using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteraction : MonoBehaviour, IInteractable
{
    #region boolean
    private bool exampleBool;

    public bool ExampleBool
    {
        get { return exampleBool; }
    }
    #endregion
    public void Activate()
    {
        exampleBool = !exampleBool;
        if(exampleBool == true)
        {
            Debug.Log("Case a");
        }
        else
        {
            Debug.Log("Case z");
        }
    }
}
