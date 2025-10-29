using System.ComponentModel.DataAnnotations;

namespace Core.Models.Token
{
    public class TokenResquest
    {
        [Required(ErrorMessage = "UserName is required")]
        [Display(Name = "UserName")]
        [MatchConfigUsername]
        public string? userName { get; set; }
    }
}
