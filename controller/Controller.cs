using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.InputSystem;
using ZoloSim.EngineCore.Enums;
using System.Collections;

namespace ZoloSim.EngineCore
{
    public abstract class Controller : MonoBehaviour
    {
        public PlayerCamera playerCamera;

        public Vehicle car => GetComponentInParent<Vehicle>();

        #region Runtime variables
        internal float _throttle = 0;
        internal float _shiftThrottle = 0;
        internal float _steer = 0;
        internal float _brake = 0;
        internal float _handbrake = 0;
        internal float _clutchEngagement = 1f;
        internal bool _isClutchAutoEngaged => car._brake > 0.5f;
        internal bool _isResetting = false;
        #endregion

        #region Damping
        [HideInInspector] private float _throttleDamping = 0.04f;
        [HideInInspector] private float _steerDamping = 0.12f;
        [HideInInspector] private float _brakeDamping = 0.04f;
        [HideInInspector] private float _handbrakeDamping = 0.08f;
        [HideInInspector] private float _clutchDamping = 0.04f;

        [HideInInspector] internal virtual float ThrottleDamping { get => _throttleDamping; set => _throttleDamping = value; }
        [HideInInspector] internal virtual float SteerDamping { get => _steerDamping; set => _steerDamping = value; }
        [HideInInspector] internal virtual float BrakeDamping { get => _brakeDamping; set => _brakeDamping = value; }
        [HideInInspector] internal virtual float HandbrakeDamping { get => _handbrakeDamping; set => _handbrakeDamping = value; }
        [HideInInspector] internal virtual float ClutchDamping { get => _clutchDamping; set => _clutchDamping = value; }
        #endregion

        public void Start()
        {
            car.engine.ignition.IsIgnited = false;
        }

        public void Ciao() => Update();
        void Update()
        {
            float throttleVel = 0, steerVel = 0, brakeVel = 0, handbrakeVel = 0, clutchVel = 0;

            car._throttle = car.transmission.IsShifting ?
                Mathf.SmoothDamp(car._throttle, _shiftThrottle, ref throttleVel, ThrottleDamping) :
                Mathf.SmoothDamp(car._throttle, _throttle, ref throttleVel, ThrottleDamping);
            car._steer = Mathf.SmoothDamp(car._steer, _steer, ref steerVel, SteerDamping);
            car._brake = Mathf.SmoothDamp(car._brake, _brake, ref brakeVel, BrakeDamping);
            car._handbrake = Mathf.SmoothDamp(car._handbrake, _handbrake, ref handbrakeVel, HandbrakeDamping);
            car._clutchEngagement = _isClutchAutoEngaged ?
                Mathf.SmoothDamp(car._clutchEngagement, 0, ref clutchVel, ClutchDamping) :
                Mathf.SmoothDamp(car._clutchEngagement, _clutchEngagement, ref clutchVel, ClutchDamping);

            // Apply automatic transmission
            if (car.transmission.transmissionType == TransmissionType.Automatic)
            {
                if (car.transmission.CurrentTransmissionAlert == TransmissionAlert.ShiftUpRequired)
                    StartCoroutine(ShiftUpCoroutine());
                else if (car.transmission.CurrentTransmissionAlert == TransmissionAlert.ShiftDownRequired)
                    StartCoroutine(ShiftDownCoroutine());
                else if (car.transmission.CurrentTransmissionAlert == TransmissionAlert.NeutralRequired)
                    StartCoroutine(ShiftNeutralCoroutine());
            }
        }

        #region Actions
        public void OnThrottle(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
                _throttle = ctx.ReadValue<float>();
            else if (ctx.phase == InputActionPhase.Canceled)
                _throttle = 0;
        }
        public void OnBrake(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
                _brake = ctx.ReadValue<float>();
            else if (ctx.phase == InputActionPhase.Canceled)
                _brake = 0;
        }
        public void OnIgnition(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
                car.engine.ignition.Ignite();
        }
        public void OnSteer(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
                _steer = ctx.ReadValue<Vector2>().x;
            else if (ctx.phase == InputActionPhase.Canceled)
                _steer = 0;
        }
        public void OnHandbrake(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
                _handbrake = ctx.ReadValue<float>();
            else if (ctx.phase == InputActionPhase.Canceled)
                _handbrake = 0;
        }
        public void OnSwitchView(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
                playerCamera.Next();
        }
        public void OnShiftNeutral(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
                car.transmission.ShiftNeutral();
        }
        public void OnReset(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
                StartCoroutine(ResetCoroutine());
        }
        #endregion

