using RestHelpers.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestHelpers.Tests
{
    /// <summary>
    ///     User test model.
    /// </summary>
    public class UserTestModel
    {
        [DtoRequired]
        public int Id { get; set; } = 1;

        [DtoRequired]
        public string Username { get; set; } = "alice";

        public string Firstname { get; set; } = "Alice";

        public string Lastname { get; set; } = "Doe";

        public string Email { get; set; } = "adoe@email.com";
    }
}
