//Script per bottoni che devono portare ad una nuova scena
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    /// <summary>
    /// Permette di caricare una scena tramite nome
    /// </summary>
    /// <param name="staticSceneName"></param>
    public static void StaticGoToScene(string staticSceneName)
    {

        SceneManager.LoadScene(staticSceneName);
        Time.timeScale = 1;
        //Debug.Log("Caricata scena di nome " + staticSceneName);
    }

    /// <summary>
    /// Permette di caricare una scena tramite buildIndex
    /// </summary>
    /// <param name="staticSceneIndex"></param>
    public static void StaticGoToScene(int staticSceneIndex)
    {

        SceneManager.LoadScene(staticSceneIndex);
        Time.timeScale = 1;
        //Debug.Log("Caricata scena ad indice " + staticSceneIndex);
    }
    /// <summary>
    /// Permette di caricare una scena tramite nome
    /// </summary>
    /// <param name="sceneName"></param>
    public void GoToScene(string sceneName) { /*SceneManager.LoadScene(levelName);*/StaticGoToScene(sceneName); }
    /// <summary>
    /// Permette di caricare una scena tramite buildIndex
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void GoToScene(int sceneIndex) { /*SceneManager.LoadScene(levelIndex);*/StaticGoToScene(sceneIndex); }
    /// <summary>
    /// Permette di caricare la stessa scena in cui si è
    /// </summary>
    public void ReloadScene() { /*Debug.Log("Carico scena del gameobject:" + gameObject + " -> " + gameObject.scene.name);*/ StaticGoToScene(gameObject.scene.name); }

    public void QuitGame()
    {
        Application.Quit();
        //Debug.Log("Esce dal gioco");
    }

}
