using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    public GameObject objectPrefab; // Drag your object prefab here in the Inspector
    private Canvas _canvas;

    private void Start()
    {
        _canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 clickPosition = eventData.position;

        // Convert screen position to Canvas position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform, 
            clickPosition, 
            _canvas.worldCamera, 
            out Vector2 localPoint
        );

        // Instantiate the object
        var newObject = Instantiate(objectPrefab, _canvas.transform);
        newObject.transform.localPosition = localPoint;
    }
}