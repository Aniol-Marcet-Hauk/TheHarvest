using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
public class EnemicQueDispara : MonoBehaviour
{
    [SerializeField] private float distanceBeforeStartShoots,distanceOfShot,speedShot,timeBeforeAttack,timeRandBeforeAttack,speedRotates= 5f;
    [SerializeField] private GameObject shot;
    [SerializeField] private float JumpHeight, destroyAnimationDuration = 0.5f;
    [SerializeField] private int numOfJumps = 0;
    [SerializeField] Transform endPosition;
    private Transform player;
    private bool isAttacking;
    private Enemy enScript;
    private NavMeshAgent nav;
    public bool shake = false;
    private void Start()
    {
        player = GameManager.gameManager.PLAYER.transform;
        enScript = transform.GetComponent<Enemy>();
        nav = transform.GetComponent<NavMeshAgent>();
        if (nav != null)
        {
            nav.updateRotation = false;
        }
        
    }
    private void Update()
    {
        if (!enScript.death)
        {
            if (enScript.enemyLife._vida == 0f)
            {
                enScript.death = true;
                if(nav != null)
                {
                    nav.isStopped = true;
                }
                StartCoroutine(enScript.Mort());
            }
            else
            {
                ComençaADisparar();
            }
        }
        
        
    }
    private void ComençaADisparar()
    {
        //Either Make It so it doesnt immediately rotate to look at the player or something like that
        
        


        Vector3 rot = (player.position-transform.position ).normalized;


        Quaternion inputRotation = Quaternion.LookRotation(rot);

       // vec.y += 180;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, inputRotation, speedRotates * Time.deltaTime);
        float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceFromPlayer < distanceBeforeStartShoots && isAttacking == false)
        {
            isAttacking = true;
            StartCoroutine("Shoot");
        }
    }
    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(timeBeforeAttack+Random.Range(-timeRandBeforeAttack,timeRandBeforeAttack));
        if (!enScript.death)
        {
            Transform activeShot = Instantiate(shot, shot.transform.position, shot.transform.rotation).transform;
            activeShot.gameObject.SetActive(true);
            
            activeShot.DOJump(endPosition.position, JumpHeight, numOfJumps, speedShot);
            if(enScript.typicalSound != null)
            {
                enScript.typicalSound.Play();
            }
            if(shake)
            {
                StartCoroutine(GameManager.gameManager._ProcessShake(3.5f, 2f, .015f));
            }
            isAttacking = false;
            yield return new WaitForSeconds(speedShot + 0.1f);
            if (activeShot != null)
            {
                activeShot.GetComponent<Animator>().Play("Breakit");
                yield return new WaitForSeconds(destroyAnimationDuration);
                Destroy(activeShot.gameObject);
            }

        }
        
        
    }
    
   

}
