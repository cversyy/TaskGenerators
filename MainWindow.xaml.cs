using Program.BL;
using Program.Extensions;
using Program.Structures;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents.Serialization;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;



namespace Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static MainWindow Window;

        TaskGenerator CurrentTask { get; set; }

        GeneratorResult CurrentResult { get; set; }

        /// <summary>
        /// Путь к папке программы
        /// </summary>
        string PATH = SetPaths();

        string CurrentResultType { get; set; }
        int CheckedTags { get; set; }
        /// <summary>
        /// Все генераторы
        /// </summary>
        List<TaskGenerator> TASKS = Main.GetGeneratorsList();


        List<TaskGenerator> CurrentTasks = new List<TaskGenerator>();

        /// <summary>
        /// Все тэги
        /// </summary>
        List<string> TAGS = new List<string>();

        List<string> CurrentTags = new List<string>();

        String CurrentSubject;
        /// <summary>
        /// Все предметы
        /// </summary>
        List<string> SUBJECTS = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            Window = this;
            SubjectBox.SelectedIndex = 0;
            GetSubjects(SUBJECTS);
            SUBJECTS = SUBJECTS.Distinct().ToList();
            SetSubjects(SUBJECTS);
            TAGS = SearchTags(TASKS);
            AddTags(SortTags(TAGS));

        }
        #region Получение и установка предметов
        private void GetSubjects(List<string> Subjects)
        {
            Subjects.Add("Все");
            foreach (TaskGenerator task in TASKS)
            {
                Subjects.Add(task.Subject);
            }
        }
        private void SetSubjects(List<string> Subjects)
        {
            foreach (string subject in Subjects)
            {
                SubjectBox.Items.Add(subject);
            }
        }
        #endregion
        private void ClearPanels()
        {
            ButtonPanel.Children.Clear();
            CurrentTasks.Clear();
        }
        private void ChangePannels()
        {
            ClearPanels();
            if (CurrentSubject == "Все")
            {
                if (CheckedTags != 0)
                {
                    foreach (TaskGenerator task in TASKS)
                    {
                        if (TagsContainCheck(task))
                        {
                            RefreshButton(task);
                        }
                    }
                    
               }
                else if (CheckedTags == 0)
                {
                    foreach (TaskGenerator task in TASKS)
                    {
                        RefreshButton(task);
                    }
                }
            }
            else
            {
                if (CheckedTags != 0)
                {
                    
                    foreach (TaskGenerator task in TASKS)
                    {
                        if (TagsContainCheck(task))
                        {
                            RefreshButton(task);
                        }
                    }
                    
                }
                if (CheckedTags == 0)
                {
                    foreach(TaskGenerator task in TASKS)
                    {
                        if (task.Subject == CurrentSubject)
                        {
                            RefreshButton(task);
                        }
                    }
                    
                }
            }
        }

        private void RefreshButton(TaskGenerator task)
        {
            CurrentTasks.Add(task);
            CreateButton(task);
        }

        private bool TagsContainCheck(TaskGenerator task)
        {
            foreach (var tag in CurrentTags)
            {
                if (task.Tags.Contains(tag)) return true;
            }
            return false;
        }
        private void SubjectSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            CurrentSubject = SubjectBox.SelectedItem.ToString();
            
            if(CurrentSubject == "Все")
            {
                PreparePanelsForChange();
                ChangePannels();
                AddTags(SortTags(TAGS));
            }
            else
            {
                PreparePanelsForChange();
                ChangePannels();
                AddTags(SortTags(SearchTags(CurrentTasks)));
            }
            
        }

        private List<string> SortTags(List<string> tags)
        {
            List<string> TagsWords = new List<string>();
            List<string> TagsNumbers = new List<string>();
            foreach(string tag in tags)
            {
                string FirstChar = tag[0].ToString();
                if (Int32.TryParse(FirstChar, out int value))
                {
                    TagsNumbers.Add(tag);
                }
                else
                {
                    TagsWords.Add(tag);
                }
                
            }
            List<string> sortedWords = TagsWords.OrderBy(s => s).ToList();
            List<string> sortedNumbers = TagsNumbers.OrderBy(s => s).ToList();
            return sortedNumbers.Union(sortedWords).ToList();
        }

        private void PreparePanelsForChange()
        {
            CurrentTags.Clear();
            CheckedTags = 0;
            ButtonPanel.Children.Clear();
            TagsPanel.Children.Clear();
            
        }
        /// <summary>
        /// Проверка на наличие тегов в генераторе
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        
        #region Добавление тэгов
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
        

        private void AddTag(string tag, bool isChecked)
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
            if (isChecked)
            {
                b.IsChecked = true;
            }
            else b.IsChecked = false;
            TagsPanel.Children.Add(b);
        }

        private void AddTags(List<string> tags)
        {

            foreach (string tag in tags)
            {
                AddTag(tag, false);
            }
        }
        #endregion
        private void TagsUnchecked(object sender, RoutedEventArgs e)
        {
            RemoveTag(sender);
            ChangePannels();


        }
        private void TagsChecked(object sender, RoutedEventArgs e)
        {
            AddTag(sender);
            ChangePannels();
        }
        private void RemoveTag(object sender)
        {
            CheckedTags -= 1;
            CheckBox b = (CheckBox)sender;
            b.IsChecked = false;
            CurrentTags.Remove(b.Content.ToString());
        }
        private void AddTag(object sender)
        {
            CheckedTags += 1;
            CheckBox b = (CheckBox)sender;
            b.IsChecked = true;
            CurrentTags.Add(b.Content.ToString());
        }
        
        

       
        private void CreateButton(TaskGenerator task)
        {
            #region old
            
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
                CurrentTask = task;
            };
            ButtonPanel.Children.Add(b);
            #endregion

        }
        
        private void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            ImagesPanel.Children.Clear();
            if (CurrentTask != null)
            {

                if (Int32.TryParse(AmountOfTasks.Text, out int value))
                {
                    GeneratorRequest Request = new(value.ToString(), CurrentTask.Path, CurrentTask.Loader);
                    if (Annotations.IsChecked == true)
                    {
                        Request = new(AmountOfTasks.Text, CurrentTask.Path, CurrentTask.Loader, true);
                    }
                    CurrentResult = Main.GenerateTasks(Request);
                    var converter = new BrushConverter();
                    if (CurrentResult.ResultType != null)
                    {
                        for (int i = 0; i < CurrentResult.ResultType.Count; i++)
                        {
                            DisplayTextResults(CurrentResult.Result[i].ToString());
                            DisplayImage(CurrentResult.ResultType[i]);
                        }
                    }
                    else
                    {
                        foreach (var result in CurrentResult.Result)
                        {
                            DisplayTextResults(result.ToString());
                        }
                    }
                    DisplayTextResults(CurrentResult.Annotations);
                    
                }
                else LogError("Incorrect value. Please, use digits");
            }
            else { MessageBox.Show("Выберете задачу"); }
        }

        private void DisplayTextResults(string text)
        {
            var converter = new BrushConverter();
            TextBox resulttext = new TextBox();
            resulttext.Text = text;
            resulttext.IsReadOnly = true;
            resulttext.TextWrapping = TextWrapping.Wrap;
            resulttext.BorderBrush = (Brush?)converter.ConvertFromString("#FFFFFFFF");
            ImagesPanel.Children.Add(resulttext);
        }
        private BitmapImage CreateBitmapImage(string base64Image)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(Convert.FromBase64String(base64Image));
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.EndInit();
            return bi;
        }
        private Image CreateImage(BitmapImage bitmap)
        {
            Image image = new Image();
            image.Width = bitmap.Width;
            image.Height = bitmap.Height;
            image.Source = bitmap;
            return image;
        }
        private void DisplayImage(string image)
        {
            ImagesPanel.Children.Add(CreateImage(CreateBitmapImage(image)));
        }

        

        #region Технические функции
        private static string SetPaths()
        {
            string PATH = Initialization.GetCurrenWorkDir();
            Initialization.SetWorkDirectory(PATH);
            return PATH;
        }
        private void Drag(object sender, RoutedEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                MainWindow.Window.DragMove();
            }
        }

        public static void LogError(string message)
        {
            MessageBox.Show("An unexpected error has occurred: " + message, "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void MinWindow(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        private void FullWindow(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Application.Current.MainWindow.WindowState == WindowState.Normal)
            {
                System.Windows.Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else System.Windows.Application.Current.MainWindow.WindowState = WindowState.Normal;
        }
        private void GitHubClick(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/cversyy/TasksGenerator/wiki") { UseShellExecute = true });
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void PrintTasks(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {

                printDialog.PrintVisual(ImagesPanel, "Printing tasks...");
            }
            

        }
        #endregion
        
    }
}
