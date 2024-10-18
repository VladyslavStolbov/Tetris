using UnityEngine;

public class StartMenu : MonoBehaviour
{
	[SerializeField] private OptionsMenu optionsMenu;
    [SerializeField] private Buttons buttons;

    public void ToggleOptionsMenu()
    {
        buttons.gameObject.SetActive(!buttons.gameObject.activeSelf);
        optionsMenu.gameObject.SetActive(!optionsMenu.gameObject.activeSelf);   
    }
}
