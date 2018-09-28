using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerArea : MonoBehaviour {

	[SerializeField]
	private string towerAreaTag = "TowerArea";
    private Tower tower;
    private Material rangeMaterial;
	private Material cantPlaceMaterial;
    private MeshRenderer rangeView;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
        rangeView = tower.rangeView.GetComponent<MeshRenderer>();
		rangeMaterial = tower.rangeMaterial;
		cantPlaceMaterial = tower.cantPlaceMaterial;
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
