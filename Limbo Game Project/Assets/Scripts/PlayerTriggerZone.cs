using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTriggerZone : MonoBehaviour
{
    public PlayerTriggerType TriggerType;
    bool Fired = false;

    public enum PlayerTriggerType
    {
        somethingLikeVictory,
        somethingLikeDeath
    }

    public UnityEvent onPlayerHitTrigger;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (Fired)
        {
            return;
        }

        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            switch (TriggerType)
            {
                case PlayerTriggerType.somethingLikeVictory:
                    player.setPlayerWin();
                    Fired = true;
                    break;
                case PlayerTriggerType.somethingLikeDeath:
                    player.setPlayerDeath();
                    Fired = true;
                    break;
            }
            onPlayerHitTrigger.Invoke();
        }
    }
    public void Start()
    {
        Fired = false;
    }
}