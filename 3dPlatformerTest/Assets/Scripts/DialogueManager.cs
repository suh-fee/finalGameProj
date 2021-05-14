using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour

{
    // queue allows for easy file IO in proper order
    public Queue<string> sentences;
    public CharacterControllerScript player;

    // UI elements to be updated in realtime
    public Text currDialogue;
    public Text currName;
    [SerializeField]
    Transform UIPanel;

    public bool test = false; // used to speed up dialogue for testing purposes
    
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        UIPanel.gameObject.SetActive(false); //dialogue is closed from start
    }

    private void Awake()
    {
        //sets maximum framerate, allows for more consistent dialogue
        QualitySettings.vSyncCount = 0;  
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        
    }

    public void StartDialogue(Dialogue dialogue)
    {
        // since this function is called to start AND continue conversation, we have to make sure we go down the correct path
        if (player.isTalking)
        {
            DisplayNextSentence();
        } else
        {
            // disable player movement, enable the UI
            player.characterController.enabled = false;
            UIPanel.gameObject.SetActive(true);
            //Debug.Log("Starting conversation with " + dialogue.name);

            //clear the queue for the new set of sentences
            sentences.Clear();
            foreach (string sent in dialogue.sentences) //read each line in sentences, add to queue
            {
                sentences.Enqueue(sent);
            }
            
            //setting the name here doesn't matter, as it will be overwritten, but i'm keeping it just in case
            currName.text = dialogue.name;

            //set bool for checking, continue dialogue
            player.isTalking = true;
            DisplayNextSentence();
        }

    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 1) //for some reason the EOF is a line as well, so we stop when there's one sentence left
        {
            EndDialogue();
            return;
        }

        // format of text: <name>;<dialogue>
        // allows for 'multi character' conversations
        string[] nextSen = sentences.Dequeue().Split(';');
        currName.text = nextSen[0];

        // basic thread usage, still really iffy on it (but allows us to skip dialogue!)
        StopAllCoroutines();
        StartCoroutine(TypeSentence(nextSen[1]));

    }

    IEnumerator TypeSentence(string sent) //using a enumerator so i can make text appear like older RPGs
    {
        //number of yields = number of frames to wait between characters
        currDialogue.text = "";

        foreach(char letter in sent.ToCharArray())
        {
            currDialogue.text += letter;
            if (!test)
            {
                yield return null;
                yield return null;
            } else
            {
                yield return null;
            }
            
        }
    }

    void EndDialogue()
    {
        //set all the variables for character control/UI, as well as advance the dialogue path (if possible)
        player.characterController.enabled = true;
        player.isTalking = false;
        //Debug.Log("Ending convo");
        UIPanel.gameObject.SetActive(false);
        FindObjectOfType<NPC>().counter += 1;
        FindObjectOfType<NPC>().next();
    }
}
