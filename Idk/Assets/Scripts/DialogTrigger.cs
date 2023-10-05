using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;

    public void StartDialogue() 
    {
        FindObjectOfType<DialogManager>().StartDialog(dialog);
        gameObject.SetActive(false);
    }
   
}
