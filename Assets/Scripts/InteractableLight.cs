using UnityEngine;

public class InteractableLight : MonoBehaviour, IInteractable
{
    public GameObject[] lights;
    public GameObject[] switchs;

    public void Interact(Player player)
    {
        if (lights[0].activeSelf)
        {
            lights[0].SetActive(false);
            lights[1].SetActive(false);
            switchs[0].SetActive(false);
            switchs[1].SetActive(true);
        }
        else
        {
            lights[0].SetActive(true);
            lights[1].SetActive(true);
            switchs[0].SetActive(true);
            switchs[1].SetActive(false);
        }
    }
}
