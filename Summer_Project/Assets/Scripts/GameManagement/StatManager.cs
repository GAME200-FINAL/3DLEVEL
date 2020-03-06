using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour {

    public float maxHealth = 100;
    private float currentHealth;
    private PlayerStates.STATE currentState = PlayerStates.STATE.NORMAL;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void ChangeHealth(float increment)
    {
        currentHealth += increment;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        if (currentHealth < 0)
            currentHealth = 0;
        
    }

    public void resetHealth()
    {
        currentHealth = maxHealth;
    }

    public void setState(PlayerStates.STATE state)
    {
        currentState = state;
    }

    public PlayerStates.STATE getCurrentState()
    {
        return currentState;
    }

}
