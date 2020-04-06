using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && SceneManager.sceneCount > 1)
            SceneManager.UnloadSceneAsync("Scenes/Options");
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (SceneManager.sceneCount < 2)
                SceneManager.LoadScene("Scenes/Options", LoadSceneMode.Additive);
        }

    }
}