using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sistemesArmes;
public class TemporariDeteccioAtac : MonoBehaviour
{
    //IMPORTANT ENRECORDA: canviar com agafes arma mal, per aixi poder ficar aixo en items tmb
    [SerializeField] private Armes m_Arma;
    [SerializeField]
    private bool destroyOnHit;
    private GameObject m_Blood;
    private void Start()
    {
        m_Blood = GameManager.gameManager.bloodSplatter;
    }
    private void OnTriggerEnter(Collider other)
    {
        /*x quan hi hagin més enemics prefereixo canviar aquest codi a crear un script
         anomenat healthEnemy, i quan vulgui fer mal busco per aquest script i canvio la vida(amb healthClass)
        o potser millor cridar a el script atac i que aquest fagi el mal
         
         */
        Enemy en = other.GetComponent<Enemy>();
        if (en != null)
        {

            GameManager.gameManager.OnHitEnemy(en);
            en.MalEn(m_Arma.mal, GameManager.gameManager.PLAYER.transform.position);
            Vector3 closestPoint = other.ClosestPoint(transform.position);
            Vector3 rot = closestPoint - en.transform.position;
            //AudioSource.PlayClipAtPoint(GameManager.gameManager.damageclip,transform.position) damageclip cache beforehand
     
            float rotation = Mathf.Atan2(rot.x, rot.z) * Mathf.Rad2Deg;
            Quaternion inputRotation = Quaternion.AngleAxis(rotation, Vector3.up);
            GameObject b = Instantiate(m_Blood, closestPoint, inputRotation);
            StartCoroutine(Dest(b));



            

           
           
        }
        
        if(destroyOnHit == true && other.GetComponent<PlayerMain>() == null && other.tag != "Bullet" && other.tag != "RoomCollider" && other.tag != "Player")
        {


            Destroy(gameObject);
        }
    }
    private IEnumerator Dest(GameObject _b)
    {
        yield return new WaitForSeconds(GameManager.gameManager.bloodTime);
        Destroy(_b);
    }

    public Armes arma => m_Arma;
}
