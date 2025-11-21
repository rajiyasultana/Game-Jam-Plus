using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStarter : MonoBehaviour
{
    public Button playButton;
    public GameObject LiePannel;
    public TextMeshProUGUI Text;
    public float delayTIme = 1.5f;
    public string sceneToLoad = "PlayerController"; 

    public void OnPlayClicked()
    {
        playButton.interactable = false; // Disable spam clicks
        LiePannel.SetActive(true);
        StartCoroutine(FunnyLoadingSequence());
    }

    IEnumerator FunnyLoadingSequence()
    {
        Text.text = "Loading Fun...";
        yield return new WaitForSeconds(delayTIme);

        Text.text = "0% Fun found.";
        yield return new WaitForSeconds(delayTIme);

        Text.text = "Trying again...";
        yield return new WaitForSeconds(2);

        Text.text = "Still no fun.";
        yield return new WaitForSeconds(2);

        Text.text = "Ok, 1% fun detected.";
        yield return new WaitForSeconds(2);

        Text.text = "Fine, starting the NON-fun version.";
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(sceneToLoad);
    }

}
