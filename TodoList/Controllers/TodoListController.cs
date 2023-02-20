using Microsoft.AspNetCore.Mvc;
using TodoList.Models;
using TodoList.Repositories;

namespace TodoList.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoListController: ControllerBase
{
    private readonly IRepository _repository;

    public TodoListController(IRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
    {
        var todoItems = await _repository.GetItems();
        
        if (todoItems is null)
        {
            return NotFound();
        }

        return Ok(todoItems);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
    {
        var todoItemExists = await _repository.TodoItemExists(id);
        if (!todoItemExists)
        {
            return NotFound();
        }
        
        var todoItem = await _repository.GetItemById(id);

        if (todoItem is null)
        {
            return NotFound();
        }

        return Ok(todoItem);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
    {
        if (string.IsNullOrWhiteSpace(todoItem.Task))
        {
            return BadRequest();
        }

        await _repository.AddItem(todoItem);
        await _repository.SaveChanges();

        return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutTodoItem(int id, TodoItem todoItem)
    {
        if (string.IsNullOrWhiteSpace(todoItem.Task))
        {
            return BadRequest();
        }

        var todoItemExists = await _repository.TodoItemExists(id);
        if (!todoItemExists)
        {
            return NotFound();
        }
        
        if (id != todoItem.Id)
        {
            return BadRequest();
        }

        await _repository.EditItem(id, todoItem);
        await _repository.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTodoItem(int id)
    {
        var todoItemExists = await _repository.TodoItemExists(id);
        if (!todoItemExists)
        {
            return NotFound();
        }
        
        await _repository.RemoveItem(id);
        await _repository.SaveChanges();

        return NoContent();
    }
}