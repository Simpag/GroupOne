using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomicsTeacher : TeacherParent {

    [Header("Properties")]
    [SerializeField]
    private float timeBeforeFirstMoney;
    [SerializeField]
    private float timeBetweenMoney;
    [SerializeField]
    private int amountOfMoney;

    private void Start()
    {
        StartCoroutine("MoneyOverTime"); 
    }

    private IEnumerator MoneyOverTime()
    {
        yield return new WaitForSeconds(timeBeforeFirstMoney);

        while (true)
        {
            yield return new WaitForSeconds(timeBetweenMoney);

            PlayerStats.RemoveCandyCurrency(amountOfMoney);
        }
    }

    public override void Died(bool _killed)
    {
        StopCoroutine("MoneyOverTime");

        base.Died(_killed);
    }
}
