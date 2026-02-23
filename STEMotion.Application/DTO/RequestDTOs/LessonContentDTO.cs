// DTO dành riêng cho việc Tạo mới
public class CreateLessonContentRequestDTO
{
    public Guid LessonId { get; set; }

    public string ContentType { get; set; }
    public string? TextContent { get; set; }
    public string? MediaUrl { get; set; }
    public string? FormulaLatex { get; set; }
    public int OrderIndex { get; set; }
}

public class UpdateLessonContentRequestDTO
{
    public string ContentType { get; set; }
    public string? TextContent { get; set; }
    public string? MediaUrl { get; set; }
    public string? FormulaLatex { get; set; }
    public int OrderIndex { get; set; }
}