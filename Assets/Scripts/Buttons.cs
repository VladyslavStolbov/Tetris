using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
	public void StartButton()
	{
		SceneManager.LoadScene("Game");
	}

	public void PauseButton()
	{
		
	}

	public void ExitButton()
	{
		SceneManager.LoadScene("StartMenu");
	}

	public void QuitButton()
	{
		Application.Quit();
	}
}
