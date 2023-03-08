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

        public async Task<List<FishingNote>> GetFishingNotesReadOnly(CancellationToken ct)
        {
            return await _db.FishingNotes.AsNoTracking().ToListAsync(ct);
        }

        public async Task<FishingNote?> GetFishingNoteByIdReadOnly(int id, CancellationToken ct)
        {
            return await _db.FishingNotes.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id, ct);
        }

        public async Task<FishingNote?> GetFishingNoteById(int id, CancellationToken ct)
        {
            return await _db.FishingNotes.FirstOrDefaultAsync(n => n.Id == id, ct);
        }

        public async Task<List<FishingNote>> GetFishingNotesByUserReadOnly(string user, CancellationToken ct)
        {
            return await _db.FishingNotes.AsNoTracking().Where(n => n.Username == user).ToListAsync(ct);
        }

        public async Task CreateFishingNote(FishingNote note, CancellationToken ct)
        {
            _db.FishingNotes.Add(note);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteFishingNote(FishingNote note, CancellationToken ct)
        {
            _db.FishingNotes.Remove(note);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateFishingNote(FishingNote note, CancellationToken ct)
        {
            _db.Update(note);
            await _db.SaveChangesAsync(ct);
        }
    }
}
