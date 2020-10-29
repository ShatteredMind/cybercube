using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    public GameObject cakePrefab;
    public float width;
    public float height;
    public float dist = 40.0f;

    // Start is called before the first frame update
    void Start()
    {
/*        float aspectRatio = width / height;
        Camera.main.projectionMatrix = Matrix4x4.Perspective(60f, aspectRatio, 0.5f, 100f);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject() && 
                GameManager.manager.cakes[0] != null && 
                !GameManager.manager.cakes[0].GetComponent<Cake>().isThrown && 
                !GameManager.manager.GameIsFinished)
            {
                Vector3 path = Vector3.zero;
                var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
                Ray ray = Camera.main.ScreenPointToRay(position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    path = hit.point - GameManager.manager.cakes[0].transform.position;
                }
                GameManager.manager.cakes[0].GetComponent<Cake>().ThrowCake(path);
            }
        }
    }
}
