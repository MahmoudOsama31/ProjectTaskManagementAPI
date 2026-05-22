using ProjectTaskManagement.Models;

namespace ProjectTaskManagement.DTOs.Tasks;

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;
    public DateTime? DueDate { get; set; }
    public TaskItemPriority Priority { get; set; } = TaskItemPriority.Low;
}
