using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sistemesArmes;
public class ArmesPickUp : MonoBehaviour
{
    [SerializeField] private Armes m_Arma;
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMain pm = GameManager.gameManager.PLAYER;

            if(pm.arma2 == null)
            {
                pm.arma2 = m_Arma;
                
                Destroy(gameObject);
            }
            else
            {
                Armes a = m_Arma;
                m_Arma = pm.arma1;

                pm.arma1 = a;
                m_Arma.Disable();
                a.Enable();
                pm.instansiadorArma.destrossarArma();
                pm.instansiadorArma.InstanciaArma(a);



            }
        }
    }

}
