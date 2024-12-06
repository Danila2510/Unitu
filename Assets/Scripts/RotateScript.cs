using UnityEngine;

public class RotateScript : MonoBehaviour
{
    [SerializeField] private float period = 2.0f;
    [SerializeField] private bool x = false;
    [SerializeField] private bool y = true;
    [SerializeField] private bool z = false;
    [SerializeField] private bool local = false;

    void Start()
    {

    }

    void Update()
    {
        float angle = 360.0f * Time.deltaTime / period;
        this.transform.Rotate(
             x ? angle : 0,
             y ? angle : 0,
             z ? angle : 0,
             local ? Space.Self : Space.World
        );
    }
}
