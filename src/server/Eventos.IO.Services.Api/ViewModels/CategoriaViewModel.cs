using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Api.ViewModels
{
    public class CategoriaViewModel
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }
        
    }
}
