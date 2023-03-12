using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// class to ascribe behavior to the ball, will detect when certain triggers are entered, when the ball starts loosing momentumj and needs a topup and any otehr behavior the ball will need
/// </summary>




public class BallBehavior : MonoBehaviour
{
    [Header("Script Refs")]
    [SerializeField] private MenuController menuController;
    [SerializeField] private SoundController soundController;

    [Header("Object Refrerences")]
    [SerializeField] private HingeJoint hingeJoin;              //this needs to be set in the editir
    [SerializeField] private LayerMask layerMask;               //what layer should the cut press be detected on
    [SerializeField] private GameObject rope;                   //the cylinder acting as a rope visual, this will need to be destroyed deactivated when donut is cut free
    [SerializeField] private Transform ropeOragin;              //the oragin point of the rope

    [Header("RespawnPoints")]
    [SerializeField] private Transform respawnPoint;            //a possable repawn point for the doughnut


    private Rigidbody rigidBody;
    private bool ballTooLow = false;
    private bool ropeCut;
    private bool scored;                                        //use this to keep track of when an instacne of a donut has scored and dont rest it until it has been respawned
    private bool spawning;                                      //if set to true then teh doughnut will be on the lookout for a hinge to connect itself with


    public void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    public void OnTriggerEnter(Collider other)
    {
        //when the ball enteres a trigger
        if (other.gameObject.tag == "Stop")
        {
            //the ball has reached the desired height for this swing, stop applying force
            ballTooLow = false;
        }
        else if (other.gameObject.tag == "Goal" && !scored)
        {
            //the donut has reached a goal and has not scored befor, add a score point
            soundController.PlaySoundEffect("Splash", true);
            ScorePoints();
            RespawnDonut();
        }
        else if (other.gameObject.tag == "Connector" && spawning)
        {
            HookupDonut(other.transform.position);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        //when the ball exits a trigger zone
        if (other.gameObject.tag == "Stop")
        {
            //the ball has dropped below the desired height, start adding force again
            ballTooLow = true;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        //when colliding with a solid object, check if this is the floor tag, if so delete the and raise a game end trigger
        
        if (collision.gameObject.tag == "Floor")
        {
            soundController.PlaySoundEffect("DonutFloor", false);
            //menuController.LoadDeathScreen(true);//dont die on donut table anymore
            RespawnDonut();
        }
        else
        {
            soundController.PlaySoundEffect("GlassTap", false);
            //has hot rim of cup or otehr such item
        }
    }




    public void Update()
    {
        if (ropeCut) { return; }//if the rope has been cut dont check for player input or force requirements

        //check to see if the player has tapped the screen (e.g. cutting the rope)
        if (Input.touchCount > 0)
        {
            if (IsMouseOverCutZone())
            {
                CutRope();
            }
        }        

        //does the ball need more force to equize pendulum swing (only apply this if the rope is still intact)
        if (ballTooLow && !ropeCut)
        {
            ApplyPendulumForce();
        }
    }

    public void NewLife()
    {
        //the player has paid for a new life/lives
        RespawnDonut();
    }

    private void CutRope()
    {
        //the player has decided to cut the rope, disconnect the hinge and mod necisary booleans
        if (!ropeCut)
        {
            ropeCut = true;
            hingeJoin.connectedBody = null; //disconect the hinge(this will be reused to connect the RB in teh future)
            rope.SetActive(false);
        }        
    }    
    private void ScorePoints()
    {
        scored = true;
        Score.AddScore(1);
    }



    #region donut spawing
    private void RespawnDonut()
    {
        //this will move the donut to the starting position and initate the reattachment sequence for gameplay to continue

        //reset the donuts tracked values        
        spawning = true;

        //remove momentum and rotation from the dougnut
        rigidBody.velocity = new Vector3();
        rigidBody.angularVelocity = new Vector3();

        //move the donut to a respawn point
        transform.position = respawnPoint.position;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        rope.SetActive(true);
    }
    private void SpawnDecoyDonut()
    {
        //if a donut lands in a goal we dont dont to get rid of it, so spawn a decony donut that has no fucntionality only visuals and parent it to the object it has landed on so we can keep using the current donut
    }
    private void HookupDonut(Vector3 pos)
    {
        //hook the doughnut back up to a hinge so it can swing, make sure to rest the hinge oragin to properly light up teh rope        
        hingeJoin.connectedBody = rigidBody;
        hingeJoin.connectedAnchor = new Vector3();

        //now the donut is back on teh hinge wec an renable some of its behavior
        spawning = false;
        scored = false;
        ropeCut = false;

        //make sure the rope is aligned with point, lerp the position to place it at

        Vector3 ropePos = Vector3.Lerp(ropeOragin.position, hingeJoin.transform.position, 0.5f);
        rope.transform.position = ropePos;

        rope.transform.LookAt(hingeJoin.transform);
    }
    #endregion


    private void ApplyPendulumForce()
    {
        //check what direction the ball is going in and apply a foce to push it in that direction, apply a modifier based on gravitional strength 
        if (rigidBody.velocity.y > 0)
        {
            rigidBody.AddForce(new Vector3(0, -Physics.gravity.y * 1.5f * Time.deltaTime, 0));
        }
        else if (rigidBody.velocity.y < 0)
        {
            rigidBody.AddForce(new Vector3(0, (Physics.gravity.y) * 1.5f * Time.deltaTime, 0));
        }
    }



    //variable for raycasting methods
    private Ray ray;
    private RaycastHit hit;
    private bool IsMouseOverCutZone()
    {
        //generate a ray from the camera perspective to touch position on the screen, checkign for cut zone
        ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
        if (Physics.Raycast(ray, out hit, 10, layerMask))
        {
            return true;
        }
        return false;
    }



}
