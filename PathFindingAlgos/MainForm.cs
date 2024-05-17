using System;
using System.Windows.Forms;

namespace PathFindingAlgos
{
    public partial class MainForm : Form
    {
        private readonly DrawMapToBitmap _mapDrawer;
        private bool _isMouseDown = false;

        public MainForm()
        {
            InitializeComponent();

            _mapDrawer = new DrawMapToBitmap(0.5f, MainPictureBox.Width, MainPictureBox.Height);
            _mapDrawer.MapChanged += MapDrawer_MapChanged;
            MainPictureBox.Image = _mapDrawer.Bitmap;
        }

        private void MapDrawer_MapChanged(object sender, EventArgs e)
        {
            MainPictureBox.Image = _mapDrawer.Bitmap;
        }
        private void MainPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                _mapDrawer?.ChangeTile(e.Location, TileType.Wall);
            }
            else if (e.Button == MouseButtons.Right)
            {
                _mapDrawer?.ChangeTile(e.Location, TileType.Empty);
            }
        }
        private void MainPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
            {
                MainPictureBox_MouseClick(sender, e);
            }
        }
        private void MainPictureBox_MouseDown(object sender, MouseEventArgs e) => _isMouseDown = true;
        private void MainPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
            _mapDrawer?.DrawMap();
        }
        private void MainPictureBox_SizeChanged(object sender, EventArgs e)
        {
            _mapDrawer?.ChangeBitmapSize(MainPictureBox.Width, MainPictureBox.Height);
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _mapDrawer.MapChanged -= MapDrawer_MapChanged;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Space)
            {
                _mapDrawer?.SaveMap();
            }
        }
    }
}
