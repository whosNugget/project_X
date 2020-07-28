using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
	[Tooltip("Defines what menu canvases which will be controlled my this manager. The first element will be shown on startup")]
	[SerializeField] List<Canvas> controllableMenus = new List<Canvas>();
	
	Canvas shownMenu = null;

	private void Start()
	{
		foreach (Canvas c in controllableMenus) c.gameObject.SetActive(false);
		shownMenu = controllableMenus[0];
		shownMenu.gameObject.SetActive(true);
	}

	public void ShowNewMenu(int index)
	{
		for (int i = 0; i < controllableMenus.Count; i++)
		{
			if (i == index) controllableMenus[i].gameObject.SetActive(true);
			else controllableMenus[i].gameObject.SetActive(false);
		}
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
