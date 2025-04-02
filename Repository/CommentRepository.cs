using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comment.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var comment = await _context.Comment.FirstOrDefaultAsync(c => c.Id == id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
                await _context.SaveChangesAsync();
                return comment;
            }
            else
            {
                return null;
            }

        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comment.Include(a=> a.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comment.Include(a=> a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment comment)
        {
            var existingComment = await _context.Comment.FirstOrDefaultAsync(c => c.Id == id);
            if (existingComment != null)
            {
                existingComment.Title = comment.Title;
                existingComment.Content = comment.Content;
                await _context.SaveChangesAsync();
                return existingComment;
            }
            else
            {
                return null;
            }
        }
    }
}