using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is Null!!!");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }


    private bool basePlaced = false;
    public GameObject ball;
    private GameObject spawnedBall;
    private float force = 2000.0f;
    private float timer = 1.0f;


    // Update is called once per frame
    void Update()
    {
        if (basePlaced && timer >0)
        {
            timer -= Time.deltaTime;
        }
        if (timer < 0 && Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began)
        {
  
            
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            //start spawn enemy
            spawnedBall = Instantiate(ball, Camera.current.transform.position, Camera.current.transform.rotation);
            spawnedBall.GetComponent<Rigidbody>().AddForce(Camera.current.transform.forward * force, ForceMode.Force);
        }
    }

    public void startGame()
    {
        basePlaced = true;
    }

    public bool BasePlaced()
    {
        return basePlaced;
    }
}
