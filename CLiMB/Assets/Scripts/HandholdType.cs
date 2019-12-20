using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandholdType : MonoBehaviour
{
    public bool requireLeft = false;
    public bool requireTop = false;
    public bool requireRight = false;
    public bool pullUp = false;
    public bool regenItem = false;
    public bool regenRespawn = false;
    public Vector3 pullUpPos;
    public Vector3 stepPos;
    public SortedList<int, string> combo = new SortedList<int, string>(); 

    public bool getRegenRespawn()
    {
        return regenRespawn;
    }

    public void setRegenRespawn(bool val)
    {
        regenRespawn = val;
    }

    public SortedList<int, string> getReqCombo()
    {
        //get the button combination necessary to hold the handhold (contained individually)
        return this.combo;
    }

    public bool getPullUp()
    {
        return pullUp;
    }

    public Vector3 getPullUpPos()
    {
        return pullUpPos;
    }

    public Vector3 getStepPos()
    {
        return stepPos;
    }

    public bool getRegenItem()
    {
        return regenItem;
    }

    private void Start()
    {
        setRegenRespawn(false);
        float xSize = GetComponent<Collider>().bounds.size.x;
        float zSize = GetComponent<Collider>().bounds.size.z;
        Vector3 step = new Vector3(xSize/4, (float)(pullUpPos.y * 0.3), -(zSize*3));

        //print(step);
        stepPos = transform.position + step;
        //print(stepPos);
        pullUpPos = new Vector3(transform.position.x + pullUpPos.x, transform.position.y + pullUpPos.y, transform.position.z + pullUpPos.z);
        //print(pullUpPos);
        //print(this.combo.ContainsValue("left") + ", " + this.combo.ContainsValue("top") + ", " + this.combo.ContainsValue("right"));   // debug for required button combo
        if (requireLeft)
        {
            this.combo.Add(0, "left");
        }
        //center digit
        if (requireTop)
        {
            this.combo.Add(1, "top");
        }
        //right digit
        if (requireRight)
        {
            this.combo.Add(2, "right");
        }
    }

    //Debug
    /*private void Update()
    {
        print(this.combo.ContainsValue("left") + ", " + this.combo.ContainsValue("top") + ", " + this.combo.ContainsValue("right"));   // debug for required button combo
    }*/
}
