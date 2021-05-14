using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NPC : MonoBehaviour
{

    //handles user input for text, as well as advancing the text tree
    
    //single dialogue object, reused multiple times for each text file
    public Dialogue text;

    [SerializeField] 
    GameObject player;

    //allows for multiple text files to be read/used
    [SerializeField] 
    string[] fileName;

    //used for dialogue system
    public int counter = 0;
    public int max;
    protected FileInfo theSourceFile = null;
    protected StreamReader reader = null;
    protected string curr; 

    void Update()
    {
        // E to talk to NPCs
        if(Vector3.Distance(transform.position, player.transform.position) < 6 && Input.GetKeyDown(KeyCode.E)){
            
            TriggerDialogue();
        }

    }

    private void Start()
    {
        next(); // reads first text file
    }


    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(text);

    }

    public void next()
    {
        curr = " ";
        text.sentences.Clear(); //ensures we can properly read next file without adding unto previous queue


        if(counter == max) //allows for rereading late dialogue file if game isn't progressed
        {
            counter -= 1;
        }

        theSourceFile = new FileInfo("Assets/Dialogue/" + fileName[counter]);
        reader = theSourceFile.OpenText();
        while (curr != null)
        {
            curr = reader.ReadLine();
            text.sentences.Add(curr);
        }
        
    }
}
