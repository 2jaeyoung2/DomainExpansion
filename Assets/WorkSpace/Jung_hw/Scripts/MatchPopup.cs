using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPopup : MonoBehaviour
{
    [SerializeField] Animation anim;
    private void OnEnable()
    {
        anim.Play();
    }
}
