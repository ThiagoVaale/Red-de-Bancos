using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domine.Entities
{
    public sealed class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public bool IsActive { get; private set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        private User() { } 
        public User(string fullName, string email)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name is required.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");

            FullName = fullName.Trim();
            Email = email.Trim().ToLowerInvariant();
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
