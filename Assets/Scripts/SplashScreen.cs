using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace splashScreen
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] private Image myImage;

        void Start()
        {
            myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, 0f);
            StartCoroutine(FadeEffect());

            #if UNITY_EDITOR
                        PlayerPrefs.DeleteAll();
            #endif

            if (PlayerPrefs.GetInt("FirstTimeOpenGame") == 0)
            {
                PlayerPrefs.SetInt("LimitFPS", 60);
                PlayerPrefs.SetInt("FirstTimeOpenGame", 1);
            }

            Application.targetFrameRate = PlayerPrefs.GetInt("LimitFPS");

        }

        private IEnumerator FadeEffect()
        {
            float fadeCount = 0;

            while (fadeCount < 1.0f)
            {
                fadeCount += 0.01f;
                yield return new WaitForSeconds(0.01f);
                myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, fadeCount);
            }

            while (fadeCount > 0.01f)
            {
                fadeCount -= 0.01f;
                yield return new WaitForSeconds(-0.01f);
                myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, fadeCount);
            }

            //Go to Next Scene
            SceneManager.LoadScene("Game");
        }
    }
}
