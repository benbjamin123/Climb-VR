using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripManager : MonoBehaviour {

    public AudioSource ambient;
	public Rigidbody Body;
    public FallDamage fallDamage;
	public ClimbFunctionality left;
	public ClimbFunctionality right;
	public GlobalVars global;
    public GameObject regenItems;
    private respawnRegen itemsScript;
    //public SteamVR_TrackedObject cameraRig;
    public Camera camera;
    public BoxCollider collider;
    public GameObject[] collidingWalls;
    public GameObject environment;
	bool leftGripped = false;
	bool rightGripped = false;
    public float xdiff = 0;
    public float ydiff = 0;
    public float zdiff = 0;
    Vector3 newdiff;
    public ClimbFunctionality lastGripped;
    public Vector3 releaseVel;
    public bool anyCanGrip;
    public bool leftHover = false;
    public bool rightHover = true;
    public bool wasGripping = false;
    private bool activatePullUp = false;
    private Vector3 pullPos;
    private bool doingPullUp = false;
    private float timeCount = 0.0f;
    private float startTime;
    private float speed1 = 0.0001f;
    private float speed2 = 0.00001f;
    private Vector3 newPos;
    private bool hittingWall=false;
    private Vector3 origPos, currPos, posDiff, newPosPortion, portion;
    private GameObject regenItem;
    private int inputSize;
    private Vector3 checkpoint;
    public bool moving = false;
    public bool canDie = false;
    public bool doRespawn = false;

    public void setCanDie(bool val)
    {
        canDie = val;
    }

    public bool getCanDie()
    {
        return canDie;
    }

    public void setDoRespawn(bool val)
    {
        doRespawn = val;
    }

    public bool getDoRespawn()
    {
        return doRespawn;
    }

    public Vector3 getCheckpoint()
    {
        return checkpoint;
    }

    public void setCheckpoint(Vector3 newPos)
    {
        checkpoint = newPos;
    }

    // Use this for initialization
    void Start () {
        checkpoint = Body.transform.position;
        collider = GetComponent<BoxCollider>();
        itemsScript = regenItems.GetComponent<respawnRegen>();
	}

    private void movePlayerBody(ClimbFunctionality device)
    {
        moving = true;
        setCanDie(false);
        //Move the player's body relative to the gripping controller (main movement function)
        //collider.size = new Vector3(0.4f, camera.transform.localPosition.y, 0.4f);
        Body.useGravity = false;
        Body.isKinematic = true;
        /*xdiff = (device.prevPos.x - device.transform.localPosition.x);
        ydiff = (device.prevPos.y - device.transform.localPosition.y);
        zdiff = (device.prevPos.z - device.transform.localPosition.z);
        newdiff.Set(xdiff, ydiff, zdiff);
        Body.transform.position += newdiff;*/
        inputSize = device.getGrab()-1;
        //portion = new Vector3(inputSize/3, inputSize/3, inputSize/3);
        newPosPortion = (device.prevPos - device.transform.localPosition) * inputSize/2;
        Body.transform.position += newPosPortion;
        if (!hittingWall)
        {
            origPos = Body.transform.position;
        }
        else
        {
            currPos = Body.transform.position;
            posDiff = currPos - origPos;
            environment.transform.position -= posDiff;
            print("hitting wall");
        }
        global.setGripping(true);
        lastGripped = device;
        if (inputSize == 2)
        {
            //only throw if the player has momentum
            releaseVel = (device.prevPos - device.transform.localPosition) / Time.deltaTime;
        }
        wasGripping = true;
    }

    private void moveRegenItem()
    {

    }

    public IEnumerator pullUpAnim(ClimbFunctionality device)
    {
        //print("pull up");
        left.setGripping(false);
        right.setGripping(false);
        left.GetComponent<Rigidbody>().isKinematic = false;
        right.GetComponent<Rigidbody>().isKinematic = false;
        //print(Time.time - startTime);
        Vector3 newPos = device.getPullUpPos();
        Vector3 stepPos = device.getStepPos();
        //resetting steppos x and  z to ensure only vertical y movement
        stepPos.x = Body.transform.position.x;
        stepPos.z = Body.transform.position.z;
        //print(stepPos);
        var lerpTime = Time.time - startTime;
        while (lerpTime < 1.5)
        {
            //print(stepPos);
            Body.transform.position = Vector3.Slerp(Body.transform.position, stepPos, (Time.time - startTime) * speed1);
            lerpTime = Time.time - startTime;
            yield return null; // leave the coroutine and come back next frame
        }
        while (lerpTime < 4)
        {
            //print(newPos);
            Body.transform.position = Vector3.Slerp(Body.transform.position, newPos, (Time.time - startTime) * speed2);
            lerpTime = Time.time - startTime;
            yield return null; // leave the coroutine and come back next frame
        }
        doingPullUp = false;
        left.GetComponent<Rigidbody>().isKinematic = true;
        right.GetComponent<Rigidbody>().isKinematic = true;
        setCheckpoint(Body.transform.position);
        refillStamina();
    }

    public void OnCollisionEnter(Collision collision)
    {
        //print(collision);
        foreach (ContactPoint contact in collision.contacts)
        {
            //print(contact.otherCollider.GetComponent<MeshCollider>());
            foreach (GameObject wall in collidingWalls)
            {
                //print(wall.GetComponent<MeshCollider>());
                if (wall.GetComponent<MeshCollider>() == contact.otherCollider.GetComponent<MeshCollider>())
                {
                    //print("hit wall");
                    hittingWall = true;
                }
                /*else if (wall.GetComponent<Killable>().getCanKill() == true)
                {
                    //print("can die");
                    setCanDie(true);
                }**/
            }
        }
        //setCanDie(false);
    }

    public void OnCollisionExit(Collision collision)
    {
        hittingWall = false;
        //setCanDie(false);
    }

    public void respawn()
    {
        //print("respawn");
        Body.transform.position = checkpoint;
        fallDamage.setRespawn(false);
        refillStamina();
        setCanDie(false);
        setDoRespawn(false);
        itemsScript.respawnItems();
        Body.velocity = new Vector3(0, 0, 0);
    }

    public void refillStamina()
    {
        left.setStamina(left.getStaminaMax());
        right.setStamina(right.getStaminaMax());
    }

    // Update is called once per frame
    void FixedUpdate () {

        var ldevice = SteamVR_Controller.Input((int)left.controller.index);
        var rdevice = SteamVR_Controller.Input((int)right.controller.index);
        //print(camera.transform.localPosition);

        if (fallDamage.getRespawn())
        {
            respawn();
        }
        //print(right.canGrip && right.gripping && !leftGripped);

        anyCanGrip = left.canGrip || right.canGrip;
        //print(anyCanGrip);

        if(left.getGrabbingRegen() || right.getGrabbingRegen())
        {
            left.refillStamina();
            right.refillStamina();
        }

        if (doingPullUp)
        {
            StartCoroutine(pullUpAnim(left));   //either hand will work, they're using the same pull up ledge
        }
        else if (left.getDoPullUp() && right.getDoPullUp())
        {
            startTime = Time.time;
            //newPos = left.getPullUpPos();   //either hand will work, they're using the same pull up ledge
            doingPullUp = true;
        }
        else if (anyCanGrip)
        {
            //right
            //successful grab
            if (right.canGrip && right.gripping)
            {
                //feedback
                if (!rightHover)
                {
                    SteamVR_Controller.Input((int)right.controller.index).TriggerHapticPulse(4000);
                    rightHover = true;
                }
                //body movement
                movePlayerBody(right);
                rightGripped = true;
                leftGripped = false;
            }
            //let go\not grabbing
            else if (right.canGrip && !right.gripping)
            {
                Body.useGravity = true;
                Body.isKinematic = false;
                //Body.velocity = (right.prevPos - right.transform.localPosition) / Time.deltaTime;
            }
            else if (!right.canGrip)
            {
                rightHover = false;
            }
            //left
            //successful grab
            if (left.canGrip && left.gripping)
            {
                //feedback
                if (!leftHover)
                {
                    SteamVR_Controller.Input((int)left.controller.index).TriggerHapticPulse(4000);
                    leftHover = true;
                }
                movePlayerBody(left);
                leftGripped = true;
                rightGripped = false;
            }
            else if (left.canGrip && !left.gripping && !right.gripping)
            {
                Body.useGravity = true;
                Body.isKinematic = false;
                //Body.velocity = (left.prevPos - left.transform.localPosition) / Time.deltaTime;
            }
            else if (!left.canGrip)
            {
                leftHover = false;
            }
            if (!left.gripping && !right.gripping && wasGripping)
            {
                Body.velocity = releaseVel;
                wasGripping = false;
            }
        }
        else
        {
            Body.useGravity = true;
            Body.isKinematic = false;
            global.setGripping(false);
            rightGripped = false;
            leftGripped = false;
        }



		left.prevPos = left.transform.localPosition;
		right.prevPos = right.transform.localPosition;

        collider.size = new Vector3(0.25f, camera.transform.localPosition.y, 0.25f);
        collider.center = new Vector3(camera.transform.localPosition.x, camera.transform.localPosition.y - (collider.size.y / 2), camera.transform.localPosition.z);
        moving = false;
    }
}
