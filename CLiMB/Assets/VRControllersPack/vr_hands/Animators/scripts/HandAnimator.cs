using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimator : MonoBehaviour
{
    private Animator animator;
    public SteamVR_TrackedController controller;
    private bool finger3 = false;
    private bool finger4 = false;
    private bool finger5 = false;
    private bool fist = false;
    private bool idle = false;
    private bool grab = false;
    private bool grab_pistol = false;
    private bool like = false;
    private bool shaka = false;
    private bool allFalse = false;
    private List<string> currTrues = new List<string>();
    private int inputSize = 0;
    private int prevSize = 0;

    private void setAllFalse()
    {
        //reset all bools to false
        animator.SetBool("finger3", false);
        animator.SetBool("finger4", false);
        animator.SetBool("fist", false);
        animator.SetBool("grab", false);
        animator.SetBool("grab_pistol", false);
        animator.SetBool("idle", false);
        animator.SetBool("like", false);
        animator.SetBool("shaka", false);
        finger3 = false;
        finger4 = false;
        fist = false;
        grab = false;
        grab_pistol = false;
        idle = false;
        like = false;
        shaka = false;
    }

    private void setOthersFalse(string name)
    {
        //print("setOthersFalse");
        /*if (!name.Equals("finger3"))
        {
            animator.SetBool("finger3", false);
            finger3 = false;
        }
        if (!name.Equals("finger4"))
        {
            animator.SetBool("finger4", false);
            finger4 = false;
        }*/
        if (!name.Equals("fist"))
        {
            animator.SetBool("fist", false);
            fist = false;
        }
        if (!name.Equals("grab"))
        {
            animator.SetBool("grab", false);
            grab = false;
        }
        if (!name.Equals("idle"))
        {
            animator.SetBool("idle", false);
            idle = false;

        }
        /*if (!name.Equals("grab_pistol"))
        {
            animator.SetBool("grab_pistol", false);
            grab_pistol = false;
        }
        if (!name.Equals("like"))
        {
            animator.SetBool("like", false);
            like = false;
        }
        if (!name.Equals("shaka"))
        {
            animator.SetBool("shaka", false);
            shaka = false;
        }*/
        currTrues.Clear();
    }

    private void manageList()
    {
        //manage list of currently true bools
        /*if (finger3 && !currTrues.Contains("finger3"))
        {
            currTrues.Add("finger3");
        }
        else if(!finger3 && currTrues.Contains("finger3"))
        {
            currTrues.Remove("finger3");
        }
        if (finger4 && !currTrues.Contains("finger4"))
        {
            currTrues.Add("finger4");
        }
        else if (!finger4 && currTrues.Contains("finger4"))
        {
            currTrues.Remove("finger4");
        }*/
        if (fist && !currTrues.Contains("fist"))
        {
            currTrues.Add("fist");
        }
        else if (!fist && currTrues.Contains("fist"))
        {
            currTrues.Remove("fist");
        }
        if (grab && !currTrues.Contains("grab"))
        {
            currTrues.Add("grab");
        }
        else if (!grab && currTrues.Contains("grab"))
        {
            currTrues.Remove("grab");
        }
        if (idle && !currTrues.Contains("idle"))
        {
            currTrues.Add("idle");
        }
        else if (!idle && currTrues.Contains("idle"))
        {
            currTrues.Remove("idle");
        }
        /*if (grab_pistol && !currTrues.Contains("grab_pistol"))
        {
            currTrues.Add("grab_pistol");
        }
        else if (!grab_pistol && currTrues.Contains("grab_pistol"))
        {
            currTrues.Remove("grab_pistol");
        }
        if (like && !currTrues.Contains("like"))
        {
            currTrues.Add("like");
        }
        else if (!like && currTrues.Contains("like"))
        {
            currTrues.Remove("like");
        }
        if (shaka && !currTrues.Contains("shaka"))
        {
            currTrues.Add("shaka");
        }
        else if (!shaka && currTrues.Contains("shaka"))
        {
            currTrues.Remove("shaka");
        }*/
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //all
        //print(prevSize);
        inputSize = controller.GetComponent<ClimbFunctionality>().getInputComboSize();
        if (inputSize == 3 || (inputSize == 2 && prevSize == 3))
        {
            animator.SetBool("fist", true);
            fist = true;
            prevSize = 3;
        }
        else if (inputSize == 1 || (inputSize == 2 && prevSize == 1))
        {
            animator.SetBool("idle", true);
            prevSize = 1;
            idle = true;
        }
        else if(inputSize == 0)
        {
            setAllFalse();
            prevSize = 0;
        }

        manageList();
        if (currTrues.Count > 1)
        {
            setOthersFalse(currTrues[currTrues.Count - 1]);
        }
        if (currTrues.Count == 0)
        {
            allFalse = true;
            animator.SetBool("allFalse", true);
        }
        else
        {
            allFalse = false;
            animator.SetBool("allFalse", false);
        }
        /*if (controller.triggerPressed && controller.padPressed && controller.gripped)
        {
            animator.SetBool("fist", true);
            fist = true;
        }
        //trigger & pad
        else if (controller.triggerPressed && controller.padPressed && !controller.gripped)
        {
            animator.SetBool("like", true);
            like = true;
        }
        //pad & grip
        else if (!controller.triggerPressed && controller.padPressed && controller.gripped)
        {
            animator.SetBool("finger3", true);
            finger3 = true;
        }
        //trigger & grip
        else if (controller.triggerPressed && !controller.padPressed && controller.gripped)
        {
            animator.SetBool("shaka", true);
            shaka = true;
        }
        //just trigger
        else if (controller.triggerPressed && !controller.padPressed && !controller.gripped)
        {
            animator.SetBool("grab_pistol", true);
            grab_pistol = true;
        }
        //just pad
        else if (!controller.triggerPressed && controller.padPressed && !controller.gripped)
        {
            animator.SetBool("finger4", true);
            finger4 = true;
        }
        //just grip
        else if (!controller.triggerPressed && !controller.padPressed && controller.gripped)
        {
            animator.SetBool("grab", true);
            grab = true;
        }
        else
        {
            setAllFalse();
        }*/
    }
}
