using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityController : MonoBehaviour
{   
    public static ElectricityController instance { get; private set; }
    public float electricityInterval = 5.0f;
    private float electricityTimer;
    private bool lightsOn = true;
    public Sprite litSprite;
    public Sprite unlitSprite;
    private SpriteRenderer sprite;
    private static bool isPowered = true;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        electricityTimer = electricityInterval;
    }

    void Update()
    {
        if(isPowered == true)
        {
            electricityTimer -= Time.deltaTime;
            if(electricityTimer < 0)
            {
                lightsOn = !lightsOn;
                electricityTimer = electricityInterval;
            }

            if(lightsOn)
            {
                TurnOffLights();
            }
            else
            {
                TurnOnLights();
            }
        }
        else
        {
            sprite.sprite = unlitSprite;
        }   
    }

    private void TurnOnLights()
    {
        sprite.sprite = litSprite;
    }

    private void TurnOffLights()
    {
        sprite.sprite = unlitSprite;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(isPowered == true)
        {
            if(!lightsOn)
            {
                if(collision.gameObject.layer == 10)
                {
                    Destroy(collision.gameObject);
                } 
            }   
        }
        
    }

    public void SetIsPowered(bool inPowered)
    {
        isPowered = inPowered;
    }

    public bool GetIsPowered()
    {
        return isPowered;
    }
}
