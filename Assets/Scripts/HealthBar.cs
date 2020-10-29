using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform bar;
    
    public void SetSize(float size)
    {
        bar.localScale = new Vector3(size, 1f);
    }
    void Start()
    {
        bar = transform.Find("Bar");
    }

}
