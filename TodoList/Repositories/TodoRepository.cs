using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Models;

namespace TodoList.Repositories;

public class TodoRepository : IRepository
{
    private readonly TodoDbContext _todoDbContext;

    public TodoRepository(TodoDbContext todoDbContext)
    {
        _todoDbContext = todoDbContext;
    }
    
    public async Task AddItem(TodoItem todoItem)
    {
        await _todoDbContext.AddAsync(todoItem);
    }

    public async Task RemoveItem(int id)
    {
        var todoItem = await _todoDbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == id);

        if (todoItem is not null)
        {
            _todoDbContext.Remove(todoItem);
        }
    }

    public async Task<IEnumerable<TodoItem>?> GetItems()
    {
        var items = await _todoDbContext.TodoItems
                                                    .AsNoTracking()
                                                    .ToListAsync();

        return items;
    }

    public async Task<TodoItem?> GetItemById(int id)
    {
        var todoItem = await _todoDbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == id);

        return todoItem;
    }

    public async Task EditItem(int id, TodoItem todoItem)
    {
        var oldTodoItem = await _todoDbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == id);

        if (oldTodoItem is not null)
        {
            oldTodoItem.Task = todoItem.Task;
            oldTodoItem.IsCompleted = todoItem.IsCompleted;
        }
    }

    public async Task SaveChanges()
    {
        await _todoDbContext.SaveChangesAsync();
    }

    public async Task<bool> TodoItemExists(int id)
    {
        return await _todoDbContext.TodoItems.AnyAsync(t => t.Id == id);
    }
}