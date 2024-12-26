using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePlacement : MonoBehaviour
{
    private SlimeCharacterManage currentSlime;

    public void SetSlime(SlimeCharacterManage slime)
    {
        if (currentSlime != null)
        {
            Debug.LogWarning("This placement already has a slime.");
            return;
        }

        slime.transform.position = this.transform.position;
        currentSlime = slime;
        currentSlime.SetSlimeAnimationSate(SlimeAnimationState.Jump);
    }

    public void RemoveSlime()
    {
        if (currentSlime != null)
        {
            currentSlime = null;
        }
    }
}
