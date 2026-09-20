using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sistemesArmes
{
    public class ArmaAgafadorIInstansiador : MonoBehaviour
    {
        public Transform posicio;
        CollisionsMal coll;
        public GameObject armaActiva;
        
        public void InstanciaArma(Armes arma)
        {

            
            if (arma == null)
            {
                destrossarArma();
                return;
            }
            GameObject armaObj = Instantiate(arma.ArmaPrefab);
            if(armaObj != null)
            {
                
                armaObj.transform.parent = posicio;
                
                
                armaObj.transform.localPosition = Vector3.zero;
                armaObj.transform.localScale = Vector3.one;
                armaObj.transform.localRotation = Quaternion.identity;


                coll = armaObj.GetComponent<CollisionsMal>();

            }
            armaActiva = armaObj;
            
        }
        public void desactivarArma()
        {
            if(armaActiva != null)
            {
                armaActiva.SetActive(false);
                coll = null;
            }
            
        }
        public void destrossarArma()
        {
            if(armaActiva != null)
            {
                Destroy(armaActiva);
                coll = null;
            }
            
        }

        public void EngegarColl()
        {
           
            if(coll == null)
            {
                return;
            }
            coll.TurnOnCollider();
        }
        public void ApagarColl()
        {
            
            if (coll == null)
            {
                return;
            }
            coll.TurnOffCollider();
        }

    }
}

