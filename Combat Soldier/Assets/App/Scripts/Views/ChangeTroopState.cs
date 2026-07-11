using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChangeTroopState : MonoBehaviour
{
    public void SetupChangeStateButton(PlayerStateController stateController)
    {
        GetComponent<Button>().onClick.AddListener(() => stateController.SwitchToOppositeState());
    }
}
