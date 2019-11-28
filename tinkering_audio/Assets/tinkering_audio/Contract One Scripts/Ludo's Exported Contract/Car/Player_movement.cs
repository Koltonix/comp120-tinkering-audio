using UnityEngine;
public class Player_movement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform Target;
    public AudioSource asStart;
    public AudioSource asLoop;
    
    private float normalSpeed ;
    public float BoostedSpeed;
    public float speedCoolDown;
    public float speed;
    public float SpeedDuration = 5f;
    
    public float ForwardForce = 2f;
    public float SidewaysForce = 2f;
    
    private bool started;
    private bool moving;
    private bool boost = false;

    private void Start()
    {
        normalSpeed = speed;
        started = false;
        moving = false;
    }

    void FixedUpdate ()
    {
        
        //Movement WASD
        
        rb.AddForce(0, 0, 0 * Time.deltaTime);

        if (Input.GetKey("w"))
        {
            rb.AddForce(0, 0, ForwardForce * speed * Time.deltaTime *1.1f);
        }

        if (Input.GetKey("s"))
        {
            rb.AddForce(0, 0, -ForwardForce * Time.deltaTime *1.5f);
        }

        if (Input.GetKey("a"))
        {
            rb.AddForce(-SidewaysForce * Time.deltaTime, 0, 0 );
        }

        if (Input.GetKey("d"))
        {
            rb.AddForce(SidewaysForce * Time.deltaTime, 0, 0 );
        }
    }   

    //speed boost
    public void SpeedUp()
    {
        if (!boost)
        {
            speed += BoostedSpeed;
            boost = true;
            Invoke("SpeedUp", 2000);
        }
        else
        {
            speed -= BoostedSpeed;
            boost = false;
        }
    }
}