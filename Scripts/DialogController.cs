using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogController : MonoBehaviour
{
    [SerializeField] GameObject box;
    [SerializeField] Text text;
    [SerializeField] int lps = 2;

    public event Action onShow;
    public event Action onHide;
    int currLine = 0;
    Dialog dialog;
    bool isTyping;

    public static DialogController instance
    {
        get; private set;
    }

    // this becomes a public instance (singleton)
    private void Awake()
    {
        instance = this;
    }
    public IEnumerator Show(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        onShow?.Invoke();
        this.dialog = dialog;
        box.SetActive(true);
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyUp(KeyCode.E) && !isTyping) 
        { 
            if (currLine < dialog.DialogLines.Count) 
            {
                StartCoroutine(type(dialog.DialogLines[currLine]));
                ++currLine;
            }
            else
            {
                box.SetActive(false);
                currLine = 0;
                onHide?.Invoke();
            }
        }
    }

    // letter by letter print function
    public IEnumerator type(string line)
    {
        text.text = "";
        isTyping = true;
        foreach(var letter in line.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(1f / lps);
        }
        isTyping=false;
    }
}
