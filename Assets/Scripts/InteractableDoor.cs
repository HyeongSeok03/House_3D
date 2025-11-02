using UnityEngine;

public interface IInteractable
{
    void Interact(Player player);
}

public class InteractableDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform indoor;
    [SerializeField] private Transform outdoor;
    public void Interact(Player player)
    {
        if (transform.position.z < player.transform.position.z)
        {
            player.transform.SetPositionAndRotation(outdoor.position, outdoor.rotation);
            return;
        }

        player.transform.SetPositionAndRotation(indoor.position, indoor.rotation);
    }
}
