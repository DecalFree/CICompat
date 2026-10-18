#if DEBUG

using CICompat.ComputerLib.Tools;
using ComputerLib.Types;
using GorillaNetworking;

namespace CICompat.ComputerLib;

internal class TestScreen : Screen {
    public override string ScreenName => "ComputerLib Test";

    private string _lastPressedButton;

    public override void OnView(bool activated) {
        Logging.Info($"Test Screen: {activated}");
    }

    public override void UpdateScreen() {
        Text.AppendLine("Hello from, ComputerLib!");
        Text.AppendLine($"Last Pressed Button: {_lastPressedButton}");
    }

    public override void OnInput(GorillaKeyboardBindings button) {
        _lastPressedButton = button.ToString();
    }
}

#endif