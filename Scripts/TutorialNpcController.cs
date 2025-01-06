using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNpcController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    public void Interact()
    {
        // debug 
        //Debug.DrawLine(transform.position, transform.position, Color.red, 1f);

        StartCoroutine(DialogController.instance.Show(dialog));
    }
}
