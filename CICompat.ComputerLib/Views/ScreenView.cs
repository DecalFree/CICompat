using System;
using System.Collections.Generic;
using ComputerInterface.Enumerations;
using ComputerInterface.Models;
using ComputerLib.Types;
using GorillaNetworking;

namespace CICompat.ComputerLib.Views;

internal class ScreenView<TScreen> : ComputerView where TScreen : Screen {
    private readonly Screen _screen = (Screen)Activator.CreateInstance(typeof(TScreen));
    private bool _isScreenActivated;

    private readonly Dictionary<EKeyboardButton, GorillaKeyboardBindings> _keyboardButtonMappings = new() {
        [EKeyboardButton.NUM0] = GorillaKeyboardBindings.zero,
        [EKeyboardButton.NUM1] = GorillaKeyboardBindings.one,
        [EKeyboardButton.NUM2] = GorillaKeyboardBindings.two,
        [EKeyboardButton.NUM3] = GorillaKeyboardBindings.three,
        [EKeyboardButton.NUM4] = GorillaKeyboardBindings.four,
        [EKeyboardButton.NUM5] = GorillaKeyboardBindings.five,
        [EKeyboardButton.NUM6] = GorillaKeyboardBindings.six,
        [EKeyboardButton.NUM7] = GorillaKeyboardBindings.seven,
        [EKeyboardButton.NUM8] = GorillaKeyboardBindings.eight,
        [EKeyboardButton.NUM9] = GorillaKeyboardBindings.nine,

        [EKeyboardButton.Delete] = GorillaKeyboardBindings.delete,
        [EKeyboardButton.Enter] = GorillaKeyboardBindings.enter,
        [EKeyboardButton.Option1] = GorillaKeyboardBindings.option1,
        [EKeyboardButton.Option2] = GorillaKeyboardBindings.option2,
        [EKeyboardButton.Option3] = GorillaKeyboardBindings.option3
    };

    public override void OnViewShown(object[] arguments) {
        if (!_isScreenActivated) {
            _isScreenActivated = true;
            _screen.OnView(true);
        }

        _screen.UpdateScreen();
    }

    protected override string GetViewText() {
        _screen.Text.Clear();
        _screen.UpdateScreen();

        return _screen.Text.ToString();
    }

    public override void OnButtonPressed(EKeyboardButton pressedButton) {
        switch (pressedButton) {
            case EKeyboardButton.Back:
                if (_isScreenActivated) {
                    _screen.OnView(false);
                    _isScreenActivated = false;
                }

                ReturnToMainMenu();
                break;
            default:
                if (TryGetKeyboardButton(pressedButton, out GorillaKeyboardBindings keyboardBinding)) {
                    _screen.OnInput(keyboardBinding);
                    _screen.UpdateScreen();
                    UpdateViewScreen();
                }
                break;
        }
    }

    private bool TryGetKeyboardButton(EKeyboardButton keyboardButton, out GorillaKeyboardBindings keyboardBinding) {
        if (_keyboardButtonMappings.TryGetValue(keyboardButton, out keyboardBinding))
            return true;

        return Enum.TryParse(keyboardButton.ToString(), out keyboardBinding) && Enum.IsDefined(typeof(GorillaKeyboardBindings), keyboardBinding);
    }
}