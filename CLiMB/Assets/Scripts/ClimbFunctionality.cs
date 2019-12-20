using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ClimbFunctionality : MonoBehaviour {

	public SteamVR_TrackedObject controller;
	public GameObject collidingObject;
    public GameObject handModel;
    private Renderer handRenderer;
    public SortedList<int, string> inputCombo = new SortedList<int, string>();
    private int prevGrab = 0;
    //public HandholdType handhold;
	public AudioSource grip;
    private static double staminaMax = 2000;
    private double staminaInc = 1;
    public double stamina;
    [HideInInspector]
	public Vector3 prevPos;
    [HideInInspector]
    public string controllerName;
    [HideInInspector]
    public bool canGrip;
    public bool gripping = false;   //for gripping handholds
    public bool grabbing = false;   //for grabbing pull-up ledges
    public bool grabbingRegen = false;
    private Color handColor = new Color(1, 0, 0, 1);
    private double handAlpha = 0;
    private bool doPullUp = false;
    private Vector3 pullUpPos;
    private Vector3 stepPos;
    private HandholdType grabbedObj;

    public int getInputComboSize()
    {
        return inputCombo.Count;
    }

    public int getGrab()
    {
        return prevGrab;
    }

    public double getStaminaMax()
    {
        return staminaMax;
    }

    public void setStamina(double val)
    {
        stamina = val;
    }

    public  bool getDoPullUp()
    {
        return doPullUp;
    }

    public void setDoPullUp(bool val)
    {
        doPullUp = val;
    }

    public void setGripping(bool val)
    {
        gripping = val;
    }

    public void setPullUpPos(Collider other)
    {
        pullUpPos = other.GetComponent<HandholdType>().getPullUpPos();
    }

    public Vector3 getPullUpPos()
    {
        return pullUpPos;
    }

    public void setStepPos(Collider other)
    {
        stepPos = other.GetComponent<HandholdType>().getStepPos();
    }

    public Vector3 getStepPos()
    {
        return stepPos;
    }

    public bool getGrabbingRegen()
    {
        return grabbingRegen;
    }

    public void setGrabbingRegen(bool val)
    {
        grabbingRegen = val;
    }
    
    public void refillStamina()
    {
        stamina = staminaMax;
    }

	private void SetCollidingObject(Collider col)
	{
		// Doesn't make the target a potential grab target if the player is already holding something or the target has no rigidbody
		if (collidingObject || !col.GetComponent<Rigidbody>() || !col.CompareTag("Grabbable"))
		{
            //print("doesn't grab");
			return;
		}
		// Assigns the object as a potential grab target
		collidingObject = col.gameObject;
	}

	// Use this for initialization
	void Start () {
		prevPos = controller.transform.localPosition;
        controllerName = controller.name;
        handRenderer = handModel.GetComponent<SkinnedMeshRenderer>();
        var material = handRenderer.material;
        //sets rendering mode to fade
        /*material.SetFloat("_Mode", 2);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;*/
        stamina = staminaMax;
        //save asset so build has a copy
        //AssetDatabase.CreateAsset(material, "Assets/redhand.mat");
        //AssetDatabase.SaveAssets();
    }
    /*void OnTriggerEnter(HandholdType handhold)
    {
        print("handhold");
        print("Handhold Type: " + handhold.getReqCombo());
    }*/

    void OnTriggerEnter(Collider other){
        if (other.GetComponent<HandholdType>()!=null){
            grip.Stop();
            grip.Play();
        }
		SetCollidingObject (other);
        canGrip = true;
        if (collidingObject) {
			SteamVR_Controller.Input((int)controller.index).TriggerHapticPulse (1500);
		}
	}

    public void OnTriggerStay(Collider other)
		{
        //print(other);
        canGrip = true;
        gripping = setGripping(other);
        SetCollidingObject(other);
		}

    public bool setGripping(Collider other)
    {
        //print(canGrip);
        var reqCombo = other.GetComponent<HandholdType>().getReqCombo();
        //int reqKey, inputKey;
        //bool match = false;

        //Debug
        /*print("Input:");
        foreach (System.Collections.Generic.KeyValuePair<int, string> entry in inputCombo)
        {
            print(entry);
        }
        print("Required");
        foreach (System.Collections.Generic.KeyValuePair<int, string> entry in reqCombo)
        {
            print(entry);
        }
        */

        //print(reqCombo.Count);
        //print(inputCombo.Count);
        grabbedObj = other.GetComponent<HandholdType>();
        if (grabbedObj.getRegenItem() && grabbing)
        {
            setGrabbingRegen(true);
            //print(getGrabbingRegen());
            //other.GetComponent<Renderer>().enabled = false;
            grabbedObj.setRegenRespawn(true);
            grabbedObj.GetComponent<MeshRenderer>().enabled = false;
            grabbedObj.GetComponent<MeshCollider>().enabled = false;
        }
        else if (other.GetComponent<HandholdType>().getPullUp() && grabbing)
        {
            setDoPullUp(true);
            setPullUpPos(other);
            setStepPos(other);
            return true;
        }
        else if (inputCombo.Count == 0 || stamina <= 0)
        {
            //print("no input");
            return false;
        }
        /*foreach(System.Collections.Generic.KeyValuePair<int, string> reqEntry in reqCombo)
        {
            reqKey = reqEntry.Key;
            if (!inputCombo.ContainsKey(reqKey))
            {
                //print("missing input");
                return false;
            }
        }
        foreach (System.Collections.Generic.KeyValuePair<int, string> inputEntry in inputCombo)
        {
            inputKey = inputEntry.Key;
            if (!reqCombo.ContainsKey(inputKey))
            {
                //print("extra input");
                return false;
            }
        }*/
        else if (grabbing)
        {
            return true;
        }
        return false;

        /*if (inputCombo.Equals(reqCombo))
        {
            match = true;
        }
        else
        {
            match = false;
        }*/
        //canGrip = match;
    }

	public void OnTriggerExit(Collider other)
	{
        //print("exit");
		canGrip = false;
        gripping = false;
		if (!collidingObject)
		{
			return;
		}
		SteamVR_Controller.Input((int)controller.index).TriggerHapticPulse(1500);
		collidingObject = null;

	}

    public void setHandColor()
    {
        handAlpha = (1 - (stamina / staminaMax));
        handColor = new Color(1, 0, 0, (float)handAlpha);
        handRenderer.material.SetColor("_Color", handColor);
        //print(handRenderer.material.GetColor("_Color"));
    }

    private void FixedUpdate()
    {
        //print(getGrabbingRegen());
        setDoPullUp(false);
        setGrabbingRegen(false);
        if (gripping)
        {
            if (inputCombo.Count == 1 || (inputCombo.Count == 2 && prevGrab  == 1))
            {
                stamina -= staminaInc;
                prevGrab = 1;
            }
            else if (inputCombo.Count == 3 || (inputCombo.Count == 2 && prevGrab == 3))
            {
                stamina -= staminaInc * 3;
                prevGrab = 3;
            }
            else
            {
                prevGrab = 0;
            }
        }
        else if (!gripping)
        {
            //prevent grabs on consecutive frames
            if(stamina < 5)
            {
                stamina += 0.1;
            }
            //recover stamina
            else if (stamina < staminaMax)
            {
                stamina += 1;
            }
        }
        setHandColor();
        if(stamina < 100 && gripping)
        {
            SteamVR_Controller.Input((int)controller.index).TriggerHapticPulse(5000);
        }
        else if (stamina < 300 && gripping)
        {
            SteamVR_Controller.Input((int)controller.index).TriggerHapticPulse(1000);
        }
        var device = SteamVR_Controller.Input((int)controller.index);
        //print(inputCombo.ContainsValue("left") + ", " + inputCombo.ContainsValue("top") + ", " + inputCombo.ContainsValue("right"));   // debug for left controller button combo
        //print("right: " + inputcombo.ContainsValue("left") + ", " + inputcombo.ContainsValue("top") + ", " + inputcombo.ContainsValue("right"));   // debug for right controller button combo
        /*if (left.canGrip)
        {
            left.getObjCombo();
        }*/

        //left controller button combination manager
        if (controllerName.Equals("Controller (left)"))
        {
            //left digit
            if (device.GetPress(SteamVR_Controller.ButtonMask.Grip) && !inputCombo.ContainsKey(0))
            {
                inputCombo.Add(0, "left");
            }
            else if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                inputCombo.Remove(0);
            }
            //center digit
            if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger) && !inputCombo.ContainsKey(1))
            {
                inputCombo.Add(1, "top");
            }
            else if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                inputCombo.Remove(1);
            }
            //right digit
            if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad) && !inputCombo.ContainsKey(2))
            {
                inputCombo.Add(2, "right");
            }
            else if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                inputCombo.Remove(2);
            }
        }

        //right controller button combination manager
        if (controllerName.Equals("Controller (right)"))
        {
            //left digit
            if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad) && !inputCombo.ContainsKey(0))
            {
                inputCombo.Add(0, "left");
            }
            else if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                inputCombo.Remove(0);
            }
            //center digit
            if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger) && !inputCombo.ContainsKey(1))
            {
                inputCombo.Add(1, "top");
            }
            else if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                inputCombo.Remove(1);
            }
            //right digit
            if (device.GetPress(SteamVR_Controller.ButtonMask.Grip) && !inputCombo.ContainsKey(2))
            {
                inputCombo.Add(2, "right");
            }
            else if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                inputCombo.Remove(2);
            }
        }
        if(inputCombo.Count > 0)
        {
            grabbing = true;
        }
        else
        {
            grabbing = false;
        }
    }
}
