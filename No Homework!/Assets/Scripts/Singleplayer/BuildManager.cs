using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {

    public Transform towerToBuild;

    [SerializeField]
    private LayerMask layerMask;


    private Vector3 locationToBuild;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            locationToBuild = hit.point;
        }

        if (Input.GetMouseButtonDown(0))
        {
            BuildTower();
        }
    }

    private void BuildTower()
    {
        Instantiate(towerToBuild, locationToBuild, Quaternion.identity);
    }
}
