using Plutono.Core.Note;
using Plutono.Scripts.Notes;
using Plutono.Util;

namespace Plutono.Scripts.Game
{
    public struct NoteClearEvent<TNote> : IEvent
        where TNote : Note
    {
        public TNote Note;
        public NoteGrade Grade;
        public float DeltaXPos;
    }

    public struct NoteMissEvent<TNote> : IEvent
        where TNote : Note
    {
        public TNote Note;
    }

    public struct GamePrepareEvent : IEvent { }
    public struct GameStartEvent : IEvent { }
    public struct GamePauseEvent : IEvent { }
    public struct GameResumeEvent : IEvent { }
    public struct GameFailEvent : IEvent { }
    public struct GameClearEvent : IEvent { }
}