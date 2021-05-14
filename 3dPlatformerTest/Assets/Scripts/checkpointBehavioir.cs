using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointBehavioir : MonoBehaviour
{

    //code used for setting player's new spawn. not used in demo!

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            CharacterControllerScript sm = other.gameObject.GetComponent<CharacterControllerScript>();
            sm.setSpawn();
        }
    }
}
