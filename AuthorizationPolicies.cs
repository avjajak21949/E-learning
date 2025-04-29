using Microsoft.AspNetCore.Authorization;

namespace Demo03.Authorization
{
    public class AuthorizationPolicies
    {
        public static void AddPolicies(AuthorizationOptions options)
        {
            // Manager policy
            options.AddPolicy("ManagerPolicy", policy =>
                policy.RequireRole("Manager"));

            // Teacher policy
            options.AddPolicy("TeacherPolicy", policy =>
                policy.RequireRole("Teacher"));

            // Student policy
            options.AddPolicy("StudentPolicy", policy =>
                policy.RequireRole("Student"));

            // CRUD operations for Manager
            options.AddPolicy("ManagerCRUDPolicy", policy =>
                policy.RequireRole("Manager"));

            // CRUD operations for Teacher
            options.AddPolicy("TeacherCRUDPolicy", policy =>
                policy.RequireRole("Teacher"));

            // CRUD operations for Student
            options.AddPolicy("StudentCRUDPolicy", policy =>
                policy.RequireRole("Student"));

            // Combined policies for common operations
            options.AddPolicy("TeacherOrManagerPolicy", policy =>
                policy.RequireRole("Teacher", "Manager"));

            options.AddPolicy("StudentOrTeacherPolicy", policy =>
                policy.RequireRole("Student", "Teacher"));
        }
    }
} 