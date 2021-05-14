using UnityEngine;
using System.Collections;


// Character controller player input script.
public class CharacterControllerScript : MonoBehaviour
{
    // Cache
    public CharacterController characterController;
    //public Animator animator;

    // Set in editor
    [SerializeField] private float speed;
    [SerializeField] private float airSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float maxAirSpeed;

    // Controls
    public Vector3 moveDirection = Vector3.zero;
    private float forward;
    private float sideways;
    private bool jump;

    // Respawning
    private Vector3 spawn;

    public bool test = false;

    //Camera
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject playerVis;

    public bool isTalking = false;



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        spawn = transform.position; //set initial spawn for falling off the map

    }

    private void FixedUpdate()
    {
        //yes... not the cleanest code...

        //save vectors for movement direction + previous movement (used for aerial control)
        Vector3 prevMove = moveDirection;
        Vector3 forwardDir = camera.transform.forward;
        Vector3 horizontalDir = camera.transform.right;

        forwardDir.y = 0f;
        horizontalDir.y = 0f;
        forwardDir.Normalize();
        horizontalDir.Normalize();

        //saves the inputted direction from the user, normalizes it for speed adjustement
        Vector3 inpDir = forwardDir * forward + horizontalDir * sideways;
        inpDir.Normalize();

        
        //2 paths; one for grounded movement, another for air control
        if (characterController.isGrounded)
        {
            moveDirection = inpDir * speed;
            moveDirection.y = 0f;

            if (jump)
            {
                moveDirection.y += jumpSpeed * Time.fixedDeltaTime;
            }

            
            //following code is for a broken feature, kept it in so i can fix it later
            Vector3 newPosition = camera.transform.position;
            newPosition.y = transform.position.y;

            if (moveDirection.magnitude > 2)
            {
                playerVis.transform.rotation = Quaternion.Lerp(playerVis.transform.rotation, Quaternion.LookRotation(moveDirection), 10 * Time.deltaTime);
            }
            else
            {
                playerVis.transform.rotation = Quaternion.Lerp(playerVis.transform.rotation, Quaternion.LookRotation(newPosition), 10 * Time.deltaTime);

            }

        }
        else
        {
            //code for air control
            Vector3 newMove = moveDirection;
            newMove.y = 0f;

            //two paths: if max speed is reached (on x/z plane only), set to max. if not, adjust moveDirection by the air speed
            if (newMove.magnitude > maxAirSpeed)
            {
                newMove.Normalize();
                newMove *= maxAirSpeed;

            }
            else
            {
                newMove += inpDir * airSpeed;
                
            }

            newMove.y = prevMove.y;
            newMove.y += gravity * Time.fixedDeltaTime;

            //same as above, broken code will fix later
            moveDirection = newMove;
            if(moveDirection.magnitude > 2)
            {
                playerVis.transform.rotation = Quaternion.Lerp(playerVis.transform.rotation, Quaternion.LookRotation(moveDirection), 10 * Time.deltaTime);
            }
        }

        // Move the controller
        characterController.Move(moveDirection * Time.fixedDeltaTime);
        
        


    }


    // Player input should be registered in Update() because FixedUpdate may not update every frame.
    void Update()
    {
        //i tried using unity's new input manager.... it did not like my computer...
        sideways = Input.GetAxisRaw("Horizontal");
        forward = Input.GetAxisRaw("Vertical");
        jump = Input.GetButton("Jump");

        //again, broken feature
        playerVis.transform.position = characterController.transform.position;
        

    }

    public void Respawn()
    {
        //must disable cc to allow for teleporting player
        characterController.enabled = false;
        transform.position = spawn;
        characterController.enabled = true;
    }

    public void setSpawn() //used by checkpoint code; not used in game
    {
        spawn = transform.position;
    }

}