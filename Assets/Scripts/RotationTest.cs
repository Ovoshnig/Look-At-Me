using UnityEngine;

public class RotationTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Transform childFigureTransform = transform.GetChild(0);

            childFigureTransform.Rotate(new Vector3(180, 0, 0));
            Renderer figureRenderer = transform.gameObject.GetComponentInChildren<Renderer>();
            Bounds currentBounds = figureRenderer.bounds;

            childFigureTransform.localPosition += new Vector3(0, currentBounds.extents.y, 0);
        }
    }
}
