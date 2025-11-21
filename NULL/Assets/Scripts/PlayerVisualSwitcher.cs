using UnityEngine;

public class PlayerVisualSwitcher : MonoBehaviour
{
    [Header("Models")]
    public GameObject simpleCube;
    public GameObject characterArt;

    [Header("Texture")]
    public Material texturedMaterial; // Only this is needed

    void Start()
    {
        // Start as Cube
        UpdateVisuals(false);
    }

    public void UpdateVisuals(bool hasUnlockedArt)
    {
        if (hasUnlockedArt)
        {
            simpleCube.SetActive(false);
            characterArt.SetActive(true);
        }
        else
        {
            simpleCube.SetActive(true);
            characterArt.SetActive(false);
        }
    }

    public void UpdateTexture(bool hasUnlockedTexture)
    {
        if (hasUnlockedTexture)
        {
            // Automatically find the Renderer on the characterArt object
            Renderer rend = characterArt.GetComponent<Renderer>();
            
            // If found, apply the texture
            if (rend != null)
            {
                rend.material = texturedMaterial;
            }
            else
            {
                // Fallback: Try looking in children if the art is a group
                Renderer childRend = characterArt.GetComponentInChildren<Renderer>();
                if (childRend != null)
                {
                    childRend.material = texturedMaterial;
                }
            }
        }
    }
}