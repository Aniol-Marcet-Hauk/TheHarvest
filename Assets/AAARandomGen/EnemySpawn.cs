
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
//aquest text va als spawners del model de l'habitaciò
public class EnemySpawn : MonoBehaviour
{
    public List<Enemies> cases;

    public Enemies randomEN(Enemies _en)
    {


        
        

        Enemies[] enemyRandom = Enum.GetValues(typeof(Enemies)).
            Cast<Enemies>().
            Where(x => _en.HasFlag(x)).ToArray();

        
        if(enemyRandom.Length == 1)
        {
            return Enemies.None;
        }
        

        
        Enemies myEnemy = enemyRandom[UnityEngine.Random.Range(1, enemyRandom.Length)];
       
        
       
        
        return myEnemy;
    }



   

}



[Flags]
public enum Enemies
{
    None = 0,
    Wolf = 1 << 0,
    FaceScream = 1 << 1,
    RandomMove = 1 << 2,
    SimpleShooter = 1 << 3,
    BarrelThrower = 1 << 4,
    RunsAway = 1 <<5,
    BasicPuncher = 1 << 6
} 

