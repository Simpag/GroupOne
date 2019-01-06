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

    public EnemyMovement Movement
    {
        get { return movement; }
    }
    private EnemyMovement movement;

    private void Awake()
    {
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
}
