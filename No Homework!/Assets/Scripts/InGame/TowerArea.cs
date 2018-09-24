using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerArea : MonoBehaviour {

    private Tower tower;
    private Color startColor;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
        startColor = tower.rangeView.GetComponent<MeshRenderer>().material.color;
    }

    public void TowerPlaced()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "TowerArea")
        {
            tower.rangeView.GetComponent<MeshRenderer>().material.color = tower.cantPlaceTint;
            BuildManager.Instance.canBuild = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "TowerArea")
        {
            tower.rangeView.GetComponent<MeshRenderer>().material.SetColor("_Color", startColor);
            BuildManager.Instance.canBuild = true;
        }
    }
}
