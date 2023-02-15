using Microsoft.EntityFrameworkCore;
using Rybalka.Core.Entities;
using Rybalka.Core.Interfaces.Repositories;
using Rybalka.Infrastructure.Data;

namespace Rybalka.Infrastructure.Repositories
{
    public sealed class FishingNoteRepository : IFishingNoteRepository
    {
        private readonly ApplicationDbContext _db;
        public FishingNoteRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<FishingNote>> GetFishingNotesReadOnly()
        {
            return await _db.FishingNotes.AsNoTracking().ToListAsync();
        }

        public async Task<FishingNote?> GetFishingNoteByIdReadOnly(int id)
        {
            return await _db.FishingNotes.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<FishingNote?> GetFishingNoteById(int id)
        {
            return await _db.FishingNotes.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<List<FishingNote>> GetFishingNotesByUserReadOnly(string user)
        {
            return await _db.FishingNotes.AsNoTracking().Where(n => n.User == user).ToListAsync();
        }

        public async Task CreateFishingNote(FishingNote note)
        {
            _db.FishingNotes.Add(note);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteFishingNote(FishingNote note)
        {
            _db.FishingNotes.Remove(note);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateFishingNote(FishingNote note)
        {
            _db.Update(note);
            await _db.SaveChangesAsync();
        }
    }
}
