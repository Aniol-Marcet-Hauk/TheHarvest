using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sistemesArmes
{
    public class Shield : MonoBehaviour
    {
        public float distanceThrowEnBlock = 1f, speedThrowEnBlock = .5f;
        [SerializeField] private Collider coll;
        private bool block, parry;

        private PlayerMain pm;
        private Transform pmT;

        // Start is called before the first frame update
        void Start()
        {

            GameManager.heavyAttack += Escut;
            block = false;
            pm = GameManager.gameManager.PLAYER;
            pmT = pm.transform;
        }

        void Escut()
        {
            block = true;
            parry = true;
            coll.enabled = true;
            StartCoroutine(ParryOff());
        }
        private void OnTriggerEnter(Collider other)
        {
            Enemy en = other.GetComponent<Enemy>();
            if (en != null)
            {
                float dot = Vector3.Dot(pmT.forward, (other.transform.position - transform.position).normalized);
                if (dot >= .3f)
                {
                    if (parry == true)
                    {
                        en.TakeKnockBack(pmT.position, distanceThrowEnBlock, speedThrowEnBlock);// Make time slow down for a moment or damage enemy ?
                    }
                    en.TakeKnockBack(pmT.position, distanceThrowEnBlock, speedThrowEnBlock);//idk wierd bc of relative knockbackof each enemy, i think i should ad a third parameter being knockback

                }
            }
            else if (other.tag == "Bullet")
            {
                Destroy(other);
            }
        }
        private void Update()
        {
            if (block == true)
            {
                pm.Rotate(Time.deltaTime);

                if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    coll.enabled = false;
                    block = false;
                    pm.animScript.PlayerTargetAn("Recoil", true);
                }


                //animator play animation
            }
        }
        IEnumerator ParryOff()
        {
            yield return new WaitForSeconds(.15f);
            parry = false;
        }
        private void OnDestroy()
        {
            GameManager.heavyAttack -= Escut;
        }
    }
}