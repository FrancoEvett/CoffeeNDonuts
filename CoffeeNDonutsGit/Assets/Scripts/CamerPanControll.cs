using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// script to allow the camer to be panned left to right in the game scene, using touch input
/// </summary>


public class CamerPanControll : MonoBehaviour
{
    [SerializeField] private Transform rotationObject;
    [SerializeField] private LayerMask layerMask;

    public float rot;



    public void Start()
    {
        //get the camer to look sirectly at derires rotationaland focus point
        transform.LookAt(rotationObject);
    }
    public void Update()
    {
        rot = rotationObject.rotation.y;
        //check if the screen has been touched
        if (Input.touchCount > 0)
        {
            //test where the touch is, if its in the pan zone then pan the camer            
            if (IsMouseOverPan())
            {
                PanCamera();
            }
            
        }
    }


    //variable for raycasting methods
    private Ray ray;
    private RaycastHit hit;
    private bool IsMouseOverPan()
    {
        //generate a ray from the camera perspective to touch position on the screen
        ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
        if (Physics.Raycast(ray, out hit, 10, layerMask))
        {
            //the raycast hit the desired pan mask object, allow teh camer to pan
            return true;
        }
        return false;        
    }

    private void PanCamera()
    {
        //take the directional change of the touch inputs on the phones x axis and apply a rotation to the camer pivots y axis to effect rotation
        //(panning sense will feel very low in the editor but on mobile devices the base senstivuty is much higher)

        rotationObject.Rotate(new Vector3(0, 10 * Input.touches[0].deltaPosition.x * Time.deltaTime, 0));

        if (rotationObject.rotation.y < -0.575f)
        {
            rotationObject.eulerAngles = new Vector3(0, -70, 0);
        }
        else if (rotationObject.rotation.y > 0.427f)
        {
            rotationObject.eulerAngles = new Vector3(0, 50, 0);
        }

    }



}
