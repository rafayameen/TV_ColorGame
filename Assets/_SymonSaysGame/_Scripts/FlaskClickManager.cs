using UnityEngine;
using UnityEngine.UI;

public class FlaskClickManager : MonoBehaviour
{
    public Camera flaskCamera;        // Camera rendering the flasks
    public RawImage flaskRawImage;    // UI displaying the RenderTexture

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 localMousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                flaskRawImage.rectTransform,
                Input.mousePosition,
                null, // UI camera if your canvas is Screen Space - Camera
                out localMousePos);

            // Convert to 0-1 UV coordinates
            Rect rect = flaskRawImage.rectTransform.rect;
            float u = (localMousePos.x - rect.x) / rect.width;
            float v = (localMousePos.y - rect.y) / rect.height;

            // Convert to pixel coordinates of the RenderTexture
            int texX = Mathf.RoundToInt(u * flaskCamera.targetTexture.width);
            int texY = Mathf.RoundToInt(v * flaskCamera.targetTexture.height);

            Ray ray = flaskCamera.ScreenPointToRay(new Vector3(texX, texY, 0));
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                FlaskScript flask = hit.collider.GetComponent<FlaskScript>();
                if (flask != null && flask.interactable)
                {
                    Debug.Log($"Flask clicked: {flask.colorId}");
                    EventManager.OnFlaskClicked?.Invoke(flask);
                    flask.EnableOutline(true);
                }
            }
        }
    }
}
