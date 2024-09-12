using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public List<GameObject> buttons = new(); // List to store buttons

    // Method to add a button to the list
    public void AddButton(GameObject button)
    {
        if (button != null)
        {
            buttons.Add(button);
            Debug.Log("Button added to ButtonManager list.");
        }
    }
}