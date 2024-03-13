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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem selectedItem = e.NewValue as TreeViewItem;
            if (selectedItem != null)
            {
                TreeViewItem parent = selectedItem.Parent as TreeViewItem;
                if (parent != null)
                {
                    TreeViewItem grandParent = parent.Parent as TreeViewItem;
                    if (grandParent != null)
                    {
                        int year = int.Parse(grandParent.Header.ToString());
                        int month = int.Parse(parent.Header.ToString());
                        int day = int.Parse(selectedItem.Header.ToString());
                        MessageBox.Show($"Selected date: {year}-{month}-{day}");
                    }
                }
            }
        }
        private void AddMonth_Click(object sender, RoutedEventArgs e)
        {
            AddMonthDialog dialog = new AddMonthDialog();
            if (dialog.ShowDialog() == true)
            {
                int year = dialog.Year;
                int month = dialog.Month;
                TreeViewItem yearItem = FindYearItem(year);
                if (yearItem == null)
                {
                    yearItem = new TreeViewItem { Header = year };
                    treeView.Items.Add(yearItem);
                }
                TreeViewItem monthItem = FindMonthItem(yearItem, month);
                if (monthItem == null)
                {
                    monthItem = new TreeViewItem { Header = month };
                    yearItem.Items.Add(monthItem);
                }
                List<int> weeks = GetWeeks(year, month);
                foreach (int week in weeks)
                {
                    TreeViewItem weekItem = new TreeViewItem { Header = week };
                    monthItem.Items.Add(weekItem);
                    List<int> days = GetDays(year, month, week);
                    foreach (int day in days)
                    {
                        TreeViewItem dayItem = new TreeViewItem { Header = day };
                        weekItem.Items.Add(dayItem);
                    }
                }
            }
        }

        private void CalculateDuration_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem1 = treeView.SelectedItem as TreeViewItem;
            if (selectedItem1 != null)
            {
                TreeViewItem parent1 = selectedItem1.Parent as TreeViewItem;
                if (parent1 != null)
                {
                    TreeViewItem grandParent1 = parent1.Parent as TreeViewItem;
                    if (grandParent1 != null)
                    {
                        int year1 = int.Parse(grandParent1.Header.ToString());
                        int month1 = int.Parse(parent1.Header.ToString());
                        int day1 = int.Parse(selectedItem1.Header.ToString());
                        TreeViewItem selectedItem2 = treeView.SelectedItem as TreeViewItem;
                        if (selectedItem2 != null)
                        {
                            TreeViewItem parent2 = selectedItem2.Parent as TreeViewItem;
                            if (parent2 != null)
                            {
                                TreeViewItem grandParent2 = parent2.Parent as TreeViewItem;
                                if (grandParent2 != null)
                                {
                                    int year2 = int.Parse(grandParent2.Header.ToString());
                                    int month2 = int.Parse(parent2.Header.ToString());
                                    int day2 = int.Parse(selectedItem2.Header.ToString());
                                    DateTime date1 = new DateTime(year1, month1, day1);
                                    DateTime date2 = new DateTime(year2, month2, day2);
                                    TimeSpan duration = date2 - date1;
                                    MessageBox.Show($"Duration: {duration.Days} days");
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CheckLeapYear_Click(object sender, RoutedEventArgs e)
        {
            AddYearDialog dialog = new AddYearDialog();
            if (dialog.ShowDialog() == true)
            {
                int year = dialog.Year;
                if (IsLeapYear(year))
                {
                    MessageBox.Show($"{year} is a leap year");
                }
                else
                {
                    MessageBox.Show($"{year} is not a leap year");
                }
            }
        }

        private TreeViewItem FindYearItem(int year)
        {
            foreach (TreeViewItem item in treeView.Items)
            {
                if (item.Header.ToString() == year.ToString())
                {
                    return item;
                }
            }
            return null;
        }

        private TreeViewItem FindMonthItem(TreeViewItem yearItem, int month)
        {
            foreach (TreeViewItem item in yearItem.Items)
            {
                if (item.Header.ToString() == month.ToString())
                {
                    return item;
                }
            }
            return null;
        }

        private List<int> GetWeeks(int year, int month)
        {
            DateTime date = new DateTime(year, month, 1);
            List<int> weeks = new List<int>();
            int week = 1;
            while (date.DayOfWeek != DayOfWeek.Sunday)
            {
                date = date.AddDays(-1);
                week++;
            }
            while (date.Month == month)
            {
                weeks.Add(week);
                date = date.AddDays(7);
                week++;
            }
            return weeks;
        }

        private List<int> GetDays(int year, int month, int week)
        {
            DateTime date = new DateTime(year, month, 1);
            List<int> days = new List<int>();
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }
            while (date.DayOfWeek != DayOfWeek.Sunday)
            {
                days.Add(date.Day);
                date = date.AddDays(1);
            }
            return days;
        }

        private bool IsLeapYear(int year)
        {
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }
    }

    
    }

