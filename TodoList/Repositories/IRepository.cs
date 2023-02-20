using TodoList.Models;

namespace TodoList.Repositories;

public interface IRepository
{
    Task AddItem(TodoItem todoItem);
    Task RemoveItem(int id);
    Task<IEnumerable<TodoItem>?> GetItems();
    Task<TodoItem?> GetItemById(int id);
    Task EditItem(int id, TodoItem todoItem);
    public Task<bool> TodoItemExists(int id);
    Task SaveChanges();
}