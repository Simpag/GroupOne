using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

	[SerializeField]
    private int homework;
    [SerializeField]
    private float health;
    [SerializeField]
    private float worth;

    public int Homework
    {
        get { return homework; }
    }
    public float Health
    {
        get { return health; }
    }
    public float Worth
    {
        get { return worth; }
    }

    [SerializeField]
    private List<Tower> seenByTower;

    private void Awake()
    {
        seenByTower = new List<Tower>();
    }

    public void TakeDamage (float _amount)
    {
        health -= _amount;

        if (health <= 0)
        {
            Died();
        }
    }

    public void AddSeenByTower (Tower _tower)
    {
        if (!seenByTower.Contains(_tower))
            seenByTower.Add(_tower);
    }

    public void Died()
    {
        PlayerStats.AddCandyCurrency(worth);

        foreach (Tower _tower in seenByTower)
        {
            if (_tower != null)
                _tower.rangeView.GetComponent<TowerRange>().RemoveEnemyFromEnemiesInRange(this.gameObject);
        }

        Destroy(this.gameObject);
    }

    public void RemoveSeenByTower(Tower _tower)
    {
        if (seenByTower.Contains(_tower))
            seenByTower.Remove(_tower);
    }
}
