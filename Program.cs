using Editor.Editor;
using GUI.Editor;
using System.Threading;

Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

FormEditor editor = new();
editor.GameEditor = new GameEditor(editor);
editor.Show();
editor.GameEditor.Run();