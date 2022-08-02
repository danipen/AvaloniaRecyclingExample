using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using Avalonia.Threading;

namespace AvaloniaRecyclingExample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ScrollViewer itemsControlScrollViewer = new ScrollViewer();

            mItemsControl = new ItemsControl();
            itemsControlScrollViewer.Content = mItemsControl;

            mListBox = new ListBox();

            mItemsControl.ItemTemplate = new FuncDataTemplate<string>(
                (_, _) => new ItemsControlTemplatePanel(), true);

            mListBox.ItemTemplate = new FuncDataTemplate<string>(
                (_, _) => new ListBoxTemplatePanel(), true);

            List<string> items = new List<string>();
            for (int i = 0; i < 150; i++)
            {
                items.Add("This is the item #" + i);
            }

            mItemsControl.Items = new List<string>(items);
            mListBox.Items = new List<string>(items);

            DockPanel leftPanel = new DockPanel();
            DockPanel rightPanel = new DockPanel();

            mRealizedItemsControlElementsTextBlock = new TextBlock();
            mRealizedListBoxElementsTextBlock = new TextBlock();

            DockPanel.SetDock(mRealizedItemsControlElementsTextBlock, Dock.Bottom);
            DockPanel.SetDock(mRealizedListBoxElementsTextBlock, Dock.Bottom);

            leftPanel.Children.Add(mRealizedItemsControlElementsTextBlock);
            leftPanel.Children.Add(itemsControlScrollViewer);

            rightPanel.Children.Add(mRealizedListBoxElementsTextBlock);
            rightPanel.Children.Add(mListBox);

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

            Grid.SetColumn(leftPanel, 0);
            Grid.SetColumn(rightPanel, 1);

            grid.Children.Add(leftPanel);
            grid.Children.Add(rightPanel);

            this.Content = grid;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            mRealizedItemsControlElementsTextBlock.Text = "Realized ItemsControl elements: " + ItemsControlTemplatePanel.RealizedElements;
            mRealizedListBoxElementsTextBlock.Text = "Realized ListBox elements: " + ListBoxTemplatePanel.RealizedElements;
        }

        class ItemsControlTemplatePanel : DockPanel
        {
            public static int RealizedElements = 0;

            public ItemsControlTemplatePanel()
            {
                this.Background = new SolidColorBrush(Colors.LightBlue);

                Children.Add(mTextBlock);

                RealizedElements++;
            }

            protected override void OnDataContextChanged(EventArgs e)
            {
                base.OnDataContextChanged(e);

                if (DataContext is not string data)
                    return;

                mTextBlock.Text = data;
            }

            TextBlock mTextBlock = new TextBlock();
        }

        class ListBoxTemplatePanel : DockPanel
        {
            public static int RealizedElements = 0;

            public ListBoxTemplatePanel()
            {
                this.Background = new SolidColorBrush(Colors.LightGreen);

                Children.Add(mTextBlock);

                RealizedElements++;
            }

            protected override void OnDataContextChanged(EventArgs e)
            {
                base.OnDataContextChanged(e);

                if (DataContext is not string data)
                    return;

                mTextBlock.Text = data;
            }

            TextBlock mTextBlock = new TextBlock();
        }

        ItemsControl mItemsControl;
        ListBox mListBox;

        TextBlock mRealizedItemsControlElementsTextBlock;
        TextBlock mRealizedListBoxElementsTextBlock;
    }
}