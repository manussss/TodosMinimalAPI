using Flunt.Notifications;
using Flunt.Validations;
using Todos.MinimalAPI.Models;

namespace Todos.MinimalAPI.ViewModels
{
    public class CreateTodoViewModel : Notifiable<Notification>
    {
        public string Title { get; set; }

        public Todo MapTo()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Title, "Informe o titulo")
                .IsGreaterThan(Title, 5, "Tamanho deve ser maior que 5"));

            return new Todo(Guid.NewGuid(), Title, false);
        }
    }
}
