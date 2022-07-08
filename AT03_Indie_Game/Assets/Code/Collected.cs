using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Collected : MonoBehaviour, IInteractable
{
    private Vector3 collecedPosition;

    void Start()
    {
        collecedPosition = new Vector3(0.0f, -5.0f, 0.0f);
    }
    public void Activate()
    {
        transform.position = transform.position + new Vector3(0.0f, -5.0f, 0.0f);
    }
}
