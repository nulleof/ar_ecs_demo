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
    public struct PlayerInputActions
    {
        private @PlayerInputControl m_Wrapper;
        public PlayerInputActions(@PlayerInputControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @SpawnPlayer => m_Wrapper.m_PlayerInput_SpawnPlayer;
        public InputAction @DespawnPlayer => m_Wrapper.m_PlayerInput_DespawnPlayer;
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
    }
}
