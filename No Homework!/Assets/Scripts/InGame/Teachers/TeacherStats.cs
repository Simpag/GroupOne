using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TeacherMovement))]
public class TeacherStats : MonoBehaviour {

    [SerializeField]
    private TeacherParent teacher;
	[SerializeField]
    private int homework;
    [SerializeField]
    private float health;
    [SerializeField]
    private float worth;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int ticketsForSpawning;

    public int Homework
    {
        get { return homework; }
        set { homework = value; }
    }
    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    public float Worth
    {
        get { return worth; }
        set { worth = value; }
    }
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public int TicketsForSpawning
    {
        get { return ticketsForSpawning; }
        set { ticketsForSpawning = value; }
    }

    public TeacherParent Teacher
    {
        get { return teacher; }
    }
    public TeacherMovement Movement
    {
        get { return movement; }
    }
    private TeacherMovement movement;


    private void Awake()
    {
        movement = GetComponent<TeacherMovement>();
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
