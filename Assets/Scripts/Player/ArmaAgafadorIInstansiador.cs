using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sistemesArmes
{
    public class ArmaAgafadorIInstansiador : MonoBehaviour
    {
        [SerializeField] private Transform m_Posicio;
        private CollisionsMal m_Coll;
        [SerializeField] private GameObject m_ArmaActiva;
        
        public void InstanciaArma(Armes arma)
        {

            
            if (arma == null)
            {
                DestrossarArma();
                return;
            }
            GameObject armaObj = Instantiate(arma.ArmaPrefab);
            if(armaObj != null)
            {
                
                armaObj.transform.parent = m_Posicio;
                
                
                armaObj.transform.localPosition = Vector3.zero;
                armaObj.transform.localScale = Vector3.one;
                armaObj.transform.localRotation = Quaternion.identity;


                m_Coll = armaObj.GetComponent<CollisionsMal>();

            }
            m_ArmaActiva = armaObj;
            
        }
        public void DesactivarArma()
        {
            if(m_ArmaActiva != null)
            {
                m_ArmaActiva.SetActive(false);
                m_Coll = null;
            }
            
        }
        public void DestrossarArma()
        {
            if(m_ArmaActiva != null)
            {
                Destroy(m_ArmaActiva);
                m_Coll = null;
            }
            
        }

        public void EngegarColl()
        {
           
            if(m_Coll == null)
            {
                return;
            }
            m_Coll.TurnOnCollider();
        }
        public void ApagarColl()
        {
            
            if (m_Coll == null)
            {
                return;
            }
            m_Coll.TurnOffCollider();
        }

        public Transform posicio => m_Posicio;
        public GameObject armaActiva => m_ArmaActiva;
        public void desactivarArma() => DesactivarArma();
        public void destrossarArma() => DestrossarArma();

    }
}

