using System;

namespace GoldenStore.Controllers
{
    internal class AuthorizedAttribute : Attribute
    {
        public object Role { get; set; }
    }
}