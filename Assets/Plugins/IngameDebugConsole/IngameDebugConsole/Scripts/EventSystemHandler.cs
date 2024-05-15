using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Plugins.IngameDebugConsole.Scripts
{
	// Avoid multiple EventSystems in the scene by activating the embedded EventSystem only if one doesn't already exist in the scene
	[DefaultExecutionOrder( 1000 )]
	public class EventSystemHandler : MonoBehaviour
	{
#pragma warning disable 0649
		[SerializeField] GameObject embeddedEventSystem;
#pragma warning restore 0649

		void OnEnable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded -= OnSceneUnloaded;
			SceneManager.sceneUnloaded += OnSceneUnloaded;

			ActivateEventSystemIfNeeded();
		}

		void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			SceneManager.sceneUnloaded -= OnSceneUnloaded;

			DeactivateEventSystem();
		}

		void OnSceneLoaded( Scene scene, LoadSceneMode mode )
		{
			ActivateEventSystemIfNeeded();
		}

		void OnSceneUnloaded( Scene current )
		{
			// Deactivate the embedded EventSystem before changing scenes because the new scene might have its own EventSystem
			DeactivateEventSystem();
		}

		void ActivateEventSystemIfNeeded()
		{
			if( embeddedEventSystem && !EventSystem.current )
				embeddedEventSystem.SetActive( true );
		}

		void DeactivateEventSystem()
		{
			if( embeddedEventSystem )
				embeddedEventSystem.SetActive( false );
		}
	}
}