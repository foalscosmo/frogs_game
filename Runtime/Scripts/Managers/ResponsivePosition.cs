using UnityEngine;

namespace Managers
{
    public class ResponsivePosition : MonoBehaviour
    {
        [SerializeField]  private Camera mainCamera;

        void Start()
        {
            Test();
        }

        private void Update()
        {
            Test();
        }

        private void Test()
        {
            transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);
            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero)/ 20;
            Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(mainCamera.rect.width, mainCamera.rect.height)) / 20;
            Vector3 screenSize = topRight - bottomLeft;
            float screenRatio = screenSize.x / screenSize.y;
            float desiredRatio = transform.localScale.x / transform.localScale.y;

            if (screenRatio > desiredRatio)
            {
                float height = screenSize.y;
                transform.localScale = new Vector3(height * desiredRatio, height);
            } else
            {
                float width = screenSize.x;
                transform.localScale = new Vector3(width, width / desiredRatio);
            }
        }
    }
}