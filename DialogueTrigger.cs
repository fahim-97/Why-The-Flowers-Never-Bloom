using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueLines dialog;

    public void TriggerDialogue()
    {
        Cursor.visible = true;
        FindObjectOfType<DialogueManager>().StartDialogue(dialog);
    }
}
