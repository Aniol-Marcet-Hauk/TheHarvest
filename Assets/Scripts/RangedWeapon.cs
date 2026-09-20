using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace sistemesArmes
{
    public class RangedWeapon : MonoBehaviour
    {

        [SerializeField]
        private GameObject arrow;
        public string releaseAnimation;
        public float damage;
        public float speedThatCharges;
        public float arrowRange;
        public float arrowTimeToTravel;
        public float startDistance = 0f;
        public LineRenderer lr;
        private PlayerAnimation pAn;
        private PlayerMain pm;
        private bool lightAttack;
        private float t = .1f;
        public LayerMask lm;

        private GameObject blood;
       
        private void Start()
        {
            pm = GameManager.gameManager.PLAYER;
            pAn = pm.animScript;
            lr.SetPosition(1, Vector3.zero);
            GameManager.lightAttack += atac;
            t += startDistance;
            blood = GameManager.gameManager.bloodSplatter;
            
        }
        private void OnDestroy()
        {
            GameManager.lightAttack -= atac;
        }
        private void Update()
        {
            if(lightAttack == true)
            {

                lr.transform.rotation =pm.transform.rotation;
               
                pm.Rotate(Time.deltaTime);
                if (t < arrowRange)
                {
                    lr.SetPosition(1, Vector3.forward * (t-.1f));
                    t += Time.deltaTime * speedThatCharges;
                }
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    //StartCoroutine(pm.GiraIMiraElEnemic(false, t, 5f, 0.01f));
                    lightAttack = false;
                    lr.SetPosition(1, Vector3.zero);
                    
                    pAn.PlayerTargetAn(releaseAnimation, true);
                    AtacFluixArc(t);
                    t = .1f +startDistance;
                }
                    
                
                 
                

            }
            
        }
        private void atac()
        {
            if(this != null)
            {
                lightAttack = true;
            }
           
        }
        private void AtacFluixArc(float range)
        {
            
            Transform _arrow = Instantiate(arrow, lr.transform.position , lr.transform.rotation).transform;
            RaycastHit info;

            if (Physics.SphereCast(pm.transform.position - _arrow.forward *0.5f, 0.5f, _arrow.forward, out info, range, lm))
            {

                float CalcSpeed =arrowTimeToTravel * info.distance / range;
                DOTween.Init();

                _arrow.DOMove(_arrow.position + _arrow.forward * (info.distance-.2f), CalcSpeed).OnComplete(() => damageEnem(info.collider.GetComponent<Enemy>(), _arrow,range,info.point));

            }
            else
            {
                _arrow.DOMove(_arrow.position + _arrow.forward * (range - .2f), arrowTimeToTravel).OnComplete(() => Destroy(_arrow.gameObject));
            }
          
            //Debug.Log("A");
            
            //_arrow.DOMove(_arrow.position + _arrow.forward * range,arrowTimeToTravel *range/arrowRange).OnComplete(() => Destroy(_arrow.gameObject));
        }
        private void damageEnem(Enemy en, Transform des,float endist,Vector3 point)
        {
            if(en != null)
            {
                en.MalEn(damage * endist / arrowRange,pm.transform.position);
                Vector3 rot = point- en.transform.position;
                

                float rotation = Mathf.Atan2(rot.x, rot.z) * Mathf.Rad2Deg;
                Quaternion inputRotation = Quaternion.AngleAxis(rotation, Vector3.up);
                StartCoroutine(dest(Instantiate(blood, point, inputRotation)));
            }
            Destroy(des.gameObject);
        }

        IEnumerator dest(GameObject _b)
        {
            yield return new WaitForSeconds(GameManager.gameManager.bloodTime);
            Destroy(_b);
        }


    }
}

