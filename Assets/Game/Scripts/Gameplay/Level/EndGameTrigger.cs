using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTrigger : MonoBehaviour
{
	[SerializeField]
	private string mainMenuScene;

	private void OnTriggerEnter2D(Collider2D other)
	{
		SceneManager.LoadScene(mainMenuScene);
	}
}