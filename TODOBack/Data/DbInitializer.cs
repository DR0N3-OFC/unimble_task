using TODOBack.Models;

namespace TODOBack.Data
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Users!.Any() || context.Tasks!.Any())
            {
                return;
            }

            var users = new List<UserModel>
            {
                new UserModel
                {
                    UserName = "brunofacundo",
                    Email = "bruno@email.com",
                    Password = "bruno123",
                },
                new UserModel
                {
                    UserName = "zeca03",
                    Email = "zezinho@email.com",
                    Password = "zezinho123",
                }
            };

            context.AddRange(users);

            var tasks = new List<TaskModel>
            {
                new TaskModel
                {
                    Description = "Fazer trabalho de Gerenciamento de Projetos",
                    Status = 0,
                    Organizer = users[0],
                    OrganizerId = 1
                },

                new TaskModel
                {
                    Description = "Fazer trabalho de Tópicos Avançados em Computação.",
                    Status = 1,
                    Organizer = users[1],
                    OrganizerId = 2
                },
            };

            context.AddRange(tasks);

            context.SaveChanges();
        }
    }
}
