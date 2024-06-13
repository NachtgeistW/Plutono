namespace Plutono.Core.Note.Render;

public interface INoteRenderer
{
    public bool DisplayNoteId { get; protected set; }

    public void Render();
    public void OnDispose();
}

public interface IRendererHoldable : INoteRenderer
{
    public void OnNoteLoaded();
    public void UpdateComponentStates();
    public void UpdateComponentOpacity();
    public void UpdateTransformScale();
}

public interface IRendererTouchable : INoteRenderer
{
    public void OnTouch(NoteGrade grade);
    public void OnClear(NoteGrade grade);
}