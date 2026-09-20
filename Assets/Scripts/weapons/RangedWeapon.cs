using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace sistemesArmes
{
    public class RangedWeapon : MonoBehaviour
    {

        [SerializeField]
        private GameObject m_Arrow;
        [SerializeField] private string m_ReleaseAnimation;
        [SerializeField] private float m_Damage;
        [SerializeField] private float m_SpeedThatCharges;
        [SerializeField] private float m_ArrowRange;
        [SerializeField] private float m_ArrowTimeToTravel;
        [SerializeField] private float m_StartDistance = 0f;
        [SerializeField] private LineRenderer m_Lr;
        private PlayerAnimation m_PAn;
        private PlayerMain m_Pm;
        private bool m_LightAttack;
        private float m_T = .1f;
        [SerializeField] private LayerMask m_Lm;

        private GameObject m_Blood;
       
        private void Start()
        {
            m_Pm = GameManager.gameManager.PLAYER;
            m_PAn = m_Pm.animScript;
            m_Lr.SetPosition(1, Vector3.zero);
            GameManager.lightAttack += Atac;
            m_T += m_StartDistance;
            m_Blood = GameManager.gameManager.bloodSplatter;
            
        }
        private void OnDestroy()
        {
            GameManager.lightAttack -= Atac;
        }
        private void Update()
        {
            HandleChargingAttack();
        }
        private void HandleChargingAttack()
        {
            if (m_LightAttack == true)
            {
                m_Lr.transform.rotation = m_Pm.transform.rotation;

                m_Pm.Rotate(Time.deltaTime);
                if (m_T < m_ArrowRange)
                {
                    m_Lr.SetPosition(1, Vector3.forward * (m_T - .1f));
                    m_T += Time.deltaTime * m_SpeedThatCharges;
                }
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    m_LightAttack = false;
                    m_Lr.SetPosition(1, Vector3.zero);

                    m_PAn.PlayerTargetAn(m_ReleaseAnimation, true);
                    AtacFluixArc(m_T);
                    m_T = .1f + m_StartDistance;
                }
            }
        }

        private void Atac()
        {
            if(this != null)
            {
                m_LightAttack = true;
            }
           
        }
        private void AtacFluixArc(float range)
        {
            
            Transform _arrow = Instantiate(m_Arrow, m_Lr.transform.position , m_Lr.transform.rotation).transform;
            RaycastHit info;

            if (Physics.SphereCast(m_Pm.transform.position - _arrow.forward *0.5f, 0.5f, _arrow.forward, out info, range, m_Lm))
            {

                float calcSpeed = m_ArrowTimeToTravel * info.distance / range;
                DOTween.Init();

                _arrow.DOMove(_arrow.position + _arrow.forward * (info.distance-.2f), calcSpeed).OnComplete(() => DamageEnem(info.collider.GetComponent<Enemy>(), _arrow,range,info.point));

            }
            else
            {
                _arrow.DOMove(_arrow.position + _arrow.forward * (range - .2f), m_ArrowTimeToTravel).OnComplete(() => Destroy(_arrow.gameObject));
            }
          
            //Debug.Log("A");
            
            //_arrow.DOMove(_arrow.position + _arrow.forward * range,arrowTimeToTravel *range/arrowRange).OnComplete(() => Destroy(_arrow.gameObject));
        }
        private void DamageEnem(Enemy en, Transform des,float endist,Vector3 point)
        {
            if(en != null)
            {
                en.MalEn(m_Damage * endist / m_ArrowRange,m_Pm.transform.position);
                Vector3 rot = point- en.transform.position;
                

                float rotation = Mathf.Atan2(rot.x, rot.z) * Mathf.Rad2Deg;
                Quaternion inputRotation = Quaternion.AngleAxis(rotation, Vector3.up);
                StartCoroutine(Dest(Instantiate(m_Blood, point, inputRotation)));
            }
            Destroy(des.gameObject);
        }

        private IEnumerator Dest(GameObject _b)
        {
            yield return new WaitForSeconds(GameManager.gameManager.bloodTime);
            Destroy(_b);
        }

        public string releaseAnimation => m_ReleaseAnimation;
        public float damage => m_Damage;
        public float speedThatCharges => m_SpeedThatCharges;
        public float arrowRange => m_ArrowRange;
        public float arrowTimeToTravel => m_ArrowTimeToTravel;
        public float startDistance => m_StartDistance;
        public LineRenderer lr => m_Lr;
        public LayerMask lm => m_Lm;


    }
}

