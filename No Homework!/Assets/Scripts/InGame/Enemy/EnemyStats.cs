using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
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
    public List<Tower> SeenByTower
    {
        get { return seenByTower; }
    }

    private EnemyMovement movement;

    private void Awake()
    {
        seenByTower = new List<Tower>();
        movement = GetComponent<EnemyMovement>();
    }

    public void TakeDamage (float _amount)
    {
        health -= _amount;

        if (health <= 0)
        {
            movement.Died(true);
        }
    }

    public void AddSeenByTower (Tower _tower)
    {
        if (!seenByTower.Contains(_tower))
            seenByTower.Add(_tower);
    }

    public void RemoveSeenByTower(Tower _tower)
    {
        if (seenByTower.Contains(_tower))
            seenByTower.Remove(_tower);
    }
}
