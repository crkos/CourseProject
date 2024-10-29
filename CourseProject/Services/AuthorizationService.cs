using CourseProject.CustomAtributes;
using CourseProject.Interfaces;
using CourseProject.Models;
using Microsoft.AspNetCore.Identity;

namespace CourseProject.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public AuthorizationService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        // I guess if needed to implement methods where

        public async Task<bool> CanEditResource<T>(T resource)
        {
            var user = _httpContextAccessor.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? false)
            {
                return false;
            }

            if (await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "Admin"))
            {
                return true;
            }

            var currentUser = await _userManager.GetUserAsync(user);

            var ownerIdProperty = typeof(T).GetProperties()
            .FirstOrDefault(p => Attribute.IsDefined(p, typeof(OwnerIdAttribute)));

            if (ownerIdProperty == null)
            {
                throw new InvalidOperationException($"No property with [OwnerIdAttribute] found in {typeof(T).Name}");
            }

            var ownerId = (int?)ownerIdProperty.GetValue(resource);
            return ownerId == currentUser?.Id;
        }
    }
}
