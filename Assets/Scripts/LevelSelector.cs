using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{



    public void LoadCavesLevel()
    {
        SceneManager.LoadScene("Day Caves");  
    }

    public void LoadWinterLevel()
    {
        SceneManager.LoadScene("Day Winter");
    }

    public void LoadSunnyLevel()
    {
        SceneManager.LoadScene("Day Sunny");
    }

}
