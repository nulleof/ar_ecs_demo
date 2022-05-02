using System;
using Managers.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers {

	public class InputManager : BaseManager<InputManager> {

		private PlayerInputControl inputControl;
		
		public void Awake() {

			this.inputControl = new PlayerInputControl();

		}

		private void OnEnable() {
			
			this.inputControl.Enable();

			this.inputControl.PlayerInput.PlayerMovement.performed += Move;

		}

		private void OnDisable() {
			
			this.inputControl.Disable();
			
		}

		private void Move(InputAction.CallbackContext context) {

			var direction = context.ReadValue<Vector2>();
			
			Logger.Log($"move called! {direction}");

		}

	}

}
