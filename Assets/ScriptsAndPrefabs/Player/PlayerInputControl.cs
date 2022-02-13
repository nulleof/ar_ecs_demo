// GENERATED AUTOMATICALLY FROM 'Assets/ScriptsAndPrefabs/Player/PlayerInputControl.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputControl : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputControl"",
    ""maps"": [
        {
            ""name"": ""PlayerInput"",
            ""id"": ""91426e53-f9e8-46c6-9a65-24de22105f6d"",
            ""actions"": [
                {
                    ""name"": ""SpawnPlayer"",
                    ""type"": ""Button"",
                    ""id"": ""f25ea3bb-b509-49ff-840b-da611b477767"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DespawnPlayer"",
                    ""type"": ""Button"",
                    ""id"": ""e8b4dd5b-1082-4172-a666-66b0f2e03600"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerMovement"",
                    ""type"": ""Value"",
                    ""id"": ""0405152a-3b4d-4f59-b9b0-d27607439729"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerLook"",
                    ""type"": ""Value"",
                    ""id"": ""6f5575b5-37f5-476d-82b1-768a902fdf20"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerEnableLook"",
                    ""type"": ""Button"",
                    ""id"": ""21a20aea-27e3-45ed-999f-8432dcb44803"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""StopFlying"",
                    ""type"": ""Button"",
                    ""id"": ""020cbb82-a271-4d5c-beee-44e6119334c3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""3660f6a3-cf99-42a1-bd10-b2ea94895c4a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8b0b79d4-9f49-4d3d-830d-5d93e271f015"",
                    ""path"": ""<Keyboard>/leftBracket"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PCPlayer"",
                    ""action"": ""SpawnPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bdbfd1c7-aaa8-42fb-93b4-5ce0c5fda4f9"",
                    ""path"": ""<Keyboard>/rightBracket"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PCPlayer"",
                    ""action"": ""DespawnPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""c3cff033-6727-4c94-a566-ce8c1b914ce1"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a64a6bb6-f152-41b6-802c-0f0018a16344"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PCPlayer"",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e5c6b740-c34a-4eb6-8551-646534d04f32"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PCPlayer"",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6eeb7330-12e3-44dc-91fd-a15820b0e783"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PCPlayer"",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a722220c-5a8b-4dfd-a4ca-6564c4039484"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PCPlayer"",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""30b7c681-5ba7-4299-b23a-a06f71f3eac9"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PCPlayer"",
                    ""action"": ""PlayerEnableLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ec9f7f0-7443-410a-aa2e-323d22b4f363"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PCPlayer"",
                    ""action"": ""PlayerLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da1a51e4-3b6b-4028-a568-10fd5693e082"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PCPlayer"",
                    ""action"": ""StopFlying"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab24a359-eb8f-4a8f-8202-44d3a233262c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PCPlayer"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PCPlayer"",
            ""bindingGroup"": ""PCPlayer"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerInput
        m_PlayerInput = asset.FindActionMap("PlayerInput", throwIfNotFound: true);
        m_PlayerInput_SpawnPlayer = m_PlayerInput.FindAction("SpawnPlayer", throwIfNotFound: true);
        m_PlayerInput_DespawnPlayer = m_PlayerInput.FindAction("DespawnPlayer", throwIfNotFound: true);
        m_PlayerInput_PlayerMovement = m_PlayerInput.FindAction("PlayerMovement", throwIfNotFound: true);
        m_PlayerInput_PlayerLook = m_PlayerInput.FindAction("PlayerLook", throwIfNotFound: true);
        m_PlayerInput_PlayerEnableLook = m_PlayerInput.FindAction("PlayerEnableLook", throwIfNotFound: true);
        m_PlayerInput_StopFlying = m_PlayerInput.FindAction("StopFlying", throwIfNotFound: true);
        m_PlayerInput_Fire = m_PlayerInput.FindAction("Fire", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // PlayerInput
    private readonly InputActionMap m_PlayerInput;
    private IPlayerInputActions m_PlayerInputActionsCallbackInterface;
    private readonly InputAction m_PlayerInput_SpawnPlayer;
    private readonly InputAction m_PlayerInput_DespawnPlayer;
    private readonly InputAction m_PlayerInput_PlayerMovement;
    private readonly InputAction m_PlayerInput_PlayerLook;
    private readonly InputAction m_PlayerInput_PlayerEnableLook;
    private readonly InputAction m_PlayerInput_StopFlying;
    private readonly InputAction m_PlayerInput_Fire;
    public struct PlayerInputActions
    {
        private @PlayerInputControl m_Wrapper;
        public PlayerInputActions(@PlayerInputControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @SpawnPlayer => m_Wrapper.m_PlayerInput_SpawnPlayer;
        public InputAction @DespawnPlayer => m_Wrapper.m_PlayerInput_DespawnPlayer;
        public InputAction @PlayerMovement => m_Wrapper.m_PlayerInput_PlayerMovement;
        public InputAction @PlayerLook => m_Wrapper.m_PlayerInput_PlayerLook;
        public InputAction @PlayerEnableLook => m_Wrapper.m_PlayerInput_PlayerEnableLook;
        public InputAction @StopFlying => m_Wrapper.m_PlayerInput_StopFlying;
        public InputAction @Fire => m_Wrapper.m_PlayerInput_Fire;
        public InputActionMap Get() { return m_Wrapper.m_PlayerInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerInputActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerInputActions instance)
        {
            if (m_Wrapper.m_PlayerInputActionsCallbackInterface != null)
            {
                @SpawnPlayer.started -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnSpawnPlayer;
                @SpawnPlayer.performed -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnSpawnPlayer;
                @SpawnPlayer.canceled -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnSpawnPlayer;
                @DespawnPlayer.started -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnDespawnPlayer;
                @DespawnPlayer.performed -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnDespawnPlayer;
                @DespawnPlayer.canceled -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnDespawnPlayer;
                @PlayerMovement.started -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnPlayerMovement;
                @PlayerMovement.performed -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnPlayerMovement;
                @PlayerMovement.canceled -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnPlayerMovement;
                @PlayerLook.started -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnPlayerLook;
                @PlayerLook.performed -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnPlayerLook;
                @PlayerLook.canceled -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnPlayerLook;
                @PlayerEnableLook.started -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnPlayerEnableLook;
                @PlayerEnableLook.performed -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnPlayerEnableLook;
                @PlayerEnableLook.canceled -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnPlayerEnableLook;
                @StopFlying.started -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnStopFlying;
                @StopFlying.performed -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnStopFlying;
                @StopFlying.canceled -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnStopFlying;
                @Fire.started -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnFire;
            }
            m_Wrapper.m_PlayerInputActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SpawnPlayer.started += instance.OnSpawnPlayer;
                @SpawnPlayer.performed += instance.OnSpawnPlayer;
                @SpawnPlayer.canceled += instance.OnSpawnPlayer;
                @DespawnPlayer.started += instance.OnDespawnPlayer;
                @DespawnPlayer.performed += instance.OnDespawnPlayer;
                @DespawnPlayer.canceled += instance.OnDespawnPlayer;
                @PlayerMovement.started += instance.OnPlayerMovement;
                @PlayerMovement.performed += instance.OnPlayerMovement;
                @PlayerMovement.canceled += instance.OnPlayerMovement;
                @PlayerLook.started += instance.OnPlayerLook;
                @PlayerLook.performed += instance.OnPlayerLook;
                @PlayerLook.canceled += instance.OnPlayerLook;
                @PlayerEnableLook.started += instance.OnPlayerEnableLook;
                @PlayerEnableLook.performed += instance.OnPlayerEnableLook;
                @PlayerEnableLook.canceled += instance.OnPlayerEnableLook;
                @StopFlying.started += instance.OnStopFlying;
                @StopFlying.performed += instance.OnStopFlying;
                @StopFlying.canceled += instance.OnStopFlying;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
            }
        }
    }
    public PlayerInputActions @PlayerInput => new PlayerInputActions(this);
    private int m_PCPlayerSchemeIndex = -1;
    public InputControlScheme PCPlayerScheme
    {
        get
        {
            if (m_PCPlayerSchemeIndex == -1) m_PCPlayerSchemeIndex = asset.FindControlSchemeIndex("PCPlayer");
            return asset.controlSchemes[m_PCPlayerSchemeIndex];
        }
    }
    public interface IPlayerInputActions
    {
        void OnSpawnPlayer(InputAction.CallbackContext context);
        void OnDespawnPlayer(InputAction.CallbackContext context);
        void OnPlayerMovement(InputAction.CallbackContext context);
        void OnPlayerLook(InputAction.CallbackContext context);
        void OnPlayerEnableLook(InputAction.CallbackContext context);
        void OnStopFlying(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
    }
}
