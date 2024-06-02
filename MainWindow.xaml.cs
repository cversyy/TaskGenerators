using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;
using Program.BL;
using Program.Structures;

namespace Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TaskGenerator CurrentTask { get; set; }
        GeneratorResult CurrentResult { get; set; }
        string CurrentResultType { get; set; }
        List<TaskGenerator> TASKS = new List<TaskGenerator>();
        List<string> TAGS = new List<string>();
        List<string> CurrentTags = new List<string>();
        String CurrentSubject;
        public MainWindow()
        {
            InitializeComponent();
            Main.GetGenerators().ForEach(task => TASKS.Add(task));
            SubjectBox.SelectedIndex = 0;
            List<string> SUBJECTS = new List<string>() { "Все", "Математика", "Русский язык", "Физика" };
            foreach (string SUBJECT in SUBJECTS) { SubjectBox.Items.Add(SUBJECT); }
            TAGS = SearchTags(TASKS);
            AddTags(TAGS);
            addButtons(TASKS);
            CurrentSubject = "Все";
        }
        private List<string> SearchTags(List<TaskGenerator> tasks)
        {
            List<string> tags = new List<string>();
            foreach (TaskGenerator task in tasks)
            {
                foreach (string tag in task.Tags)
                {
                    if (tags.Contains(tag) == false)
                    {
                        tags.Add(tag);
                    }
                }
            }
            return tags;
        }
        private void AddTags(List<string> tags)
        {

            foreach (string tag in tags)
            {
                CheckBox b = new CheckBox();
                b.Content = tag;
                b.HorizontalAlignment = HorizontalAlignment.Left;
                b.Height = 20;
                b.Width = 150;
                var converter = new BrushConverter();
                b.Foreground = (Brush?)converter.ConvertFromString("#ebdbb2");
                b.Checked += TagsChecked;
                b.Unchecked += TagsUnchecked;
                b.IsChecked = true;
                TagsPanel.Children.Add(b);
            }
        }

        private void TagsUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox b = (CheckBox)sender;
            CurrentTags.Remove(b.Content.ToString());
            StringBuilder sb = new StringBuilder();
            foreach (var tag in CurrentTags)
            {
                sb.Append(tag.ToString() + " ");
            }
            Description.Text = sb.ToString();
            addButtons(TASKS);

        }
        private void TagsChecked(object sender, RoutedEventArgs e)
        {
            CheckBox b = (CheckBox)sender;
            CurrentTags.Add(b.Content.ToString());
            StringBuilder sb = new StringBuilder();
            addButtons(TASKS);
            Description.Text = sb.ToString();
            
        }
        private void addButtons(List<TaskGenerator> TASKS)
        {   
            ButtonPanel.Children.Clear();
            foreach (var task in TASKS)
            {   
                foreach(string tag in CurrentTags)
                {
                    if (task.Tags.Contains(tag) && task.Subject == CurrentSubject)
                    {
                        CreateButton(task);
                        break;
                    }
                    else
                    {
                        if (task.Tags.Contains(tag) && CurrentSubject == "Все")
                        {
                            CreateButton(task);
                            break;
                        }
                    }
                }

            }
           
        }
        private void CreateButton(TaskGenerator task)
        {
            Button b = new Button();
            b.Content = task.Name;
            b.Height = 50;
            b.Width = 580;
            b.FontSize = 20;
            var converter = new BrushConverter();
            b.Background = (Brush?)converter.ConvertFromString("#3c3836");
            b.Foreground = (Brush?)converter.ConvertFromString("#ebdbb2");
            b.OpacityMask = (Brush?)converter.ConvertFromString("#504945");
            b.Click += (o, e) =>
            {
                Description.Text = task.Description;
            };
            b.Click += (o, e) =>
            {
                CurrentTask = task;
            };
            ButtonPanel.Children.Add(b);
        }
        private void addButton(TaskGenerator task, List<string> tags)
        {    
            foreach(var tag in CurrentTags)
            {
                if(task.Tags.Contains(tag.ToString())) 
                {
                    CreateButton(task);
                    break;
                }
            }
            

        }

        
        private void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTask != null)
            {
                GeneratorRequest Request = new(AmountOfTasks.Text, CurrentTask.Path);
                if (Annotations.IsChecked == true)
                {
                    Request = new(AmountOfTasks.Text, CurrentTask.Path, true);
                }
                CurrentResult = Main.GenerateTask(Request);
                
                Result.Text = CurrentResult.Result;
                Result.Text += CurrentResult.Annotations;
            }
            else { MessageBox.Show("Выберете задачу"); }
        }


        private void SubjectBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            while (ButtonPanel.Children.Count > 0)
                ButtonPanel.Children.RemoveAt(0);
            switch (SubjectBox.SelectedIndex)
            {
                case 0:
                    foreach (var task in TASKS)
                    {
                        addButton(task, CurrentTags);
                    }
                    CurrentSubject = "Все";
                    Description.Text = CurrentSubject;
                    break;
                case 1:
                    foreach (var task in TASKS) 
                    { 
                        if (task.Subject == "Математика")
                        {
                            addButton(task, CurrentTags);
                        }
                    }
                    CurrentSubject = "Математика";
                    break;
                case 2:
                    foreach (var task in TASKS) 
                    {
                        if (task.Subject == "Русский язык") 
                        {
                            addButton(task, CurrentTags); 
                        }
                    }
                    CurrentSubject = "Русский язык";
                    break;
                case 3:
                    foreach (var task in TASKS) 
                    {
                        if (task.Subject == "Физика")
                        {
                            addButton(task, CurrentTags); 
                        }
                    }
                    CurrentSubject = "Физика";
                    break;

            }

        }

        private void GitHubClick(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.youtube.com/watch?v=dQw4w9WgXcQ") { UseShellExecute = true });
        }
    }
}
