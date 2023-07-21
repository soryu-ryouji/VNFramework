using UnityEngine;

namespace VNFramework
{
    public class InputController : MonoBehaviour, IController
    {
        private KeyCode _nextPerformanceKeyCode = KeyCode.Return;
        private KeyCode _toggleDialoguePanelKeycode = KeyCode.Space;
        
        private void Update()
        {
            if (Input.GetKeyDown(_nextPerformanceKeyCode)) this.SendCommand<NextPerformanceCommand>();
            if (Input.GetKeyDown(_toggleDialoguePanelKeycode)) this.SendCommand<ToggleDialoguePanelCommand>();
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
