using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentArea : MonoBehaviour {

	[SerializeField]
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
        if (collision.gameObject.CompareTag(GameConstants.STUDENT_AREA_TAG))
        {
            rangeView.material = cantPlaceMaterial;
            BuildManager.Instance.canBuild = false;
        }
        else if (collision.gameObject.tag == GameConstants.CAN_PLACE_STUDENT)
        {
            rangeView.material = rangeMaterial;
            BuildManager.Instance.canBuild = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag(GameConstants.STUDENT_AREA_TAG))
        {
			rangeView.material = rangeMaterial;
            BuildManager.Instance.canBuild = true;
        }
        else if (collision.gameObject.tag == GameConstants.CAN_PLACE_STUDENT)
        {
            rangeView.material = cantPlaceMaterial;
            BuildManager.Instance.canBuild = false;
        }
    }
}
