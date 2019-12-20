using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour {
	public Rigidbody Body;
    public GripManager manager;
	public bool isPlaying;
	public float lastY;
	public float fallSoundDelay = 1f;
	public bool deadlyFall = false;
	public AudioSource DeathSound;
	public AudioSource FallSound;
	public GlobalVars global;
	public Sprite blackScreen;
	public Color temp;
	public float lastGrip;
    private Vector3 spawnPoint;
    private float yDiff;
    private bool respawn;
    private float num;
    public bool isFalling = false;

    public bool getRespawn()
    {
        return respawn;
    }
    public void setRespawn(bool newVal)
    {
        respawn = newVal;
    }

    private Vector3 origin;
	public void Start(){
        spawnPoint = Body.GetComponent<GripManager>().getCheckpoint();
        origin = Body.transform.position;
        lastY = Body.transform.position.y;
		deadlyFall = false;
		isPlaying = true;
		//blackScreen = GetComponent<SpriteRenderer> ();
		//temp = blackScreen.color;
		lastGrip = 0.0f;
	}
	public void Update(){
        yDiff = Body.position.y - origin.y;
		if (lastGrip >= 3.5f) {
			deadlyFall = true;
		} else {
			deadlyFall = false;
		}
        //Debug.Log (isPlaying);
        //Debug.Log (lastY);
        //Debug.Log (Body.transform.position.y);
        //Debug.Log (Body.transform.position.y < lastY / 2);
        //Debug.Log (GameObject.Find ("Controller (Right)").GetComponent<ClimbFunctionality> ().canGrip);
        num = yDiff - lastY;
        if (num <= 0 && num >= -0.01)
        {
            FallSound.Stop();
            isPlaying = false;
        }
        else if (yDiff < lastY)
        {
            if (yDiff < lastY - 0.1)
            {
                isFalling = true;
            }
            if (global.getGripping())
            {
                FallSound.Stop();
                isPlaying = false;
                isFalling = false;
            }
            else if (deadlyFall && !isPlaying && yDiff < (lastY - 0.075))
            {
                FallSound.Play();
                isPlaying = true;
            }
            /*print("");
            print((isFalling));
            print(Body.velocity.y <= 0);
            print(Body.velocity.y);
            print(!manager.moving);*/
            //print(manager.getCanDie());
            //print(isFalling);
            if (manager.getCanDie() && isFalling)
            {
                //print("dead");
                DeathSound.Stop();
                setRespawn(true);
                //spawnPoint = Body.GetComponent<GripManager>().getCheckpoint();
                //Body.MovePosition (spawnPoint);
                if (deadlyFall)
                {
                    // Fallen from a deadly height
                    DeathSound.Play();
                    FallSound.Stop();
                    //cutToBlack ();
                }
            }
        }

		if (global.getGripping()) {
            lastGrip = yDiff;
		} 

		lastY = yDiff;
        isFalling = false;

	}

	public void cutToBlack(){
		//temp.a = 1f;
		//blackScreen.color = temp;
	}
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallDamage : MonoBehaviour {
	public Rigidbody Body;
	public bool falling = false;
	public float lastY;
	public AudioSource DeathSound;
	public AudioSource FallSound;
	public Image BlackScreen = GameObject.Find ("Canvas/BlackScreen").GetComponent<Image> ();

	private Vector3 origin = new Vector3(0,0,-1);

	public void Start(){
		lastY = Body.transform.position.y;
	}
	public void Update(){
		if (Body.transform.position.y == lastY / 2) {
			FallSound.Stop ();
		}
		else if (Body.transform.position.y < lastY/2){
			FallSound.Play (44100);
			if (lastY > 0) {
				DeathSound.Stop ();
				FallSound.Stop ();
				Body.MovePosition (origin);
				DeathSound.Play ();
				BlackScreen.color = new Color (0, 0, 0, 255);
			}
		} 
		//while(GameObject.Find("BlackScreen").GetComponent<Image>().color
		lastY = Body.transform.position.y;
	}
}
**/