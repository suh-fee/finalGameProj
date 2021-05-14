using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEMScript : MonoBehaviour
{
    //script for the DEM, or end game item;
    public GameObject tar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //allows the quad this script is assigned to to always be facing the camera
        transform.LookAt(tar.transform.position);
        transform.forward *= -1;
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        //allows the player to 'collect' the DEM

        if (other.gameObject.CompareTag("Player"))
        {
            // sets max and counter, advancing the dialogue to the end-of-demo spot, will be different in final game
            FindObjectOfType<NPC>().max = 4;
            FindObjectOfType<NPC>().counter = 3;
            FindObjectOfType<NPC>().next();

            // 'collects' the item
            Destroy(this.gameObject);
        }
    }
}
