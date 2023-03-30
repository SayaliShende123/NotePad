using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.API.Data;
using Notes.API.Models.Entities;

namespace Notes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        private readonly NotesDbContext notesDbContext;

        public NotesController(NotesDbContext notesDbContext)
        {
            this.notesDbContext = notesDbContext;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            //Get the notes from database
            return Ok(await notesDbContext.MyPropNoteserty.ToListAsync());

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetNoteById")]
        public async Task<IActionResult> GetNoteById([FromRoute] Guid id)
        {
            //Get the note by id from database
            var note=await notesDbContext.MyPropNoteserty.FindAsync(id);

            if(note!=null)
                return Ok(note);

            return NotFound();
           

        }

        [HttpPost]
        public async Task<IActionResult> AddNote(Note note)
        {
            note.Id=Guid.NewGuid();
            await notesDbContext.MyPropNoteserty.AddAsync(note);
            await notesDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, note);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateNote([FromRoute] Guid id,[FromBody]Note UpdatedNote)
        {
            var existingnote = await notesDbContext.MyPropNoteserty.FindAsync(id);

            if (existingnote == null)
                return NotFound();

            existingnote.Title=UpdatedNote.Title;
            existingnote.Description=UpdatedNote.Description;
            existingnote.IsVisible=UpdatedNote.IsVisible;

            await notesDbContext.SaveChangesAsync();

            return Ok(existingnote);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteNote([FromRoute] Guid id)
        {
            var existingnote = await notesDbContext.MyPropNoteserty.FindAsync(id);

            if (existingnote == null)
                return NotFound();

            notesDbContext.MyPropNoteserty.Remove(existingnote);
            await notesDbContext.SaveChangesAsync();

            return Ok();
        }


    }
}
