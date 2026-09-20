using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "Items/Knives")]
public class Knives:Items
{
    public string _Name = "Knife";
    public Texture _imageUI;
    public float Damage = 2;
    public int _addAmount = 3;
    
    public GameObject knifePrefab;
    public float knifeSpeed = 0.4f;
    public float _distance = 6f;
    [SerializeField]
    private LayerMask m_Lm;
    public float _coolDown;
    //private Transform throwKnife;
    public override int addAmount()
    {
        return _addAmount;
    }
    public override string giveName()
    {
        return _Name;
    }
    public override Texture imageUI()
    {
        return _imageUI;
    }
    public override float coolDown()
    {
        return _coolDown;
    }
    public override void _Update(PlayerMain player)
    {
        player.ItemInteract();
        Transform throwKnife= Instantiate(knifePrefab).transform;
   
        throwKnife.position = player.transform.position;
        throwKnife.rotation = Quaternion.LookRotation(player.rot);
        RaycastHit info;

        if( Physics.SphereCast(throwKnife.position - throwKnife.forward*0.25f, 0.5f, throwKnife.forward,out info, _distance, m_Lm))
        {

            float calcSpeed = knifeSpeed * info.distance / _distance;
            DOTween.Init();
            
            throwKnife.DOMove(throwKnife.position + throwKnife.forward * info.distance, calcSpeed).OnComplete(() => DamageEnem(info.collider.GetComponent<Enemy>(), throwKnife));
            
        }
        else
        {
            throwKnife.DOMove(throwKnife.position + throwKnife.forward * _distance, knifeSpeed).OnComplete(() => Destroy(throwKnife.gameObject));
        }

  
        
        
    }
    private void DamageEnem(Enemy en,Transform des)
    {
        if(en!= null)
        {
            en.MalEn(Damage,GameManager.gameManager.PLAYER.transform.position);
            
        }
        Destroy(des.gameObject);
    }
}
