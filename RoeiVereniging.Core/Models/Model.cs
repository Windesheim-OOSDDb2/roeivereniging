using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Models
{
    public abstract partial class Model(int id, string name) : ObservableObject
    {
        public int Id { get; set; } = id;
        [ObservableProperty]
        public string name = name;
    }
}
