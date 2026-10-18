using System;
using ComputerInterface.Enumerations;
using ComputerInterface.Models;
using ComputerLib.Types;
using GorillaNetworking;

namespace CICompat.ComputerLib.Views;

internal class ScreenView<TScreen> : ComputerView where TScreen : Screen {
    private readonly Screen _screen = (Screen)Activator.CreateInstance(typeof(TScreen));
    private bool _isScreenActivated;

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

    // TODO: Make Enter, Delete, Option, and Up/Down Arrow Keys work.
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
                if (Enum.TryParse(pressedButton.ToString(), out GorillaKeyboardBindings keyboardBinding)) {
                    _screen.OnInput(keyboardBinding);
                    _screen.UpdateScreen();
                    UpdateViewScreen();
                }
                break;
        }
    }
}