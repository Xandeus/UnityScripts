using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : NetworkBehaviour
{
    [SerializeField]
    GameObject alterPrefab;
    [SerializeField]
    Material highlight;

    [SerializeField]
    Transform buildingSpawn;

    [SerializeField]
    MeshRenderer buildingMesh;

    private bool buildingActive = false;
    // Use this for initialization
    void Start()
    {

    }
	void OnTriggerEnter(Collider other) {
             if (other.gameObject.tag == "YourTag") {
                Debug.Log("Its a hit!");
             }
	 }
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        RaycastHit hit2;

        if (Physics.Raycast(buildingSpawn.position, -Vector3.up, out hit2, 100.0f))
        {
            if (hit2.collider.tag == "Terrain")
            {
                Debug.DrawLine(buildingSpawn.position, hit2.point);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            buildingActive = !buildingActive;
            buildingMesh.enabled = buildingActive;
            // GameObject alter = (GameObject)Instantiate(
            // alterPrefab,
            // buildingSpawn.position,
            // buildingSpawn.rotation);
            if (!buildingActive)
            {
                RaycastHit hit;
                if (Physics.Raycast(buildingSpawn.position, -buildingSpawn.up, out hit, 100.0f))
                {
                    if (hit.collider.tag == "Terrain")
                    {
                        Vector3 placePos = hit.point;
                        placePos.x = Mathf.Round(placePos.x);
						placePos.y = Mathf.Round(placePos.y);
                        placePos.z = Mathf.Round(placePos.z);
                        Instantiate(alterPrefab, placePos, buildingSpawn.rotation);
                    }
                }
            }
        }
    }
}
