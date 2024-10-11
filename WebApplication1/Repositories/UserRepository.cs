using System.Globalization;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Data.DTOs;
using WebApplication1.Exceptions;

namespace WebApplication1.Repositories;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> FindUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new UserNotFoundException("User not found");
        }

        return user;
    }

    public async Task DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new UserNotFoundException("User not found");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(int id, UserDTO user)
    {
        var existingUser = await FindUser(id);
        if (existingUser == null)
        {
            throw new UserNotFoundException("User not found");
        }

        existingUser.FirstName = user.firstName;
        existingUser.LastName = user.lastName;
        await _context.SaveChangesAsync();
    }
}