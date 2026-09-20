using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//quan tinguem el model del personatge caldrà fer una cosa pq segueixi la mà del personatge
namespace sistemesArmes 
{

    [CreateAssetMenu(menuName = "Armes")]

    public class Armes: ScriptableObject
    {
        
        public string atacFluix,atacFort;

        //private string _atacFlCombo, _atacFoCombo;
    
        private int combo;
        public float mal;
        [SerializeField]
        private float mal1, mal2;
        public string armaNom;
        public Vector3 posicioArma;
        public GameObject ArmaPrefab;
        public bool noActiu;
        private PlayerAnimation pAn;
        public bool usesFreeFlow;
        public Texture WeaponSprite;
        public float speedOfAttacks = 1;
        public AudioClip a,b;
        public void Enable()
        {
            
            GameManager.lightAttack += AtacFluix;
           
            
            GameManager.heavyAttack += AtacFort;
            
           
        }
        public void Disable()
        {
            
            GameManager.lightAttack -= AtacFort;
            
            
            GameManager.heavyAttack -= AtacFort;
            
                
            if(pAn != null)
            {
                pAn.an.SetFloat("velocitatAttack", 1);
            }
            
        }
        public void OnDestroy()
        {

            GameManager.lightAttack -= AtacFort;


            GameManager.heavyAttack -= AtacFort;


            if (pAn != null)
            {
                pAn.an.SetFloat("velocitatAttack", 1);
            }

        }


        public void AtacFluix()
        {
            if (atacFluix == "")
            {
                return;
            }
            if (pAn == null)
            {
                
                pAn = GameManager.gameManager.PLAYER.animScript;
                pAn.an.SetFloat("velocitatAttack", speedOfAttacks);
            }
            if(a!= null)
            {
                GameManager.gameManager.PlayGeneralAudio(a);
            }
           
            mal = mal1;
            if (pAn.GetPermetreCombo()== false)
            {

                pAn.PlayerTargetAn(atacFluix, true);
            }
            else
            {

                pAn.an.SetBool("combo", true);
            }
            
            
        }
        public void AtacFort()
        {
            if (atacFort == "")
            {
                return;
            }

            if (pAn == null)
            {
                pAn = GameManager.gameManager.PLAYER.animScript;
                pAn.an.SetFloat("velocitatAttack", speedOfAttacks);
            }
            if (b != null)
            {
                GameManager.gameManager.PlayGeneralAudio(b);
            }
            else if(a != null)
            {
                    GameManager.gameManager.PlayGeneralAudio(a);
            }
            
            mal = mal2;
            if (pAn.GetPermetreCombo() == false)
            {
                pAn.PlayerTargetAn(atacFort, true);
            }
            else
            {
                 pAn.an.SetBool("combo", true);
            }
                
        }


    }

}

