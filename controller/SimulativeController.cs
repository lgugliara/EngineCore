using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.InputSystem;
using ZoloSim.EngineCore.Enums;

namespace ZoloSim.EngineCore
{
    public class SimulativeController : Controller
    {
        #region Actions
        public void OnClutch(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
                _clutchEngagement = 1f - ctx.ReadValue<float>();
            else if (ctx.phase == InputActionPhase.Canceled)
                _clutchEngagement = 1f;
        }
        public void OnShiftUp(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
                car.transmission.ShiftUp();
        }
        public void OnShiftDown(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
                car.transmission.ShiftDown();
        }
        #endregion
    }
}