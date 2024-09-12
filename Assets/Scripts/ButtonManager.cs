using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public List<GameObject> buttonGameObjects = new(); // List to store GameObjects

    // Method to add a button GameObject to the list
    public void AddButton(GameObject buttonGameObject)
    {
        if (buttonGameObject != null && !buttonGameObjects.Contains(buttonGameObject))
        {
            buttonGameObjects.Add(buttonGameObject);
            Debug.Log("Button GameObject added to ButtonManager list.");
        }
    }

    // Method to delete all buttons
    public void DeleteAllButtons()
    {
        foreach (var buttonGameObject in buttonGameObjects)
            if (buttonGameObject != null)
                Destroy(buttonGameObject); // Destroy the button GameObject
        buttonGameObjects.Clear(); // Clear the list
        Debug.Log("All buttons have been deleted.");
    }

    // Method to lock or unlock all buttons
    public void LockAllButtons(bool lockState)
    {
        foreach (var buttonGameObject in buttonGameObjects)
            if (buttonGameObject != null)
            {
                var buttonBehavior = buttonGameObject.GetComponent<ButtonBehavior>();
                if (buttonBehavior != null) buttonBehavior.Lock(lockState); // Call the Lock method on each button
            }

        Debug.Log($"All buttons have been {(lockState ? "locked" : "unlocked")}.");
    }
}