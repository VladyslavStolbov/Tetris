using UnityEngine;

public class StartMenu : MonoBehaviour
{
	[SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject buttons;

    public void ToggleOptionsMenu()
    {
        buttons.SetActive(!buttons.activeSelf);
        optionsMenu.SetActive(!optionsMenu.activeSelf);   
    }
}
