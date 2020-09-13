using System.ComponentModel.DataAnnotations;

namespace ParperaEngineeringTest.Models.ViewModels
{
    public class StatusUpdate
    {
        [Required]
        public string Status { get; set; }
    }
}