        #region Methods
        public void Reset(bool force = false)
        {
            _isResetting = force ? false : _isResetting;
            StartCoroutine(ResetCoroutine());
        }
        IEnumerator ResetCoroutine()
        {
            if (_isResetting)
                yield break;

            // Resetta la posizione in base all'altezza del terreno
            _isResetting = true;
            _throttle = 0;
            var oldPosition = car.transform.position;
            var newPosition = new Vector3(oldPosition.x, 0.5f, oldPosition.z);
            var newRotation = Quaternion.Euler(0, car.transform.eulerAngles.y, 0);
            int layerMask = ~LayerMask.GetMask("Player");
            var ray = new Ray(car.transform.position + Vector3.up * 10, Vector3.down * 20);
            if (Physics.Raycast(ray, out var hit, 20, layerMask))
                newPosition.y = hit.point.y + 0.2f;
            else
                newPosition = new Vector3(0, 0.2f, 0);

            // Resetta il transform
            car.transform.position = newPosition;
            car.transform.localRotation = newRotation;

            // Azzera velocità lineare e angolare
            car.rb.linearVelocity = Vector3.zero;
            car.rb.angularVelocity = Vector3.zero;

            // Resetta il tensore d'inerzia e il centro di massa
            car.rb.ResetInertiaTensor();
            car.rb.ResetCenterOfMass();

            // Resetta il motore
            car.engine.Reset();

            yield return new WaitForSeconds(1);
            _isResetting = false;
        }
        #endregion

        #region Automatic transmission
        IEnumerator ShiftUpCoroutine()
        {
            var delta = 1f / car.transmission.clutchResolution;
            car.transmission.IsShifting = true;
            _clutchEngagement = 0;
            _shiftThrottle = 0.2f;

            if (_isClutchAutoEngaged)
                yield return new WaitForSeconds(car.transmission.shiftDelay * 0.5f);
            else
                while (car._clutchEngagement > 0.3f)
                    yield return new WaitForSeconds(car.transmission.shiftDelay * delta * 0.5f);

            car.transmission.ShiftUp();
            if (_isClutchAutoEngaged)
                yield return new WaitForSeconds(car.transmission.shiftDelay * 0.5f);
            else
            {
                while (car._clutchEngagement < 0.66f)
                {
                    _clutchEngagement = Mathf.SmoothDamp(_clutchEngagement, 1, ref _clutchDamping, ClutchDamping + delta);
                    _shiftThrottle = Mathf.SmoothDamp(_shiftThrottle, _throttle, ref _throttleDamping, ThrottleDamping);
                    yield return new WaitForSeconds(car.transmission.shiftDelay * delta * 0.5f);
                }
            }

            _clutchEngagement = 1;
            car.transmission.IsShifting = false;
        }
        IEnumerator ShiftDownCoroutine()
        {
            var delta = 1f / car.transmission.clutchResolution;
            car.transmission.IsShifting = true;
            _clutchEngagement = 0;
            _shiftThrottle = 0;

            if (_isClutchAutoEngaged)
                yield return new WaitForSeconds(car.transmission.shiftDelay * 0.5f);
            else
                while (car._clutchEngagement > 0.3f)
                    yield return new WaitForSeconds(car.transmission.shiftDelay * delta * 0.5f);

            car.transmission.ShiftDown();
            if (_isClutchAutoEngaged)
                yield return new WaitForSeconds(car.transmission.shiftDelay * 0.5f);
            else
            {
                while (car._clutchEngagement < 0.6f)
                {
                    var releaseAmount = Mathf.Clamp01(car._clutchEngagement - 0.3f);
                    _clutchEngagement = Mathf.SmoothDamp(_clutchEngagement, 1, ref _clutchDamping, ClutchDamping + delta);
                    yield return new WaitForSeconds(car.transmission.shiftDelay * delta * 0.5f);
                }
            }

            _clutchEngagement = 1;
            car.transmission.IsShifting = false;
        }
        IEnumerator ShiftNeutralCoroutine()
        {
            var delta = 1f / car.transmission.clutchResolution;
            car.transmission.IsShifting = true;
            _clutchEngagement = 0;
            _shiftThrottle = 0;

            if (_isClutchAutoEngaged)
                yield return new WaitForSeconds(car.transmission.shiftDelay * 0.5f);
            else
                while (car._clutchEngagement > 0.3f)
                    yield return new WaitForSeconds(car.transmission.shiftDelay * delta * 0.5f);

            car.transmission.ShiftNeutral();
            _clutchEngagement = 1;
            car.transmission.IsShifting = false;
        }
        #endregion
    }
}