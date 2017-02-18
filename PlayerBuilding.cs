using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : NetworkBehaviour
{
    [SerializeField]
    GameObject[] buildings;
    private GameObject currentBuilding;
    [SerializeField]
    Material highlight;

    [SerializeField]
    Transform buildingSpawn;

    [SerializeField]
    MeshRenderer buildingMesh;

    private bool buildingActive = false;
    [SerializeField]

    Canvas ui;
    public float rotateSpeed = 10f;
    // Use this for initialization
    void Start()
    {

    }
    [Command]
    void CmdBuild()
    {
        RaycastHit hit;
        if (Physics.Raycast(buildingSpawn.position, -buildingSpawn.up, out hit, 100.0f))
        {
            if (hit.collider.tag == "Terrain")
            {
                Vector3 placePos = hit.point;
                placePos.x = Mathf.Round(placePos.x);
                placePos.y = Mathf.Round(placePos.y) + (currentBuilding.GetComponent<BoxCollider>().size.y/2)-currentBuilding.GetComponent<BoxCollider>().center.y;
                placePos.z = Mathf.Round(placePos.z);
                GameObject building = (GameObject)Instantiate(currentBuilding, placePos, buildingSpawn.rotation);
                NetworkServer.Spawn(building);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.B)){
            GetComponentInChildren<SimpleSmoothMouseLook>().ToggleMouseLock();
            ui.enabled = !ui.enabled;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            // GameObject alter = (GameObject)Instantiate(
            // alterPrefab,
            // buildingSpawn.position,
            // buildingSpawn.rotation);
            if (buildingActive)
            {
                CmdBuild();
                buildingActive = false;
                buildingMesh.enabled = false;
            }
        }
        if(buildingActive){
            buildingSpawn.Rotate(Vector3.up,Input.GetAxis("Rotational") * Time.deltaTime * rotateSpeed);
        }
    }
    public void SetBuilding(int selection){
        currentBuilding = buildings[selection];
        ui.enabled = !ui.enabled;
        GetComponentInChildren<SimpleSmoothMouseLook>().ToggleMouseLock();
        buildingActive = true;
        buildingMesh.GetComponent<MeshFilter>().mesh = buildings[selection].GetComponent<MeshFilter>().sharedMesh;
        buildingMesh.enabled = true;
        buildingSpawn.GetComponent<BoxCollider>().size = buildings[selection].GetComponent<BoxCollider>().size;

    }
}
