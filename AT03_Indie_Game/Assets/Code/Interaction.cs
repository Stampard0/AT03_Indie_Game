using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] public float distance = 2.5f;
    [SerializeField] private LayerMask interactionMask;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Use") == true)
        {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance) == true)
            {
                if(hit.transform.TryGetComponent(out IInteractable interaction) == true)
                {
                    Debug.DrawRay(transform.position, transform.forward * distance, Color.blue, 0.3f);
                    interaction.Activate();
                }
                else
                {
                    Debug.DrawRay(transform.position, transform.forward * distance, Color.red, 0.3f);
                }
            }
        }
    }
}

public interface IInteractable
{
    void Activate();
}