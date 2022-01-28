using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragModifier : MonoBehaviour
{
    public float modifier;

    void OnTriggerEnter2D(Collider2D col) {
        PlayerMovement controller = col.GetComponent<PlayerMovement>();
        if(controller != null) {
            controller.ApplyDragModifier(modifier);
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        PlayerMovement controller = col.GetComponent<PlayerMovement>();
        if(controller != null) {
            controller.ApplyDragModifier(1.0f/modifier);
        }
    }
}
