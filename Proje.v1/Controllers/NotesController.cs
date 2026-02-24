
using Microsoft.AspNetCore.Mvc;
using Proje.v1.Data;
using Proje.v1.Models;

namespace Proje.v1.Controllers
{
        [ApiController]
        [Route("api/notes")]
        public class NotesController:ControllerBase
        {
            [HttpGet]
            public IActionResult GetNotes()
            {
                var result = NoteStore.Notes
                    .Where(x => !x.IsArchived)
                    .OrderByDescending(x => x.CreatedDate)
                    .ToList();

                return Ok(result);
            }

           [HttpPost]
           public IActionResult CreateNote(CreateNoteDto dto)
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                    return BadRequest("Title boş olamaz");

                if (string.IsNullOrWhiteSpace(dto.Content) || dto.Content.Length < 5)
                    return BadRequest("Content en az 5 karakter olmalı");

                var newNote = new Note
                {
                    Id = NoteStore.Notes.Count == 0 ? 1: NoteStore.Notes.Max(x => x.Id) + 1,
                    Title = dto.Title,
                    Content = dto.Content,
                    IsArchived = false,
                    CreatedDate = DateTime.Now
                };

                NoteStore.Notes.Add(newNote);

                return CreatedAtAction(
                    nameof(GetNotes),
                    new { id = newNote.Id },
                    newNote
                );
            }
            [HttpGet("{id}")]
             public IActionResult GetNoteById(int id)
             {
              var note = NoteStore.Notes.FirstOrDefault(x => x.Id == id && !x.IsArchived);
                if (note == null)
                    return NotFound();
                return Ok(note);
                }

            [HttpDelete("{id}")]
            public IActionResult DeleteNote(int id)
            { 
            var note = NoteStore.Notes.FirstOrDefault(x => x.Id == id);
                if (note == null) 
                
                 return NotFound();

                note.IsArchived = true;
                return NoContent();
            }

            [HttpGet("search")]
            public IActionResult SearchNotes([FromQuery] string? search)
            {
                if (string.IsNullOrWhiteSpace(search))
                    return BadRequest("Search boş olamaz!!");
                var result = NoteStore.Notes
                    .Where(x => !x.IsArchived  && x.Title != null && x.Title.Contains(search) )
                    .OrderByDescending(x => x.CreatedDate)
                    .ToList();

                return Ok(result);
            }
        }
    }
