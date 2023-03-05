using ICKT;
using ICKT.UI;
using UnityEngine;

public class UIMainMenu : UIBase
{
    public void OnPlayButtonClicked()
    {
        GameInstance.StartLevel();
        FMODUnity.RuntimeManager.PlayOneShot(Const.FMOD_UI_CLICK_EVENT);
    }
}
