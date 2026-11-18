
using UnityEngine;

public class NoteGenerator : MonoBehaviour
{
    public GameObject notePrefab;
    public int numberOfNotes = 8;

   public void GenerateNotes(Transform player, Transform target)
{
    foreach (Transform child in transform)
    {
        Destroy(child.gameObject);
    }

    Vector3 start = player.position;
    Vector3 end = target.position;

    for (int i = 1; i <= numberOfNotes; i++)
    {
        float progress = (float)i / numberOfNotes;

        Vector3 position = Vector3.Lerp(
            start,
            end,
            progress
        );

        GameObject newNote = Instantiate(
            notePrefab,
            position,
            Quaternion.identity,
            transform
        );

        MarchingNote note = newNote.GetComponent<MarchingNote>();

        note.expectedProgress = (float)i / numberOfNotes;
    }
}
}
