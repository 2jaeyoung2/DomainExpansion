using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] Animation uiAnim;
    [SerializeField] MapMaker mapMaker;
    public List<string> anims;

    private void Start()
    {
        anims = new List<string>();
        uiAnim.wrapMode = WrapMode.Once;
        foreach(AnimationState state in uiAnim)
        {
            anims.Add(state.name);
        }
    }
    public void SetEditMap(bool expand)
    {
        if(expand == true)
        {
            uiAnim.Play(anims[0]);
        }
        else
        {
            uiAnim.Play(anims[1]);
        }
        mapMaker.placeMode = expand;
    }
}
