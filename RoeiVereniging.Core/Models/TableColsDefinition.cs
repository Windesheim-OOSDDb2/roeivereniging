
namespace RoeiVereniging.Core.Models
{
    public class TableColumnDefinition
    {
        public string Header { get; set; } = string.Empty;

        
        public string BindingPath { get; set; } = string.Empty;


        // Optiona format like datetime etc
        public string? StringFormat { get; set; }
    }
}
