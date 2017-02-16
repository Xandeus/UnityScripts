using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class ResourceTree : NetworkBehaviour {
	public int health = 10;
	[SerializeField]
	GameObject logPrefab;
	[SerializeField]
	Transform logSpawn;
	// Update is called once per frame
	public void TakeDamage(int amount){
		health-=amount;
		if(health <= 0){
			//GetComponent<CapsuleCollider>().enabled = false;
			for(int i = 0;i<1;i++){
				CmdDropLogs(i);
			}
			Destroy(gameObject);

		}
	}
	[Command]
    void CmdDropLogs(int num)
    {
		GameObject log = (GameObject)Instantiate(logPrefab,logSpawn.position, logSpawn.rotation);
				        NetworkServer.Spawn(log);
	}
}
