using UnityEngine;

public class InteractableTV : MonoBehaviour, IInteractable
{
    public GameObject tvPlane;
    public void Interact(Player player)
    {
        tvPlane.SetActive(!tvPlane.activeSelf);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
