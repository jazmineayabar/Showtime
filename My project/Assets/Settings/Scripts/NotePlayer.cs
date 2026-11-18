using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class NotePlayer : MonoBehaviour
{
    MarchingNote currentNote;
    public DrillManager drillManager;
    public MusicNoteScorer musicScorer;

    void OnTriggerEnter2D(Collider2D other)
    {
        currentNote = other.GetComponent<MarchingNote>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<MarchingNote>() == currentNote)
        {
            currentNote = null;
        }
    }

    void Update()
    {
       if (Keyboard.current != null &&
        Keyboard.current.spaceKey.wasPressedThisFrame &&
        currentNote != null)
    {
        musicScorer.ScoreNote(currentNote, transform);

        currentNote.PlayNote();
        currentNote = null;
    }
    }
}