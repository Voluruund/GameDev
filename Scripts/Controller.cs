using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Roaming, Dialog, Menu
}

public class Controller : MonoBehaviour
{
    // game logic for interactable objs and walking
    // var visible in inspector
    [SerializeField] PlayerController controller;

    State state;

    private void Start()
    {
        DialogController.instance.onShow += () =>
        {
            state = State.Dialog;
        };
        DialogController.instance.onHide += () =>
        {
            if (state == State.Dialog) {
                state = State.Roaming;
            }
        };
    }

    private void Update()
    {
        if (state == State.Roaming)
        {
            controller.HandleUpdate();
        }
        else if (state == State.Dialog)
        {
            DialogController.instance.HandleUpdate();
        }
        else if (state == State.Menu) 
        {

        }
    }
}
