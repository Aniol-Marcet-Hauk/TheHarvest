using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sistemesArmes
{
    public class Shield : MonoBehaviour
    {
        [SerializeField] private float m_DistanceThrowEnBlock = 1f;
        [SerializeField] private float m_SpeedThrowEnBlock = .5f;
        [SerializeField] private Collider m_Coll;
        private bool m_Block, m_Parry;

        private PlayerMain m_Pm;
        private Transform m_PmT;

        // Start is called before the first frame update
        void Start()
        {

            GameManager.heavyAttack += Escut;
            m_Block = false;
            m_Pm = GameManager.gameManager.PLAYER;
            m_PmT = m_Pm.transform;
        }

        void Escut()
        {
            m_Block = true;
            m_Parry = true;
            m_Coll.enabled = true;
            StartCoroutine(ParryOff());
        }
        private void OnTriggerEnter(Collider other)
        {
            Enemy en = other.GetComponent<Enemy>();
            if (en != null)
            {
                float dot = Vector3.Dot(m_PmT.forward, (other.transform.position - transform.position).normalized);
                if (dot >= .3f)
                {
                    if (m_Parry == true)
                    {
                        en.TakeKnockBack(m_PmT.position, m_DistanceThrowEnBlock, m_SpeedThrowEnBlock);// Make time slow down for a moment or damage enemy ?
                    }
                    en.TakeKnockBack(m_PmT.position, m_DistanceThrowEnBlock, m_SpeedThrowEnBlock);//idk wierd bc of relative knockbackof each enemy, i think i should ad a third parameter being knockback

                }
            }
            else if (other.tag == "Bullet")
            {
                Destroy(other);
            }
        }
        private void Update()
        {
            HandleBlocking();
        }

        private void HandleBlocking()
        {
            if (m_Block == true)
            {
                m_Pm.Rotate(Time.deltaTime);

                if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    m_Coll.enabled = false;
                    m_Block = false;
                    m_Pm.animScript.PlayerTargetAn("Recoil", true);
                }


                //animator play animation
            }
        }
        IEnumerator ParryOff()
        {
            yield return new WaitForSeconds(.15f);
            m_Parry = false;
        }
        private void OnDestroy()
        {
            GameManager.heavyAttack -= Escut;
        }
    }
}