using EFRepDemo.Data;
using EFRepDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFRepDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (TodoContext context = new TodoContext())
            {
                User user = null;
                while (true)
                {
                    if (user == null)
                    {
                        user = UserMenu(context);
                    }
                    else
                    {
                        TodoMenu(context, user);
                        user = null;
                    }
                }
            }
        }

        static User UserMenu(TodoContext context)
        {
            Console.Clear();
            List<User> users = context.Users.ToList();
            for (int i = 0; i < users.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {users[i].Name}");
            }
            Console.WriteLine("Press Enter to create a new user");
            Console.WriteLine("Press D to delete a user");
            Console.WriteLine("Choose your user number or press Q to quit: ");
            string input = Console.ReadLine().ToLower();

            switch (input)
            {
                case "":
                    Console.Write("Enter username: ");
                    string name = Console.ReadLine();
                    User user = new User()
                    {
                        Name = name
                    };
                    context.Users.Add(user);
                    context.SaveChanges();
                    return user;
                case "d":
                    DeleteUser(context, users);
                    return null;
                case "q":
                    Environment.Exit(0);
                    break;
                default:
                    int menuChoice = Convert.ToInt32(input) - 1;
                    return users[menuChoice];
            }

            return null;
        }

        static void DeleteUser(TodoContext context, List<User> users)
        {
            Console.Write("Enter the number of the user to delete: ");
            if (int.TryParse(Console.ReadLine(), out int deleteUser) && deleteUser >= 1 && deleteUser <= users.Count)
            {
                User userToDelete = users[deleteUser - 1];
                context.Users.Remove(userToDelete);
                context.SaveChanges();
                Console.WriteLine("User deleted.");
            }
            else
            {
                Console.WriteLine("Invalid input. User not deleted.");
            }
        }

        static void TodoMenu(TodoContext context, User user)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"{user.Name}'s todo list");

                List<TodoItem> todoItems = context.Users
                    .Where(u => u.Id == user.Id)
                    .Include(u => u.TodoItems)
                    .Single()
                    .TodoItems
                    .ToList();

                for (int i = 0; i < todoItems.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {todoItems[i].Title}: {todoItems[i].Description}");
                }
                Console.WriteLine("Press Enter to create a new todo item");
                Console.WriteLine("Press D to delete a todo item");
                Console.WriteLine("Press B to go back to the user menu");
                Console.WriteLine("Press Q to quit");
                string input = Console.ReadLine().ToLower();

                switch (input)
                {
                    case "":
                        Console.Write("Enter title: ");
                        string title = Console.ReadLine();
                        string titleInput = title.ToUpper();
                        Console.Write("Enter description: ");
                        string description = Console.ReadLine();
                        TodoItem newItem = new TodoItem()
                        {
                            Title = titleInput,
                            Description = description,
                            User = user
                        };
                        context.TodoItems.Add(newItem);
                        context.SaveChanges();
                        break;
                    case "d":
                        Console.Write("Enter the number of the todo item to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteItem) && deleteItem >= 1 && deleteItem <= todoItems.Count)
                        {
                            TodoItem itemToDelete = todoItems[deleteItem - 1];
                            context.TodoItems.Remove(itemToDelete);
                            context.SaveChanges();
                        }
                        break;
                    case "b":
                        return;
                    case "q":
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
