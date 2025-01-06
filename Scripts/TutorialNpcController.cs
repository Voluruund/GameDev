using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNpcController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    public void Interact()
    {
        StartCoroutine(DialogController.instance.Show(dialog));
    }
}
