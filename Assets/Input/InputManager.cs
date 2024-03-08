using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public Abilities abilities;
    Actions actions;
    private void Awake()
    {
        Instance = this;
        actions = new Actions();
        actions.Enable();
        actions.Abelities.CastSpell.performed += _ => abilities.CastAbility(); 
        actions.Abelities.Supernova.performed += _ => abilities.Supernova(); 
        actions.Abelities.Blackhole.performed += _ => abilities.Blackhole(); 
    }
}
