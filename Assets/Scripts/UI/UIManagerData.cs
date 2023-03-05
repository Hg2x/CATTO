using UnityEngine;

namespace ICKT.UI
{
    [CreateAssetMenu(fileName = "UIManagerData", menuName = "ScriptableObject/UIManagerData", order = 0)]
    public class UIManagerData : ScriptableObject
    {
        public UIBase StartingUI;
        public UIBase[] UICollection;
    }
}
