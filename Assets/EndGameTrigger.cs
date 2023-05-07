using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTrigger : MonoBehaviour
{
	[SerializeField]
	private string mainMenuScene;

	private void OnTriggerEnter(Collider other)
	{
		SceneManager.LoadScene(mainMenuScene);
	}
}