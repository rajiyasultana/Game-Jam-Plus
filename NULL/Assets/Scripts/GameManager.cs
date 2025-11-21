using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Unlocked Abilities")]
    // We use a List so you can see what you have in the Inspector
    public List<AbilityType> unlockedAbilities = new List<AbilityType>();

    void Awake()
    {
        // Singleton Pattern: Ensures only one GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps this object when changing scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this to unlock something
    public void UnlockAbility(AbilityType ability)
    {
        if (!unlockedAbilities.Contains(ability))
        {
            unlockedAbilities.Add(ability);
            Debug.Log("GameManager: Unlocked " + ability);
            
            // Optional: Trigger UI updates or Save System here
        }
    }

    // Call this to check if we can do something
    public bool HasAbility(AbilityType ability)
    {
        return unlockedAbilities.Contains(ability);
    }
}