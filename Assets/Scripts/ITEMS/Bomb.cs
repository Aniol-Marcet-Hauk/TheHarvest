using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/Bombs")]
public class Bomb : Items
{
    public string _Name = "Bomb";
    public Texture _imageUI;

    public int _addAmount = 2;
    [Space]
    public Rigidbody bomb;
    
    public float speed,timeExplode,distanceFromPlayerAppear = 1;

    //ToCreate Bomb that explodes on impact just change on the gameobjectofthebomb: isThrown= false;
    //StickyBomb is exactly this but give the gameobjectofthebomb a script that makes it stick
    //Posa amount a infinity i velocitat a 0 amb temps 0 i distacnia 0 per crear com una explosio interna o algo aixi
    //posa stickybomb amb una velocitat baixa i temps infinit i al objecte isThrown=True per crear una mina//(aqui has de fer un lm pk ignori explotar al tocar al terra i ficar-li dos colliders(un pel sticky un per explotar
    //TAMBE POTS CREAR BOMBES QUE FAN QUE ELS ENEMICS ES CONFONGUIN, QUE ES CONGELIN, QUE ES POSIN EN FOC...
    //[SerializeField]
    //private LayerMask lm;// i dont think i need this layermask?
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
        Rigidbody rbBomb = Instantiate(bomb, player.transform.position + player.transform.forward*distanceFromPlayerAppear, player.transform.rotation);
        rbBomb.velocity = player.transform.forward * speed;
        player.StartCoroutine (TimeToExplode(rbBomb.GetComponent<Animator>()));
    }
    IEnumerator TimeToExplode(Animator an)
    {
        if(timeExplode >= 0)
        {
            yield return new WaitForSeconds(timeExplode);
    
            an.Play("Explosion");


        }

    }
}

