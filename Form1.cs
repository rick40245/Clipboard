using System.Runtime.InteropServices;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private List<(TextBox, Button, Button)> elements = new List<(TextBox, Button, Button)>();

        public Form1()
        {
            InitializeComponent();

            if (File.Exists("data.txt"))
            {
                string[] lines = File.ReadAllLines("data.txt");

                for (int i = 0; i < lines.Length; i++)
                {
                    AddTextBoxAndButton(i, lines[i]);
                }
            }

        }

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const int KEYEVENTF_KEYUP = 0x0002;
        public const byte VK_MENU = 0x12;
        public const byte VK_TAB = 0x09;
        public const byte VK_CONTROL = 0x11;
        public const byte VK_V = 0x56;


        private void buttonCopy_Click(object sender, EventArgs e)
        {
            // Press Alt+Tab
            keybd_event(VK_MENU, 0x38, KEYEVENTF_EXTENDEDKEY | 0, 0);
            keybd_event(VK_TAB, 0x38, KEYEVENTF_EXTENDEDKEY | 0, 0);
            keybd_event(VK_TAB, 0x38, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            keybd_event(VK_MENU, 0x38, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            // Wait for a second
            System.Threading.Thread.Sleep(100);
            // Press Ctrl+V
            keybd_event(VK_CONTROL, 0, KEYEVENTF_EXTENDEDKEY | 0, 0);
            keybd_event(VK_V, 0, KEYEVENTF_EXTENDEDKEY | 0, 0);
            keybd_event(VK_V, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(elements.Count);

            AddTextBoxAndButton(elements.Count, string.Empty);
        }

        private void AddTextBoxAndButton(int index, string text)
        {
            TextBox newTextBox = new TextBox
            {
                Name = "textBox" + (index + 1),
                Location = new Point(120, 20 + 30 * (index + 1)),
                Size = new Size(500, 30),
                Text = text
            };

            Button newButton = new Button
            {
                Name = "button" + (index + 1),
                Text = "½Æ»s",
                Location = new Point(30, 20 + 30 * (index + 1))
            };

            newButton.Click += (sender, e) =>
            {
                Clipboard.SetText(newTextBox.Text);
                buttonCopy_Click(sender, e);
            };

            Button newDeleteButton = new Button
            {
                Name = "deleteButton" + (index + 1),
                Text = "§R°£",
                Location = new Point(710, 20 + 30 * (index + 1)),
                Tag = index
            };

            newDeleteButton.Click += (sender, e) =>
            {
                Button clickedButton = (Button)sender;

                // Delete the UI elements first
                this.Controls.Remove(newTextBox);
                this.Controls.Remove(newButton);
                this.Controls.Remove(clickedButton);

                // Remove the element from elements
                int removeIndex = (int)clickedButton.Tag;
                elements.RemoveAt(removeIndex);

                // Update locations and tags
                for (int i = removeIndex; i < elements.Count; i++)
                {
                    elements[i].Item1.Location = new Point(120, 20 + 30 * (i + 1));
                    elements[i].Item2.Location = new Point(30, 20 + 30 * (i + 1));
                    elements[i].Item3.Location = new Point(710, 20 + 30 * (i + 1));
                    elements[i].Item3.Tag = i; // Update the tag
                }
            };

            this.Controls.Add(newTextBox);
            this.Controls.Add(newButton);
            this.Controls.Add(newDeleteButton);

            elements.Add((newTextBox, newButton, newDeleteButton));
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            List<string> lines = new List<string>();

            foreach (var tuple in elements)
            {
                lines.Add(tuple.Item1.Text);
            }

            File.WriteAllLines("data.txt", lines);
        }
    }
}
