using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerVisFollow : MonoBehaviour
{
    //broken currently, will work on in final build (not part of final submission for project)
    public GameObject player;
    private CharacterControllerScript ccs;
    // Start is called before the first frame update
    void Start()
    {
        ccs = player.GetComponent<CharacterControllerScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(ccs.moveDirection.magnitude >= 0)
        {
            transform.rotation = Quaternion.LookRotation(player.transform.position);
            
        }
        transform.position = player.transform.position;
    }
}
