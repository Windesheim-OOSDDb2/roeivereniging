using System.Collections;

namespace RoeiVereniging.Core.Models
{
    public class TableColumnDefinition
    {
        public string Header { get; set; } = string.Empty;
        
        public string BindingPath { get; set; } = string.Empty;

        // Optiona format like datetime etc
        public string? StringFormat { get; set; }

        // TODO: change to enum or find way to get typing for grid column width
        public string Width { get; set; } = "*";
    }
}
