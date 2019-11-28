using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float SpeedBost = 3;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            PickUp(other);

    }

    void PickUp(Collider player)
    {
        Debug.Log("Speed picked up");

        Player_movement movement = player.gameObject.GetComponent<Player_movement>();

        if(movement != null)
        {
            Debug.Log("speeding up");
            movement.SpeedUp();
        }
         
        Destroy(gameObject);

    }
}