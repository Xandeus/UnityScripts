using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
	public float playerSpeed = 50f;
	public float ascentSpeed = .1f;
    void Update()
    {
        if (!isLocalPlayer)
        {
			if(gameObject.tag == "Player")
				GetComponentInChildren<Camera>().enabled = false;
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * playerSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed;

        transform.Translate(x, 0, z);

        if (Input.GetKey(KeyCode.Space))
        {
			transform.Translate(0,Time.deltaTime * ascentSpeed,0);
        }
    }

    // This [Command] code is called on the Client …
    // … but it is run on the Server!
    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    public override void OnStartLocalPlayer ()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}