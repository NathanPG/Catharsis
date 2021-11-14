using UnityEngine;

public class Indicator : MonoBehaviour
{
    private Animator animator = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Deactivate();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        //animator.SetBool("Show", true);
        animator.Play("Show");
    }

    public void Hide()
    {
        //animator.SetBool("Show", false);
        animator.Play("Hide");
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
