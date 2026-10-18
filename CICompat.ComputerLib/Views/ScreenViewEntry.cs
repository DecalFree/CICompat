using System;
using ComputerInterface.Interfaces;
using ComputerLib.Types;

namespace CICompat.ComputerLib.Views;

internal class ScreenViewEntry : IComputerViewEntry {
    public ScreenViewEntry() {
        EntryComputerView = null;
        EntryName = null;
    }

    public ScreenViewEntry(Type screenType) {
        EntryComputerView = screenType;

        Screen screen = (Screen)Activator.CreateInstance(screenType);
        EntryName = screen.ScreenName;
    }

    public string EntryName { get; }

    public Type EntryComputerView => typeof(ScreenView<>).MakeGenericType(field);
}