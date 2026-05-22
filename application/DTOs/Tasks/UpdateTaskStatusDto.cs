using ProjectTaskManagement.Models;

namespace ProjectTaskManagement.DTOs.Tasks;

public class UpdateTaskStatusDto
{
    public TaskItemStatus Status { get; set; }
}
