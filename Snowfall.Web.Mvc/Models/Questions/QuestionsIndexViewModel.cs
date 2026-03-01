using Snowfall.Domain.Models;

namespace Snowfall.Web.Mvc.Models.Questions;

public class QuestionsIndexViewModel
{
    public required Evenement Evenement { get; set; }
    public required List<Question> Questions { get; set; }
}