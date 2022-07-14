using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private float distance = 2.5f;
    [SerializeField] private bool debug = false;
    [SerializeField] private LayerMask interactionMask;

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance, interactionMask) == true)
        {
            GameHUD.Instance.SetCrosshairColour(Color.green);
            if (debug == true) { Debug.DrawRay(transform.position, transform.forward * distance, Color.blue, 0.3f); }
            if (Input.GetButtonDown("Use") == true)
            {
                if (hit.transform.TryGetComponent(out IInteractable interaction) == true)
                {
                    interaction.Activate();
                }
            }
        }
        else
        {
            GameHUD.Instance.SetCrosshairColour(Color.red);
            if (debug == true) { Debug.DrawRay(transform.position, transform.forward * distance, Color.red, 0.3f); }
        }
    }
}
public interface IInteractable
{
    void Activate();
}