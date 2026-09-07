using UnityEngine;
using System;
using System.Collections.Generic;

public enum CharacterProfession
{
    Unemployed,
    Mechanic,  
    Laborer,   
    Lumberjack,
    Thief      
}

public class PlayerStats : MonoBehaviour
{
    [Header("Sistema de Creación (Estilo PZ)")]
    public CharacterProfession profession = CharacterProfession.Unemployed;
    public int creationPoints = 5; 

    [Header("Lista de Rasgos Activos")]
    public List<string> activeTraits = new List<string>();

    [Header("Economía")]
    public float cashMoney;      
    public float bankBalance;    

    [Header("Atributos Físicos")]
    public float bodyWeight = 75.0f;     
    public int muscleStrength = 1;       
    
    [Header("Atributos Mentales")]
    public int intellectLevel = 0;       
    private float studyXP = 0f;
    private float xpMultiplier = 1.0f; 

    [Header("Necesidades Básicas (0-100)")]
    [Range(0, 100)] public float hunger = 0f;   
    [Range(0, 100)] public float thirst = 0f;   
    [Range(0, 100)] public float energy = 100f; 

    private void OnEnable()
    {
        TimeManager.OnHourChanged += HandleHourlyUpdate;
    }

    private void OnDisable()
    {
        TimeManager.OnHourChanged -= HandleHourlyUpdate;
    }

    private void Start()
    {
        BuildCharacter();
    }

    private void Update()
    {
        // Prueba rápida en PC: Si presionas la 'C', te comes el primer ítem del inventario
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerInventory inv = GetComponent<PlayerInventory>();
            if (inv != null && inv.inventoryItems.Count > 0)
            {
                // Toma el primer objeto que tengas guardado en la lista y lo consume
                inv.ConsumeItem(inv.inventoryItems[0]);
            }
            else
            {
                Debug.Log("[🎒 MOCHILA VACÍA] No tienes nada que comer. ¡Busca provisiones!");
            }
        }
    }

    public void BuildCharacter()
    {
        ApplyProfession();
        SetupSimulatedTraits();

        if (creationPoints < 0)
        {
            Debug.LogError($"[⚠️ CREACIÓN ILEGAL] Tienes {creationPoints} puntos. ¡No puedes chetarte desde el nivel 0! Elige rasgos negativos para balancear.");
        }
        else
        {
            Debug.Log($"[👤 PERSONAJE LISTO] Creación exitosa. Puntos restantes: {creationPoints}. Profesión: {profession}");
        }
    }

    private void ApplyProfession()
    {
        switch (profession)
        {
            case CharacterProfession.Unemployed:
                creationPoints += 0; 
                cashMoney = 100f;
                bankBalance = 1500f;
                break;

            case CharacterProfession.Mechanic:
                creationPoints -= 6; 
                intellectLevel = 1;  
                cashMoney = 150f;
                bankBalance = 1200f;
                break;

            case CharacterProfession.Laborer:
                creationPoints -= 4; 
                muscleStrength = 3;  
                bodyWeight = 82.0f;  
                cashMoney = 50f;
                bankBalance = 800f;
                break;

            case CharacterProfession.Lumberjack:
                creationPoints -= 5; 
                muscleStrength = 2;
                cashMoney = 80f;
                bankBalance = 950f;
                break;

            case CharacterProfession.Thief:
                creationPoints -= 8; 
                cashMoney = 400f;   
                bankBalance = 200f;  
                break;
        }
    }

    private void SetupSimulatedTraits()
    {
        if (profession == CharacterProfession.Thief)
        {
            AddTrait("Sobrepeso", true); 
        }

        if (profession == CharacterProfession.Unemployed)
        {
            AddTrait("Musculoso", false); 
        }
    }

    public void AddTrait(string traitName, bool isNegative)
    {
        activeTraits.Add(traitName);

        if (isNegative)
        {
            if (traitName == "Sobrepeso") { creationPoints += 3; bodyWeight = 98.0f; }
            if (traitName == "Lento de Aprendizaje") { creationPoints += 4; xpMultiplier = 0.6f; }
        }
        else
        {
            if (traitName == "Musculoso") { creationPoints -= 4; muscleStrength = 4; }
            if (traitName == "Erudito") { creationPoints -= 3; xpMultiplier = 1.3f; }
        }
    }

    private void HandleHourlyUpdate()
    {
        hunger += 2.5f;   
        thirst += 3.5f;   
        energy -= 1.8f;   

        if (bodyWeight > 95.0f)
        {
            energy -= 1.8f; 
        }

        hunger = Mathf.Clamp(hunger, 0f, 100f);
        thirst = Mathf.Clamp(thirst, 0f, 100f);
        energy = Mathf.Clamp(energy, 0f, 100f);
    }
}