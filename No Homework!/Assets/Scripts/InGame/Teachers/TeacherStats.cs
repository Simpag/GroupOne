using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherStats : MonoBehaviour {

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

    private TeacherParent teacher;
    public TeacherParent Teacher
    {
        get { return teacher; }
    }
    private TeacherMovement movement;
    public TeacherMovement Movement
    {
        get { return movement; }
    }

    private float normalSpeed = 0;
    private float slowTimer = 0;

    private void Awake()
    {
        movement = GetComponent<TeacherMovement>();
        teacher = GetComponent<TeacherParent>();

        if (teacher == null)
            teacher = GetComponentInChildren<TeacherParent>();
    }

    private void Update()
    {
        if (normalSpeed != 0 && slowTimer <= 0)
        {
            ReturnToNormalSpeed();
        }
        else if (normalSpeed != 0)
        {
            slowTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage (float _amount)
    {
        health -= _amount;

        if (health <= 0)
        {
            movement.Died(true);
        }
    }

    public void SlowTeacher(float _amount, float _time)
    {
        normalSpeed = speed;
        speed *= 1 - _amount;
        slowTimer = _time;
    }

    private void ReturnToNormalSpeed()
    {
        if (normalSpeed != 0)
        {
            speed = normalSpeed;
            normalSpeed = 0;
        }
    }
}
