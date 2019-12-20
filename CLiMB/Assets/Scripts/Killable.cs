using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour
{
    private static bool canKill = true;

    public bool getCanKill()
    {
        return canKill;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<GripManager>().getDoRespawn())
        {
            other.gameObject.GetComponent<GripManager>().setCanDie(true);
        }
    }

    /*public void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.GetComponent<GripManager>().getDoRespawn())
        {
            other.gameObject.GetComponent<GripManager>().setCanDie(false);
        }
    }**/

    public void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<GripManager>().setDoRespawn(true);
    }
}
