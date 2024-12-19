using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePlacement : MonoBehaviour
{
    public void SetSlime(SlimeCharacterManage slime)
    {
        slime.transform.position = this.transform.position;
    }
}
