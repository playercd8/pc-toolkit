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
            ColumnClick += new ColumnClickEventHandler(OnColumnClick2);
        }


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

            // Call the sort method to manually sort.
            Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer
            // object.
            ListViewItemSorter = new ListViewItemComparer(e.Column, Sorting);
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
                returnVal = String.Compare(((ListViewItem)x).SubItems[_column].Text,
                ((ListViewItem)y).SubItems[_column].Text);

                if (_Sorting == SortOrder.Ascending)
                    return returnVal;
                else
                    return -returnVal;
            }
        }
    }
}
