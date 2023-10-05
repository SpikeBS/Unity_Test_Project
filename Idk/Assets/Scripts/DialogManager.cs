using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text dialogText;
    public Text nameText;
    public GameObject dbox;
    public GameObject Player;
    RectTransform rt;

    [SerializeField] Animator blockOpen;

    Queue<string> sentences;

    private void Start()
    {       
        sentences = new Queue<string>();
        rt = dbox.GetComponent<RectTransform>();

    }
    public void StartDialog(Dialog dialog) 
    {
        Debug.Log("dd");
        GlobalEventManager.SendPlayerStop(); 
        dbox.SetActive(true);
        nameText.text = dialog.name;
        sentences.Clear();

        foreach (var sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence() 
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void EndDialog()
    {
        GlobalEventManager.SendPlayerActive();

        dbox.SetActive(false);
        return;
    }

   
}
