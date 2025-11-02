# House 3D

## Introduction  

The house I created is a small and cozy studio room with solid brick walls and a stone roof.  
To achieve realistic surface textures, I used a **free brick asset** from the Unity Asset Store.  
All the furniture and interior elements were modeled by myself using Unity’s **basic 3D shapes** (Cube, Cylinder, Plane, etc.).  
Each object has a custom-made material that I designed to maintain consistent tone and texture across the entire environment.  

Inside the room, there are a **bed, TV, lighting, light switch, and a door**, and among these, the **door, TV, and light switch** are all **interactive**.  
The player can **move with WASD**, **rotate the camera by dragging the mouse**, and **jump with the spacebar**.  
All interactive objects are implemented using the **Unity Input System**.  
The project focuses on **natural lighting, shadow effects, object placement ratio, and smooth player interaction** for realism.

**Asset Used:**  
[Free Brick Textures (Unity Asset Store)](https://assetstore.unity.com/packages/2d/textures-materials/floors/textures-free-122421)

---

## Functions  

The project consists of **three main scripts**:  
`Player`, `InteractableDoor`, `InteractableTV`, and `InteractableLight`.  

The basic player movement system is implemented using the **Unity Input System** (so movement-related functions are omitted below).  
For interactions, all interactable objects inherit an interface called `IInteractable`.  
The player uses a **Raycast** to detect objects within the `Interactable` layer,  
and when such an object is detected, the player accesses its `IInteractable` component and calls the `Interact()` method.

---

### 🔹 Player – Interaction Update (FixedUpdate)

```csharp
private void HandleInteract()
{
    Ray ray = new Ray(playerCamera.position, playerCamera.forward);
    if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
    {
        interactable = hit.collider.GetComponent<IInteractable>();
        interactableUI.SetActive(true);
        return;
    }
    interactable = null;
    interactableUI.SetActive(false);
}
```

### 🔹 Player – When Pressing the Interact Key (E)

```csharp
public void OnInteract(InputValue value)
{
    if (!value.isPressed) return;

    Debug.Log("Try interact.");
    if (interactable != null)
    {
        interactable.Interact(this);
        return;
    }
    Debug.Log("can not interact.");
}
```
### 🔹 InteractableDoor Class
```
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
```
### Interaction Summary

- Player → Raycast → IInteractable Object

- If hit: Interact() is called through the interface.

- Works for all interactive objects (door, TV, and light switch).

- Designed for smooth, modular expansion when adding new interactable items later.
