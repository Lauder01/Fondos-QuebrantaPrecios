using System;

namespace WebAPI.Dtos.User
{
    public class FQPUserGetterDto : FQPUserBaseDto
    {
        public Guid Id { get; set; }
        // Hereda los demás campos del base
    }
}
