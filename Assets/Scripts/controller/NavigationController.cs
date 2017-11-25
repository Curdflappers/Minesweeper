using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Attach this to buttons that navigate between scenes
/// </summary>
public class NavigationController : MonoBehaviour {

    /// <summary>
    /// Loads the scene named <paramref name="name"/>
    /// </summary>
    /// <param name="name"></param>
	public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
