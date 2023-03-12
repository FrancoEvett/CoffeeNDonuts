using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// script that each cup will have just to record weather it had a donut lan in it when teh cup hits the floor
/// </summary>

public class Cup : MonoBehaviour
{
    public CupManager cupManager;
    public bool donut;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
            cupManager.HitFloor(donut);
        }
        else if (other.gameObject.tag == "Donut")
        {
            donut = true;
        }
    }
}
