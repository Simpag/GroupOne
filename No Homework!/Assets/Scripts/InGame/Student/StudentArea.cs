using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentArea : MonoBehaviour {

	[SerializeField]
	private string towerAreaTag = "TowerArea";
    private StudentStats student;
    private Material rangeMaterial;
	private Material cantPlaceMaterial;
    private MeshRenderer rangeView;

    private void Awake()
    {
        student = GetComponentInParent<StudentStats>();
        rangeView = student.rangeView.GetComponent<MeshRenderer>();
		rangeMaterial = student.rangeMaterial;
		cantPlaceMaterial = student.cantPlaceMaterial;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(towerAreaTag))
        {
			rangeView.material = cantPlaceMaterial;
            BuildManager.Instance.canBuild = false;
        }
        else if (collision.gameObject.tag == "CanPlaceTower")
        {
            rangeView.material = rangeMaterial;
            BuildManager.Instance.canBuild = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag(towerAreaTag))
        {
			rangeView.material = rangeMaterial;
            BuildManager.Instance.canBuild = true;
        }
        else if (collision.gameObject.tag == "CanPlaceTower")
        {
            rangeView.material = cantPlaceMaterial;
            BuildManager.Instance.canBuild = false;
        }
    }
}
