using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private string level;
    [SerializeField] private Button returnButton;
    [SerializeField] private TextMeshProUGUI gameOverText;
    Image image;

    private bool teleporting;
    private bool canFade = false;


    private void Start()
    {
        teleporting = false;
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, 0);
        canFade = true;
        gameObject.SetActive(false);
    }

    public void ReturnToVillage()
    {
        if(teleporting == false)
        {
            StartCoroutine(Teleport());
            teleporting = true;
        }

    }

    private IEnumerator Teleport()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(level);
    }

    private IEnumerator FadeToBlack()
    {
        while (image.color.a < 1)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.01f);
            gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, gameOverText.color.a + 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
        returnButton.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (canFade)
        {
            StartCoroutine(FadeToBlack());
            canFade = false;
        }
    }

}
