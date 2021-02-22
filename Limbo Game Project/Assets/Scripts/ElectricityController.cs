using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent onTogglePowerOn;
    public UnityEvent onTogglePowerOff;

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
        onTogglePowerOn.Invoke();
    }

    private void TurnOffLights()
    {
        sprite.sprite = unlitSprite;
        onTogglePowerOff.Invoke();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(isPowered == true)
        {
            if(!lightsOn)
            {
                var player = collision.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.setPlayerDeath();
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
