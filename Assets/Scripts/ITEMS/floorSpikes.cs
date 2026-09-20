using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/floorSpikes")]
public class floorSpikes : Items
{

    public string _Name = "floor Spikes";
    public Texture _imageUI;

    public int _addAmount = 2;
    [Space]
    public Rigidbody spikes;

    public float amountOfSpikes, distanceFromPlayerAppear, speed,randSpeed,randRotation;

    //With This you can create a bunch of item:
    //item 1: place a sticky script that only sticks to the floor and then add the activate damage script with another collider
    //item 2: give the random rotation 360 degrees and then set the spikes to bullets like the ones from shotgun
    //item 3: 
    [SerializeField]
    private LayerMask lm;
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
        for (int i = 0; i < amountOfSpikes; i++)
        {
            Quaternion randRot =Quaternion.Euler(0, Random.Range(-randRotation * .5f, randRotation * .5f),0);
            Rigidbody rbSpikes = Instantiate(spikes, player.transform.position + player.transform.forward * distanceFromPlayerAppear, player.transform.rotation);
            Vector3 rot = randRot * player.transform.forward;
            rbSpikes.velocity = rot * (speed + Random.Range(0, randSpeed));
        }
       

    }
}
