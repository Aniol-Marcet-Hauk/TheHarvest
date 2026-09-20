using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sistemesArmes;
public class ArmesPickUp : MonoBehaviour
{
    [SerializeField] private Armes arma;
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMain pm = GameManager.gameManager.PLAYER;

            if(pm.arma2 == null)
            {
                pm.arma2 = arma;
                
                Destroy(gameObject);
            }
            else
            {
                Armes a = arma;
                arma = pm.arma1;

                pm.arma1 = a;
                arma.Disable();
                a.Enable();
                pm.instansiadorArma.destrossarArma();
                pm.instansiadorArma.InstanciaArma(a);



            }
        }
    }

}
