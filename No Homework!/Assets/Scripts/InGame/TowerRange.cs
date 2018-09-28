using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour {

	[SerializeField]
	private string enemyTag = "enemy";

    private Tower tower;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
    }

    private void OnTriggerEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
			tower.target = collision.transform;
        }
    }

    private void OnTriggerExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            tower.target = null;
        }
    }
}
