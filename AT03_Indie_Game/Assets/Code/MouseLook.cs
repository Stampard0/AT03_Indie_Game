using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 2.5f;
    public float drag = 1.5f;

    private Transform character; // references character transform
    private Vector2 mouseDirection; // stores cursor coordinates
    private Vector2 smoothing; // smoothed cursor movement
    private Vector2 result; // cursor end position

    private void Awake()
    {
        character = transform.parent; // get reference to parent's transform
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        mouseDirection = new Vector2(Input.GetAxisRaw("Mouse X") * sensitivity, Input.GetAxisRaw("Mouse Y") * sensitivity); // calculate mouse direction
        smoothing = Vector2.Lerp(smoothing, mouseDirection, 1 / drag); // calculate smoothing
        result += smoothing; // add smoothing to result
        result.y = Mathf.Clamp(result.y, -80, 80); // limits y angle

        transform.localRotation = Quaternion.AngleAxis(-result.y, Vector3.right); //apply x axis rotation to camera
        character.rotation = Quaternion.AngleAxis(result.x, character.up); // apply y rotation to character
    }
}
