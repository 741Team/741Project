using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager singleton;

    public List<EnemyBase> enemyList = new List<EnemyBase>();

    protected void Awake()
    {
        singleton = this;
    }

    public void AddEnemy(EnemyBase enemy)
    {
        enemyList.Add(enemy);
    }

    public List<EnemyBase> GetEnemyList()
    {
        return enemyList;
    }

}
