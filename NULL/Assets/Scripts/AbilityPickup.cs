using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public AbilityType abilityToUnlock; 
    public GameObject pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.UnlockAbility(abilityToUnlock);
            
            PlayerVisualSwitcher visuals = other.GetComponent<PlayerVisualSwitcher>();

            if (visuals != null)
            {
                // Unlock the Model
                if (abilityToUnlock == AbilityType.CharacterArt)
                {
                    visuals.UpdateVisuals(true); 
                }
                
                // Unlock the Texture
                if (abilityToUnlock == AbilityType.Texture)
                {
                    visuals.UpdateTexture(true);
                }
            }

            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}