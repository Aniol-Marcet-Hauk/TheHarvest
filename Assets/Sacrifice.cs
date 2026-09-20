using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Sacrifice : MonoBehaviour
{
    private bool hasHit;
    private void OnTriggerEnter(Collider other)
    {
        if (hasHit == true || other.tag != "Player")
        {
            return;
        }
        else
        {
            sac(); 
        }

    }
    private void sac()
    {
        if (GameManager.gameManager.PLAYER.rageModeON)
        {
            return;
        }
        hasHit = true;
        StartCoroutine(TimeToNextHit());
        GameObject _chosen;
        float _rand = Random.Range(0, 140);
        switch (_rand)
        {
            case < 30f:
                _chosen = GameManager.gameManager.chooseRandWeapon(GameManager.gameManager.allWeapons);
                spawnChosen(_chosen);
                break;
            case < 70f:
                int x = Random.Range(1, 3);
                for (int i = 0; i < x; i++)
                {
                    _chosen = GameManager.gameManager.chooseRandItem(GameManager.gameManager.allItems);
                    spawnChosen(_chosen);
                }
                break;
            case < 100f:
                int x2 = Random.Range(1, 4);
                for (int i = 0; i < x2; i++)
                {
                    _chosen = GameManager.gameManager.chooseRandUpgrade(GameManager.gameManager.allUpgrades);
                    spawnChosen(_chosen);
                }
                break;
            case < 141:
                GameManager.gameManager.coins += Random.Range(20, 40);
                break;
            default:
                _chosen = GameManager.gameManager.chooseRandWeapon(GameManager.gameManager.allWeapons);
                spawnChosen(_chosen);
                break;
        }
    }
    private void spawnChosen(GameObject _chosen)
    {
        GameObject _chosenGO = Instantiate(_chosen, transform.position, _chosen.transform.rotation);
        Vector3 vecn = Vector3.zero;
        do
        {
             vecn = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized * 3f;
        }
        while (vecn ==Vector3.zero);
        
        
        _chosenGO.transform.DOJump(transform.position+ vecn, 1.5f, 1, 1);


    }
    private IEnumerator TimeToNextHit()
    {
        yield return new WaitForSeconds(1.5f);
        hasHit = false;
    }
}
