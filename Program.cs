using System;
using System.Collections.Generic;
enum ProjectStatus
{
    Project,
    Execution,
    Closed
}
enum TaskStatus
{
    Assigned,
    InProgress,
    UnderReview,
    Completed
}
class Person
{
    public string Name { get; set; }
    public Person(string name)
    {
        Name = name;
    }
}
class Report
{
    public string Text { get; private set; }
    public DateTime Date { get; private set; }
    public Person Executor { get; private set; }
    public Report(string text, Person executor)
    {
        Text = text;
        Date = DateTime.Now;
        Executor = executor;
    }
    public void Revise(string text, Person executor)
    {
        Text = text;
        Date = DateTime.Now;
        Executor = executor;
    }
    public void Print()
    {
        Console.WriteLine("Отчет: " + Text);
        Console.WriteLine("Дата: " + Date);
        Console.WriteLine("Исполнитель: " + Executor.Name);
    }
}
class Task
{
    public string Description { get; private set; }
    public DateTime Deadline { get; private set; }
    public Person Initiator { get; private set; }
    public Person? Executor { get; private set; }
    public Person TeamLead { get; private set; }
    public TaskStatus Status { get; private set; }
    public Report? Report { get; private set; }
    public Task(string description, DateTime deadline, Person initiator, Person executor, Person teamLead)
    {
        Description = description;
        Deadline = deadline;
        Initiator = initiator;
        Executor = executor;
        TeamLead = teamLead;
        Status = TaskStatus.Assigned;
    }
    public void Take()
    {
        if (Status == TaskStatus.Assigned && Executor != null)
        {
            Status = TaskStatus.InProgress;
            Console.WriteLine(Executor.Name + " взял задачу.");
        }
    }
    public void Delegate(Person newExecutor)
    {
        if ((Status == TaskStatus.Assigned || Status == TaskStatus.InProgress) && Executor != null && Report == null && Executor != newExecutor)
        {
            Executor = newExecutor;
            Status = TaskStatus.Assigned;
            Console.WriteLine("Задача передана " + newExecutor.Name);
        }
    }
    public void Reject()
    {
        if ((Status == TaskStatus.Assigned || Status == TaskStatus.InProgress) && Executor != null && Report == null)
        {
            Console.WriteLine(Executor.Name + " отклонил задачу.");
            Executor = null;
            Status = TaskStatus.Assigned;
        }
    }
    public void Assign(Person newExecutor)
    {
        if (Executor == null && Status == TaskStatus.Assigned && newExecutor != TeamLead)
        {
            Executor = newExecutor;
            Console.WriteLine("Задача назначена " + newExecutor.Name);
        }
    }
    public void SubmitReport(string text)
    {
        if (Status == TaskStatus.InProgress && Executor != null)
        {
            if (Report == null)
                Report = new Report(text, Executor);
            else
                Report.Revise(text, Executor);
            Status = TaskStatus.UnderReview;
            Console.WriteLine("Отчет отправлен на проверку.");
        }
    }
    public void ApproveReport(Person reviewer)
    {
        if (Status == TaskStatus.UnderReview && Report != null && reviewer == Initiator)
        {
            Status = TaskStatus.Completed;
            Console.WriteLine("Отчет утвержден.");
        }
    }
    public void ReturnReport(Person reviewer)
    {
        if (Status == TaskStatus.UnderReview && Report != null && reviewer == Initiator)
        {
            Status = TaskStatus.InProgress;
            Console.WriteLine("Отчет возвращен на доработку.");
        }
    }
    public void Print()
    {
        Console.WriteLine("Задача: " + Description);
        Console.WriteLine("Срок: " + Deadline.ToShortDateString());
        Console.WriteLine("Инициатор: " + Initiator.Name);
        Console.WriteLine("Исполнитель: " + (Executor == null ? "Нет" : Executor.Name));
        Console.WriteLine("Статус: " + Status);
        if (Report != null)
            Report.Print();
    }
}
class Project
{
    public string Description { get; private set; }
    public DateTime Deadline { get; private set; }
    public Person Initiator { get; private set; }
    public Person TeamLead { get; private set; }
    public ProjectStatus Status { get; private set; }
    private List<Task> tasks = new List<Task>();
    public Project(string description, DateTime deadline, Person initiator, Person teamLead)
    {
        Description = description;
        Deadline = deadline;
        Initiator = initiator;
        TeamLead = teamLead;
        Status = ProjectStatus.Project;
    }
    public void AddTask(string description, DateTime deadline, Person initiator, Person executor)
    {
        if (Status == ProjectStatus.Project && deadline <= Deadline)
        {
            tasks.Add(new Task(description, deadline, initiator, executor, TeamLead));
            Console.WriteLine("Создана задача: " + description);
        }
    }
    public void Start()
    {
        if (Status == ProjectStatus.Project && tasks.Count > 0)
        {
            bool allAssigned = true;
            foreach (Task task in tasks)
            {
                if (task.Executor == null)
                    allAssigned = false;
            }
            if (allAssigned)
            {
                Status = ProjectStatus.Execution;
                Console.WriteLine("Проект перешел в исполнение.");
            }
        }
    }
    public Task GetTask(int index)
    {
        return tasks[index];
    }
    public void AssignRejectedTask(Task task, Person executor)
    {
        if (Status == ProjectStatus.Execution && tasks.Contains(task) && task.Executor == null)
            task.Assign(executor);
    }
    public void RemoveRejectedTask(Task task)
    {
        if (Status == ProjectStatus.Execution && tasks.Contains(task) && task.Executor == null)
        {
            tasks.Remove(task);
            Console.WriteLine("Задача удалена из проекта.");
        }
    }
    public void CheckClose()
    {
        if (Status == ProjectStatus.Execution && tasks.Count > 0)
        {
            bool allCompleted = true;
            foreach (Task task in tasks)
            {
                if (task.Status != TaskStatus.Completed)
                    allCompleted = false;
            }
            if (allCompleted)
            {
                Status = ProjectStatus.Closed;
                Console.WriteLine("Проект закрыт.");
            }
        }
    }
    public void Print()
    {
        Console.WriteLine("\nПроект: " + Description);
        Console.WriteLine("Срок: " + Deadline.ToShortDateString());
        Console.WriteLine("Заказчик: " + Initiator.Name);
        Console.WriteLine("Тимлид: " + TeamLead.Name);
        Console.WriteLine("Статус: " + Status);
        foreach (Task task in tasks)
        {
            Console.WriteLine();
            task.Print();
        }
    }
}
class Program
{
    static void Main()
    {
        List<Person> team = new List<Person>();
        for (int i = 1; i <= 10; i++)
            team.Add(new Person("Сотрудник " + i));
        Person customer = new Person("Заказчик");
        Person teamLead = team[0];
        Project project = new Project("Task Manager", DateTime.Now.AddMonths(2), customer, teamLead);
        for (int i = 0; i < 10; i++)
            project.AddTask("Задача " + (i + 1), DateTime.Now.AddDays(10 + i), customer, team[i]);
        project.AddTask("Задача 11", DateTime.Now.AddDays(20), customer, team[1]);
        project.Start();
        Task task1 = project.GetTask(0);
        task1.Delegate(team[1]);
        task1.Take();
        task1.SubmitReport("Задача выполнена.");
        task1.ApproveReport(customer);
        Task task2 = project.GetTask(1);
        task2.Reject();
        project.AssignRejectedTask(task2, team[2]);
        task2.Take();
        task2.SubmitReport("Задача выполнена.");
        task2.ApproveReport(customer);
        Task task3 = project.GetTask(10);
        task3.Reject();
        project.RemoveRejectedTask(task3);
        Task task4 = project.GetTask(3);
        task4.Take();
        task4.SubmitReport("Первый отчет.");
        task4.ReturnReport(customer);
        task4.Take();
        task4.SubmitReport("Отчет исправлен.");
        task4.ApproveReport(customer);
        for (int i = 2; i < 10; i++)
        {
            if (i == 3)
                continue;
            Task task = project.GetTask(i);
            task.Take();
            task.SubmitReport("Задача выполнена.");
            task.ApproveReport(customer);
        }
        project.CheckClose();
        project.Print();
    }
}