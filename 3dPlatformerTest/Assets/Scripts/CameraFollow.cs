using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    //3rd person camera that follows the player and rotates them for camera-relative movement
    [SerializeField] private GameObject player;
    [SerializeField] private float cameraSens;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        float horizontalRot = Input.GetAxis("Mouse X") * cameraSens;

        player.transform.Rotate(0, horizontalRot, 0);

        float angle = player.transform.eulerAngles.y;
        Quaternion rot = Quaternion.Euler(0, angle, 0);
        transform.position = player.transform.position - (rot * offset);

        transform.LookAt(player.transform);
    }
}
