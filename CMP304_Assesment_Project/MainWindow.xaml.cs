using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CMP304_Assesment_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> allFiles = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
        }

        //displays the dragged in file paths
        private void DropTargetlst_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            //fills list box
            foreach (string file in files)
            {
                DropTargetlst.Items.Add(file);
            }
        }

        private void DropTargetlst_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effects = DragDropEffects.All;
            }
        }

        //proccesses and displayed results
        private void processBtn_Click(object sender, EventArgs e)
        {
            DataGathered data = new DataGathered();

            //delete any previously stored list items 
            allFiles.Clear();

            //stores the list box items to the list
            for (int i = 0; i < DropTargetlst.Items.Count; i++)
                allFiles.Add(DropTargetlst.Items[i].ToString());

            //sends the list to the ProcessInput class
            ProcessInput inputs = new ProcessInput();
            ProcessInput.SortFiles(allFiles, data);

            //clears the list box
            DropTargetlst.Items.Clear();

            //display the model test results is present
            if (!string.IsNullOrEmpty(data.testResults))
                MessageBox.Show(data.testResults);

            //checks if, and displayed any files that are not images or csv files
            if (data.nonCompatibleFiles != null)
            {
                if (data.nonCompatibleFiles.Count > 0)
                {
                    var nonCompatibleFiles = string.Join(Environment.NewLine, data.nonCompatibleFiles);
                    MessageBox.Show("Files That Could Not Be Processed: \n" + nonCompatibleFiles);
                }
            }

            //checks if there were predictions and open the Pridictions form so they can be displayed
            if (data.processedImagesPath != null)
            {
                if (data.processedImagesPath.Count > 0)
                {
                    Pridictions PridictionsWindow = new Pridictions(data);
                    PridictionsWindow.Show();
                }
            }
        }

        //clears the list box
        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            DropTargetlst.Items.Clear();
        }
    }
}
