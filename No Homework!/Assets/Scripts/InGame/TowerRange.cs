using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour {

	[SerializeField]
	private string enemyTag = "Enemy";

    private Tower tower;

    //Quick fix
    GameObject currentTarget;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(enemyTag) && currentTarget == null)
        {
			tower.target = collision.transform;
            currentTarget = collision.gameObject;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag(enemyTag) && collision.gameObject == currentTarget)
        {
            tower.target = null;
            currentTarget = null;
        }
    }
}
