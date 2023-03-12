using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// script responsable for managing the spawing and despawing/ controll on the cups that slide down the wasshing board
/// </summary>



public class CupManager : MonoBehaviour
{
    [SerializeField] private MenuController menuController;
    [SerializeField] private List<Animator> cupAnimators = new List<Animator>();  //store a list of all the available items

    //game difficulty settings
    private float gravity;                  //the higher this is the faster the balls will swing and fall

    

    private float cupSpeed = 0.2f;          //the higher this is the faster the cups will move

    public void Start()
    {
        //assigne this class as reference for the cups
        foreach (Animator animator in cupAnimators)
        {
            animator.gameObject.GetComponent<Cup>().cupManager = this;
        }

        InvokeRepeating("SpawnCups", 0, 5);//call the spawncups method every x seconds        
    }


    private int currentIndex;
    private void SpawnCups()
    {
        //spawn some cups in for the player, use random for this but if ebnough time create speratre lists for different diffucklty containers and slowly ram up the diffuclty of the containers
        //check how high the players score is and adjust the difficulty accordingly as the score gets higher

        //spawn a cup from the list, keep track of the index
        if (currentIndex < cupAnimators.Count)
        {
            //is within range
            cupAnimators[currentIndex].SetTrigger("Play");
            cupAnimators[currentIndex].GetComponent<Cup>().donut = false;
            currentIndex++;
        }
        else
        {
            //outside range, loop back to start
            cupAnimators[0].SetTrigger("Play");
            currentIndex = 1;
        }
    }

    private void UpdateSpeed(float newSpeed)
    {
        foreach(Animator animator in cupAnimators)
        {
            animator.speed = newSpeed;
        }
    }

    private int donutStreak;
    internal void HitFloor(bool donut)
    {
        //when the donut hits the floor, if it has received a donut then add to streak, if no donut break a life
        if (donut)
        {
            donutStreak++;
            if (donutStreak >= 3)
            {
                donutStreak = 0;
                menuController.LifeBack();
            }
        }
        else
        {
            donutStreak = 0;
            menuController.MugSmash();
        }
    }




    public void Pause()
    {
        UpdateSpeed(0);
    }
    public void UpPause()
    {
        UpdateSpeed(cupSpeed);
    }
}
