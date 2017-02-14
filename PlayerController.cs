using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public Transform bulletSpawn;
    public float playerSpeed = 1f;
    public float ascentSpeed = .1f;
    [SerializeField]
    private bool isDeity = false;
    private SimpleSmoothMouseLook mouseLook;
    private CharacterController controller;
    private float vSpeed = 0f;
    private float jumpSpeed = 8f;
    private float gravity = 9.8f;
    private Vector3 vel;
    void Update()
    {
        if (!isLocalPlayer)
        {
            if (gameObject.tag == "Player")
                GetComponentInChildren<Camera>().enabled = false;
            return;
        }
        controller = GetComponent<CharacterController>();
        if (!isDeity)
        {
            if (controller.isGrounded)
            {
                vel = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                vel = transform.TransformDirection(vel);
                vel *= playerSpeed;
                vSpeed = 0; // grounded character has vSpeed = 0...
                if (Input.GetKeyDown("space"))
                { // unless it jumps:
                    vSpeed = jumpSpeed;
                }
            }
            // apply gravity acceleration to vertical speed:
            vSpeed -= gravity * Time.deltaTime;
            vel.y = vSpeed; // include vertical speed in vel
            // convert vel to displacement and Move the character:
            controller.Move(vel * Time.deltaTime);
        }
        else
        {
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * playerSpeed*10;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed*10;
            var r = Input.GetAxis("Rotational") * Time.deltaTime * playerSpeed*10;
            transform.Translate(x, 0, z);
            transform.Rotate(Vector3.up * r);
        }
        if(Input.GetKeyDown(KeyCode.B)){
            ToggleDeity();
        }
        
    }
    void ToggleDeity(){
        Cursor.lockState = CursorLockMode.None;
        GetComponentInChildren<SimpleSmoothMouseLook>().enabled = isDeity;
        controller = GetComponent<CharacterController>();
        mouseLook = GetComponentInChildren<SimpleSmoothMouseLook>();
        controller.enabled = isDeity;
        mouseLook.enabled = isDeity;
        //Set position for deity
        if(!isDeity){
            transform.position = new Vector3(transform.position.x,75,transform.position.z);
            GetComponentInChildren<Camera>().transform.localEulerAngles = new Vector3(30,0,0);
        }
        isDeity = !isDeity; 
       
    }
    // This [Command] code is called on the Client …
    // … but it is run on the Server!
    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        // var bullet = (GameObject)Instantiate(
        //     bulletPrefab,
        //     bulletSpawn.position,
        //     bulletSpawn.rotation);

        // // Add velocity to the bullet
        // bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // // Spawn the bullet on the Clients
        // NetworkServer.Spawn(bullet);

        // // Destroy the bullet after 2 seconds
        // Destroy(bullet, 2.0f);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}