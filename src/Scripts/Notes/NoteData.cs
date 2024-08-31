using System.Collections.Generic;

namespace Plutono.Core.Note;

//class NoteDetail -- Store the data of a note in game.
[System.Serializable]
public class NoteData
{
    public uint id;         //the id of this note (start from 1)
    public float pos;       //the position of this note (from -2 to 2, or it won't be shown). float, due to Vector3 accept float only.
    public double size;      //the size of this note (from 0 to 4)
    
    public NoteData(uint id, float pos, double size)
    {
	    this.id = id;
	    this.pos = pos;
	    this.size = size;
    }
}

public enum NoteType
{
    Piano, Slide, Blank, Vibrate, Swipe
}

[System.Serializable]
public sealed class GamePianoSound
{
    public double delay; //w
    public double duration; //d
    public short pitch; //p
    public short volume; //v
}

public class BlankNoteData : NoteData
{
	public double time;      //the time when this note should be touched (start from 0)

	public BlankNoteData(uint id, float pos, double size, double time) : base(id, pos, size)
	{
		this.time = time;
	}

    public bool IsShown => pos is <= 2.0f and >= -2.0f;     //TRUE if this note should be shown
}

public sealed class SlideNoteData : BlankNoteData
{
    public bool isLink;
    public int prevLink = -1;
    public int nextLink = -1;

    public List<GamePianoSound> sounds;
    public bool IsShown => pos is <= 2.0f and >= -2.0f;     //TRUE if this note should be shown

    public SlideNoteData(uint id, float pos, double size, double time) : base(id, pos, size, time)
    {
    }
}

public sealed class PianoNoteData : BlankNoteData
{
    public List<GamePianoSound> sounds;
    public bool IsShown => pos is <= 2.0f and >= -2.0f;     //TRUE if this note should be shown

    public PianoNoteData(uint id, float pos, double size, double time) : base(id, pos, size, time)
    {
    }
}

public sealed class HoldNoteData : NoteData
{
    public double duration; //the duration of the hold
    public bool IsShown => pos is <= 2.0f and >= -2.0f;     //TRUE if this note should be shown
    public double BeginTime;
    public double EndTime;

    public HoldNoteData(uint id, float pos, double size, double beginTime, double endTime) : base(id, pos, size)
	{
	    BeginTime = beginTime;
        EndTime = endTime;
	}
}