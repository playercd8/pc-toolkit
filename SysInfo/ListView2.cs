using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SysInfo
{
    class ListView2 : ListView
    {
        public ListView2()
        {
            _strToolTip = "";
            _lastShow = DateTime.MinValue;
            MouseMove += new MouseEventHandler(OnMouseMove2);

            ColumnClick += new ColumnClickEventHandler(OnColumnClick2);
        }

        public void TodoAutoResizeColumns() 
        {
            AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            int w = 0;// Width;
            foreach (ColumnHeader c in Columns) {
                w += c.Width;
            }
            if (w < Width) 
                AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        string _strToolTip;
        DateTime _lastShow;

        void OnMouseMove2(object sender, MouseEventArgs e)
        {
            ListViewItem item = GetItemAt(e.X, e.Y);
            ListViewHitTestInfo info = HitTest(e.X, e.Y);

            string strToolTip = "";
            if ((item != null) && (info.SubItem != null))
            {
                if (info.SubItem.Tag != null)
                    strToolTip = info.SubItem.Tag.ToString();
                else if (item.Tag != null)
                    strToolTip = item.Tag.ToString();
                
                if ((_strToolTip != strToolTip) || (_lastShow < DateTime.Now.AddSeconds(-10)))
                {
                    toolTip1.Show(strToolTip, this, e.X, e.Y, 5000);
                    _strToolTip = strToolTip;
                    _lastShow = DateTime.Now;
                }           
            }
            else
            {
                toolTip1.Show("", this);
                _strToolTip = "";
            }
        }

        ToolTip toolTip1  = new ToolTip();

        private int sortColumn = -1;

        private void OnColumnClick2(object sender, ColumnClickEventArgs e)
        {
            // Determine whether the column is the same as the last column clicked.
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.
                sortColumn = e.Column;
                // Set the sort order to ascending by default.
                Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.
                if (Sorting == SortOrder.Ascending)
                    Sorting = SortOrder.Descending;
                else
                    Sorting = SortOrder.Ascending;
            }

            // Set the ListViewItemSorter property to a new ListViewItemComparer
            // object.            
            ListViewItemSorter = new ListViewItemComparer(e.Column, Sorting);

            // Call the sort method to manually sort.
            Sort();
        }


        // Implements the manual sorting of items by column.
        class ListViewItemComparer : System.Collections.IComparer
        {
            private int _column;
            private SortOrder _Sorting;

            public ListViewItemComparer()
            {
                _column = 0;
                _Sorting = SortOrder.Ascending;
            }

            public ListViewItemComparer(int column, SortOrder Sorting)
            {
                _column = column;
                _Sorting = Sorting;
            }

            public int Compare(object x, object y)
            {
                int returnVal = -1;

                //Compare the two items as a string.
                returnVal = String.Compare(
                    ((ListViewItem)x).SubItems[_column].Text,
                    ((ListViewItem)y).SubItems[_column].Text);

                if (_Sorting == SortOrder.Ascending)
                    return returnVal;
                else
                    return -returnVal;
            }
        }
    }
}
