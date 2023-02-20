namespace TodoList.Models;

public class TodoItem
{
    public int Id { get; set; }
    public required string Task { get; set; }
    public bool IsCompleted { get; set; }
}